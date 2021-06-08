using MVC.Models.Negocio.Usuario;
using MVC.Models.Negocio.Menores;
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
using MVC.Models.Negocio.Test;
using System.Text.RegularExpressions;
using System.Collections;

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
            Session["Test"] = null;
            if (TempData["Mensaje"] != null)
            {
                ViewBag.Mensaje = TempData["Mensaje"].ToString();
            }
            return View(Session["Usuario"]);
        }

        public ActionResult Empleado()
        {
            if (TempData["Mensaje"] != null)
            {
                ViewBag.Mensaje = TempData["Mensaje"].ToString();
            }
            return View(Session["Usuario"]);
        }

        

        [HttpPost]
        public ActionResult SetTest()
        {
            return Json(((OTest)Session["Test"]).Preguntas);
        }

        [HttpPost]
        public JsonResult DatosUsuario()
        {
            return Json((OUsuario)Session["Usuario"]);
        }

        [HttpPost]
        public JsonResult CambiarContraseña(OUsuario PmtPeticion)
        {
            Debug.WriteLine("Se cambio la contraseña");
            PmtPeticion.IdUsuario = ((OUsuario)Session["Usuario"]).IdUsuario;
            return Json(PmtPeticion);
        }

        [HttpPost]
        public ActionResult SetTimeTest()
        {
            if (((OTest)Session["Test"]).FechaInicio == DateTime.MinValue || ((OTest)Session["Test"]).FechaLimite == DateTime.MinValue)
            {
                ((OTest)Session["Test"]).FechaInicio = DateTime.Now;
                ((OTest)Session["Test"]).FechaLimite = ((OTest)Session["Test"]).FechaInicio.AddSeconds(((OTest)Session["Test"]).TiempoDisponible);
                ((OTest)Session["Test"]).TiempoRestante = TimeSpan.FromTicks(((OTest)Session["Test"]).FechaLimite.Ticks - ((OTest)Session["Test"]).FechaInicio.Ticks).TotalSeconds;
            }
            else
            {
                ((OTest)Session["Test"]).TiempoRestante = ((OTest)Session["Test"]).TiempoDisponible - TimeSpan.FromTicks((DateTime.Now.Ticks - ((OTest)Session["Test"]).FechaInicio.Ticks)).TotalSeconds;
            }
            return Json(((OTest)Session["Test"]).TiempoRestante);
        }

        public ActionResult Test()
        {
            return View(Session["Usuario"]);
        }

        [HttpPost]
        public ActionResult ContestarTest(List<OPregunta> pmtPeticion)
        {
            ((OTest)Session["Test"]).FechaFin = DateTime.Now;
            int Errores = 0;
            string MensajeError = "Se encontraron los siguientes errores al validar su prueba:";
            if(((OTest)Session["Test"]).FechaFin > ((OTest)Session["Test"]).FechaLimite)
            {
                MensajeError += "<br />La fecha del test presenta inconsistencias.";
                Errores++;
            }
            if(pmtPeticion.Count < 189)
            {
                MensajeError += "<br />El número de preguntas recibidas presenta inconsistencias.";
                Errores++;
            }
            foreach(OPregunta pregunta in pmtPeticion)
            {
                if(pregunta.IdPregunta > 189 || pregunta.IdPregunta <= 0)
                {
                    MensajeError += "<br />La pregunta número " + pregunta.IdPregunta.ToString() + " presenta inconsistencias.";
                    Errores++;
                }
                if(pregunta.Respuesta > 4 || pregunta.Respuesta <= 0)
                {
                    MensajeError += "<br />La pregunta número " + pregunta.IdPregunta.ToString() + " presenta error en la respuesta.";
                    Errores++;
                }
            }
            if(Errores > 0)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + MensajeError + ".</center></label>');");
                return Json(Url.Action("Solicitante", "Usuario"));
            }
            else
            {
                ((OTest)Session["Test"]).IdUsuario = ((OUsuario)Session["Usuario"]).IdUsuario;
                foreach(OPregunta pregunta in pmtPeticion)
                {
                    ((OTest)Session["Test"]).Preguntas.Where(a => a.IdPregunta == pregunta.IdPregunta).ToList().ForEach(a => a.Respuesta = pregunta.Respuesta);
                }
                try
                {
                    var url = $"https://localhost:44335/api/Usuario/GuardarTest";
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    string json = JsonConvert.SerializeObject(((OTest)Session["Test"]));
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
                                return Json(Url.Action("Solicitante", "Usuario"));
                            }
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                ORespuesta<string> respApi = JsonConvert.DeserializeObject<ORespuesta<string>>(responseBody);
                                if (respApi.Exitoso)
                                {
                                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Se guardó la prueba correctamente.</center></label>');");
                                }
                                else
                                {
                                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + respApi.Mensaje + "</center></label>');");
                                }
                                return Json(Url.Action("Solicitante", "Usuario"));
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + ex.Message + ".</center></label>');");
                    return Json(Url.Action("Solicitante", "Usuario"));
                }
                catch (Exception e)
                {
                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>" + e.Message + ".</center></label>');");
                    return Json(Url.Action("Solicitante", "Usuario"));
                }
            }
        }

        [HttpPost]
        public ActionResult RealizarTest()
        {
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListarPreguntas";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = JsonConvert.SerializeObject(((OUsuario)Session["Usuario"]));
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
                            ORespuesta<OTest> respApi = JsonConvert.DeserializeObject<ORespuesta<OTest>>(responseBody);
                            if (respApi.Exitoso)
                            {
                                respApi.Respuesta[0].TiempoDisponible = 2700;
                                Session["Test"] = respApi.Respuesta[0];
                                return RedirectToAction("Test");
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
            Session["Usuario"] = null;
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
                            ORespuesta<OUsuario> respApi = JsonConvert.DeserializeObject<ORespuesta<OUsuario>>(responseBody);
                            if (respApi.Exitoso)
                            {
                                if (respApi.Respuesta[0].TipoUsuario == 1)
                                {
                                    Session["Usuario"] = respApi.Respuesta[0];
                                    return RedirectToAction("Empleado");
                                }
                                else
                                {
                                    if(respApi.Respuesta[0].IdEstatus == 2)
                                    {
                                        TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El usuario está inactivo. Contacta a un administrador.</center></label>');");
                                        return RedirectToAction("Index", "Principal");
                                    }
                                    else if (respApi.Respuesta[0].IdEstatus == 4)
                                    {
                                        TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El usuario está pendiente de validación.</center></label>');");
                                        return RedirectToAction("Index", "Principal");
                                    }
                                    Session["Usuario"] = respApi.Respuesta[0];
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
                string.IsNullOrEmpty(formCollection["txtPasswordRegistroUsuario"]) || string.IsNullOrEmpty(formCollection["txtConfirmarPasswordRegistroUsuario"]))
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Asegurate de llenar todos los campos correctamente.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
           
            if (CurpValida(formCollection["txtCURPUsuario"]) != true) {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del CURP es incorrecto.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            Match m = Regex.Match(formCollection["txtNombreUsuario"], @"^([a-zA-ZñÑ\s]*){0,150}$", RegexOptions.IgnoreCase);
            if (m.Success==false)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del nombre es incorrecto.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            if (!formCollection["txtPasswordRegistroUsuario"].Equals(formCollection["txtConfirmarPasswordRegistroUsuario"]))
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Las contraseñas no coinciden.</center></label>');");
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
                                ORespuesta<OUsuario> respApi = JsonConvert.DeserializeObject<ORespuesta<OUsuario>>(responseBody);
                                if (respApi.Exitoso)
                                {
                                    Session["Usuario"] = respApi.Respuesta[0];
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

        [HttpPost]
        public ActionResult RegistroEmpleado(FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(formCollection["txtCURPEmpleado"]) || string.IsNullOrEmpty(formCollection["txtNombreEmpleado"]) ||
                DateTime.Parse(formCollection["dtFechaIngreso"]) == DateTime.MinValue || int.Parse(formCollection["sctCentroLabores"]) == 0 ||
                string.IsNullOrEmpty(formCollection["txtTelefonoEmpleado"]) || string.IsNullOrEmpty(formCollection["txtCorreoRegistroEmpleado"]) ||
                string.IsNullOrEmpty(formCollection["txtPasswordRegistroEmpleado"]) || string.IsNullOrEmpty(formCollection["txtConfirmarPasswordRegistroEmpleado"]) ||
                int.Parse(formCollection["sctSexoUsuario2"]) == 0
                )
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Asegurate de llenar todos los campos correctamente.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }

            if (CurpValida(formCollection["txtCURPEmpleado"]) == false)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del CURP es incorrecto.</center></label>');");
                Console.WriteLine("No entró");
                return RedirectToAction("Index", "Principal");
            }
            char[] MyChar = { '_' };

            Match m = Regex.Match(formCollection["txtNombreEmpleado"].TrimEnd(MyChar), @"^([a-zA-ZñÑ\s]*){0,150}$", RegexOptions.IgnoreCase);
            if (m.Success != true)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del nombre es incorrecto.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }

            if (!formCollection["txtPasswordRegistroEmpleado"].Equals(formCollection["txtConfirmarPasswordRegistroEmpleado"]))
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Las contraseñas no coinciden.</center></label>');");
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
                user.CURP = formCollection["txtCURPEmpleado"];
                user.Nombre = formCollection["txtNombreEmpleado"].TrimEnd(MyChar);
                user.FechaIngreso = DateTime.Parse(formCollection["dtFechaIngreso"]);
                user.IdSexo = int.Parse(formCollection["sctSexoUsuario2"]);
                user.CorreoElectronico = formCollection["txtCorreoRegistroEmpleado"];
                user.IdCentroLaboral = int.Parse(formCollection["sctCentroLabores"]);
                user.Telefono = formCollection["txtTelefonoEmpleado"];
                user.Password = formCollection["txtPasswordRegistroEmpleado"];
                try
                {
                    var url = $"https://localhost:44335/api/Usuario/RegistroEmpleado";
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
                                ORespuesta<OUsuario> respApi = JsonConvert.DeserializeObject<ORespuesta<OUsuario>>(responseBody);
                                if (respApi.Exitoso)
                                {
                                    OUsuario sessionUser = respApi.Respuesta[0];
                                    Session["Usuario"] = sessionUser;
                                    return RedirectToAction("Empleado");
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

        [HttpPost]
        public ActionResult ObtenerMenores()
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try 
            {
                var url = $"https://localhost:44335/api/Usuario/ObtenerMenores";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OMenores> respApi = JsonConvert.DeserializeObject<ORespuesta<OMenores>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }


        [HttpPost]
        public ActionResult RegistrarMenor(OMenores PmtPeticion)
        {
            ORespuesta<string> res = new ORespuesta<string>();
            if (string.IsNullOrEmpty(PmtPeticion.nombres) || string.IsNullOrEmpty(PmtPeticion.apellidos) ||
                PmtPeticion.edad >= 18 || PmtPeticion.edad < 0 ||
                PmtPeticion.edadMeses >= 12 || PmtPeticion.edadMeses < 0 ||
                PmtPeticion.idSexo == 0 || PmtPeticion.idCentroAdopcion == 0 ||
                string.IsNullOrEmpty(PmtPeticion.antecedentes) || PmtPeticion.cAl == 0 ||
                PmtPeticion.cAp == 0 || PmtPeticion.cAs == 0 || PmtPeticion.cAt == 0 ||
                PmtPeticion.cRp == 0 || PmtPeticion.cEm == 0 || PmtPeticion.cEe == 0 ||
                PmtPeticion.cFl == 0 || PmtPeticion.cIn == 0 || PmtPeticion.cRf == 0 ||
                PmtPeticion.cSc == 0 || PmtPeticion.cTf == 0 || PmtPeticion.cAg == 0 ||
                PmtPeticion.cDl == 0
                )
            {
                res.Exitoso = false;
                res.Mensaje = "Asegurate de llenar todos los campos correctamente.";
                return Json(res);
            }
            else
            {
                try
                {
                    PmtPeticion.IdUsuario = ((OUsuario)Session["Usuario"]).IdUsuario;
                    var url = $"https://localhost:44335/api/Usuario/RegistrarMenor";
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
                                res.Exitoso = false;
                                res.Mensaje = "El servidor no responde.";
                                return Json(res);
                            }
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                ORespuesta<string> respApi = JsonConvert.DeserializeObject<ORespuesta<string>>(responseBody);
                                return Json(respApi);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    res.Exitoso = false;
                    res.Mensaje = ex.Message;
                    return Json(res);
                }
                catch (Exception e)
                {
                    res.Exitoso = false;
                    res.Mensaje = e.Message;
                    return Json(res);
                }
            }
        }

        private bool CurpValida(string curp)
        {
            var re = @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$";
            Regex rx = new Regex(re, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var validado = rx.IsMatch(curp);

            if (!validado)  //Coincide con el formato general?
                return false;

            //Validar que coincida el dígito verificador
            if (!curp.EndsWith(DigitoVerificador(curp.ToUpper())))
                return false;

            return true; //Validado
        }
        private string DigitoVerificador(string curp17)
        {
            //Fuente https://consultas.curp.gob.mx/CurpSP/
            var diccionario = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            var suma = 0.0;
            var digito = 0.0;
            for (var i = 0; i < 17; i++)
                suma = suma + diccionario.IndexOf(curp17[i]) * (18 - i);
            digito = 10 - suma % 10;
            if (digito == 10) return "0";
            return digito.ToString();
        }

        [HttpPost]
        public ActionResult ListarTest()
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListarTest";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = JsonConvert.SerializeObject(((OUsuario)Session["Usuario"]));
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OTest> respApi = JsonConvert.DeserializeObject<ORespuesta<OTest>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult Match()
        {
            ORespuesta<OMenores> res = new ORespuesta<OMenores>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/Match";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = JsonConvert.SerializeObject(((OUsuario)Session["Usuario"]));
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OMenores> respApi = JsonConvert.DeserializeObject<ORespuesta<OMenores>>(responseBody);
                            respApi.Respuesta = respApi.Respuesta.OrderByDescending(o => o.Porcentaje).ToList();
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult IniciarTramiteAdopcion(OMenores PmtPeticion)
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/IniciarTramiteAdopcion";
                var request = (HttpWebRequest)WebRequest.Create(url);
                OTramiteAdopcion req = new OTramiteAdopcion();
                req.IdUsuario = ((OUsuario)Session["Usuario"]).IdUsuario;
                req.IdMenorAdopcion = PmtPeticion.idMenorAdopcion;
                string json = JsonConvert.SerializeObject(req);
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<string> respApi = JsonConvert.DeserializeObject<ORespuesta<string>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ListaTramitesAdopcionUsuario()
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListaTramitesAdopcionUsuario";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = JsonConvert.SerializeObject(((OUsuario)Session["Usuario"]));
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OTramiteAdopcion> respApi = JsonConvert.DeserializeObject<ORespuesta<OTramiteAdopcion>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult DetalleMenor(OMenores PmtPeticion)
        {
            ORespuesta<OMenores> res = new ORespuesta<OMenores>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/DetalleMenor";
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OMenores> respApi = JsonConvert.DeserializeObject<ORespuesta<OMenores>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ModificarMenor(OMenores PmtPeticion)
        {
            ORespuesta<string> res = new ORespuesta<string>();
            if (string.IsNullOrEmpty(PmtPeticion.nombres) || string.IsNullOrEmpty(PmtPeticion.apellidos) ||
                PmtPeticion.edad >= 18 || PmtPeticion.edad < 0 ||
                PmtPeticion.edadMeses >= 12 || PmtPeticion.edadMeses < 0 ||
                PmtPeticion.idSexo == 0 || PmtPeticion.idCentroAdopcion == 0 ||
                string.IsNullOrEmpty(PmtPeticion.antecedentes) || PmtPeticion.cAl == 0 ||
                PmtPeticion.cAp == 0 || PmtPeticion.cAs == 0 || PmtPeticion.cAt == 0 ||
                PmtPeticion.cRp == 0 || PmtPeticion.cEm == 0 || PmtPeticion.cEe == 0 ||
                PmtPeticion.cFl == 0 || PmtPeticion.cIn == 0 || PmtPeticion.cRf == 0 ||
                PmtPeticion.cSc == 0 || PmtPeticion.cTf == 0 || PmtPeticion.cAg == 0 ||
                PmtPeticion.cDl == 0
                )
            {
                res.Exitoso = false;
                res.Mensaje = "Asegurate de llenar todos los campos correctamente.";
                return Json(res);
            }
            else
            {
                try
                {
                    PmtPeticion.IdUsuario = ((OUsuario)Session["Usuario"]).IdUsuario;
                    var url = $"https://localhost:44335/api/Usuario/ModificarMenor";
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
                                res.Mensaje = "El servidor no responde.";
                                res.Exitoso = false;
                                return Json(res);
                            }
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                string responseBody = objReader.ReadToEnd();
                                ORespuesta<string> respApi = JsonConvert.DeserializeObject<ORespuesta<string>>(responseBody);
                                return Json(respApi);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    res.Mensaje = ex.Message;
                    res.Exitoso = false;
                    return Json(res);
                }
                catch (Exception e)
                {
                    res.Mensaje = e.Message;
                    res.Exitoso = false;
                    return Json(res);
                }
            }
        }

        [HttpPost]
        public ActionResult ListaTramitesAdopcion()
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListaTramitesAdopcion";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OTramiteAdopcion> respApi = JsonConvert.DeserializeObject<ORespuesta<OTramiteAdopcion>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ModificarEstatusTramite(OTramiteAdopcion PmtPeticion)
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ModificarEstatusTramite";
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<string> respApi = JsonConvert.DeserializeObject<ORespuesta<string>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ListaEvaluaciones()
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListaEvaluaciones";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OUsuario> respApi = JsonConvert.DeserializeObject<ORespuesta<OUsuario>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ModificarSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                PmtPeticion.IdUsuario = ((OUsuario)Session["Usuario"]).IdUsuario;
                var url = $"https://localhost:44335/api/Usuario/ModificarSolicitante";
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OUsuario> respApi = JsonConvert.DeserializeObject<ORespuesta<OUsuario>>(responseBody);
                            if (respApi.Exitoso)
                            {
                                Session["Usuario"] = respApi.Respuesta[0];
                            }
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ModificarEstatusSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ModificarEstatusSolicitante";
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
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<string> respApi = JsonConvert.DeserializeObject<ORespuesta<string>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }

        [HttpPost]
        public ActionResult ListaSolicitantes()
        {
            ORespuesta<string> res = new ORespuesta<string>();
            try
            {
                var url = $"https://localhost:44335/api/Usuario/ListaSolicitantes";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                        {
                            res.Mensaje = "El servidor no responde.";
                            res.Exitoso = false;
                            return Json(res);
                        }
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            ORespuesta<OUsuario> respApi = JsonConvert.DeserializeObject<ORespuesta<OUsuario>>(responseBody);
                            return Json(respApi);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                res.Mensaje = ex.Message;
                res.Exitoso = false;
                return Json(res);
            }
            catch (Exception e)
            {
                res.Mensaje = e.Message;
                res.Exitoso = false;
                return Json(res);
            }
        }
    } 
}