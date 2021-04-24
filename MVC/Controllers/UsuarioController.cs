using MVC.Models.Negocio.Usuario;
using MVC.Models.Peticion;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models.ViewModels;
using MVC.Models.Negocio.Test;
using System.Text.RegularExpressions;

namespace MVC.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Solicitante()
        {
            if (TempData["Mensaje"] != null)
            {
                ViewBag.Mensaje = TempData["Mensaje"].ToString();
            }
            return View(Session["Solicitante"]);
        }

        [HttpPost]
        public ActionResult RealizarTest()
        {
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListarPreguntas";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = JsonConvert.SerializeObject(((SolicitanteViewModel)Session["Solicitante"]).Usuario);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El servidor no responde.</center></label>');");
                            return RedirectToAction("Solicitante");
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta respApi = JsonConvert.DeserializeObject<ORespuesta>(responseBody);
                            if (respApi.Exitoso)
                            {
                                OTest test = JsonConvert.DeserializeObject<OTest>(respApi.Respuesta[0].ToString());
                                foreach(OPregunta pregunta in test.Preguntas)
                                {
                                    pregunta.Respuesta = Enumerable.Repeat(false, 4).ToList();
                                }
                                ((SolicitanteViewModel)Session["Solicitante"]).Test = test;
                                DateTime endTime = DateTime.UtcNow.AddSeconds(2700);
                                double tiempoRestante = TimeSpan.FromTicks(endTime.Ticks - DateTime.UtcNow.Ticks).TotalSeconds;
                                TempData["TiempoTest"] = tiempoRestante;
                                return View(Session["Solicitante"]);
                            }
                            else
                            {
                                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + respApi.Mensaje + "</center></label>');");
                                return RedirectToAction("Solicitante");
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + ex.Message + ".</center></label>');");
                return RedirectToAction("Solicitante");
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + e.Message + ".</center></label>');");
                return RedirectToAction("Solicitante");
            }
        }

        public ActionResult Logout()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Response.Cookies.Clear();
            Session.Clear();
            Session.RemoveAll();
            Session["Solicitante"] = null;
            return RedirectToAction("Index", "Principal");
        }


        [HttpPost]
        public ActionResult Login(OUsuario PmtPeticion)
        {
            if (string.IsNullOrEmpty(PmtPeticion.CorreoElectronico) || string.IsNullOrEmpty(PmtPeticion.Password))
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Asegurate de llenar todos los campos correctamente.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            try
            {
                var url = $"https://localhost:44335/api/Usuario/Login";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = JsonConvert.SerializeObject(PmtPeticion);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El servidor no responde.</center></label>');");
                            return RedirectToAction("Index", "Principal");
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta respApi = JsonConvert.DeserializeObject<ORespuesta>(responseBody);
                            if (respApi.Exitoso)
                            {
                                OUsuario sessionUser = JsonConvert.DeserializeObject<OUsuario>(respApi.Respuesta[0].ToString());
                                if (sessionUser.TipoUsuario == 1)
                                {
                                    return RedirectToAction("Solicitante");
                                }
                                else
                                {
                                    SolicitanteViewModel solicitante = new SolicitanteViewModel();
                                    solicitante.Usuario = sessionUser;
                                    Session["Solicitante"] = solicitante;
                                    return RedirectToAction("Solicitante");
                                }
                            }
                            else
                            {
                                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + respApi.Mensaje + "</center></label>');");
                                return RedirectToAction("Index", "Principal");
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + ex.Message + ".</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            catch (Exception e)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + e.Message + ".</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
        }

        [HttpPost]
        public ActionResult RegistroSolicitante(FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["txtCURPUsuario"]) || string.IsNullOrEmpty(formCollection["txtNombreUsuario"]) ||
                DateTime.Parse(formCollection["dtFechaNacimientoUsuario"]) == DateTime.MinValue || int.Parse(formCollection["sctSexoUsuario"]) == 0 ||
                int.Parse(formCollection["sctEstadoCivil"]) == 0 || string.IsNullOrEmpty(formCollection["txtCorreoRegistroUsuario"]) ||
                int.Parse(formCollection["sctNivelEstudios"]) == 0 || string.IsNullOrEmpty(formCollection["txtOcupacion"]) ||
                string.IsNullOrEmpty(formCollection["txtDireccion"]) || string.IsNullOrEmpty(formCollection["txtTelefonoUsuario"]) ||
                string.IsNullOrEmpty(formCollection["txtPasswordRegistroUsuario"]))
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Asegurate de llenar todos los campos correctamente.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            Match m = Regex.Match(formCollection["txtCURPUsuario"], @"^[a-zA-Z]{3,4}[0-9]{6}[hmHM]{1}[a-zA-Z]{1,2}[a-zA-z]{3}[0-9]{2}$", RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del CURP es incorrecto.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            m = Regex.Match(formCollection["txtNombreUsuario"], @"[a-zA-ZñÑ ]{150}", RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del nombre es incorrecto.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            string secretKey = System.Web.Configuration.WebConfigurationManager.AppSettings["reCaptchaPrivateKey"];
            OReCaptcha reCaptcha;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + secretKey + "&response=" + formCollection["g-recaptcha-response"]);
            using (WebResponse wResponse = req.GetResponse())
            {
                using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                {
                    string response = readStream.ReadToEnd();
                    reCaptcha = JsonConvert.DeserializeObject<OReCaptcha>(response);

                }
            }
            if (reCaptcha.Success)
            {
                OUsuario user = new OUsuario();
                user.CURP = formCollection["txtCURPUsuario"];
                user.Nombre = formCollection["txtNombreUsuario"];
                user.FechaNacimiento = DateTime.Parse(formCollection["dtFechaNacimientoUsuario"]);
                user.IdSexo = int.Parse(formCollection["sctSexoUsuario"]);
                user.IdEstadoCivil = int.Parse(formCollection["sctEstadoCivil"]);
                user.IdNivelEstudios = int.Parse(formCollection["sctNivelEstudios"]);
                user.CorreoElectronico = formCollection["txtCorreoRegistroUsuario"];
                user.Ocupacion = formCollection["txtOcupacion"];
                user.Direccion = formCollection["txtDireccion"];
                user.Telefono = formCollection["txtTelefonoUsuario"];
                user.Password = formCollection["txtPasswordRegistroUsuario"];
                try
                {
                    var url = $"https://localhost:44335/api/Usuario/RegistroSolicitante";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    string json = JsonConvert.SerializeObject(user);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream strReader = response.GetResponseStream())
                        {
                            if (strReader == null)
                            {
                                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El servidor no responde.</center></label>');");
                                return RedirectToAction("Index", "Principal");
                            }
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                ORespuesta respApi = JsonConvert.DeserializeObject<ORespuesta>(responseBody);
                                if (respApi.Exitoso)
                                {
                                    OUsuario sessionUser = JsonConvert.DeserializeObject<OUsuario>(respApi.Respuesta[0].ToString());
                                    SolicitanteViewModel solicitante = new SolicitanteViewModel();
                                    solicitante.Usuario = sessionUser;
                                    Session["Solicitante"] = solicitante;
                                    return RedirectToAction("Solicitante");
                                }
                                else
                                {
                                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + respApi.Mensaje + "</center></label>');");
                                    return RedirectToAction("Index", "Principal");
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + ex.Message + ".</center></label>');");
                    return RedirectToAction("Index", "Principal");
                }
                catch (Exception e)
                {
                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + e.Message + ".</center></label>');");
                    return RedirectToAction("Index", "Principal");
                }
            }
            else
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Verificación Captcha requerida.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
        }
    }
}