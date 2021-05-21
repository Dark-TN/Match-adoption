﻿using MVC.Models.Negocio.Usuario;
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

        public ActionResult Test()
        {
            if (((SolicitanteViewModel)Session["Solicitante"]).Test.FechaInicio == DateTime.MinValue || ((SolicitanteViewModel)Session["Solicitante"]).Test.FechaLimite == DateTime.MinValue)
            {
                ((SolicitanteViewModel)Session["Solicitante"]).Test.FechaInicio = DateTime.Now.AddSeconds(2); //Se agregan 2 segundos para compensar el tiempo de respuesta 
                ((SolicitanteViewModel)Session["Solicitante"]).Test.FechaLimite = ((SolicitanteViewModel)Session["Solicitante"]).Test.FechaInicio.AddSeconds(((SolicitanteViewModel)Session["Solicitante"]).Test.TiempoDisponible);
                ((SolicitanteViewModel)Session["Solicitante"]).Test.TiempoRestante = TimeSpan.FromTicks(((SolicitanteViewModel)Session["Solicitante"]).Test.FechaLimite.Ticks - ((SolicitanteViewModel)Session["Solicitante"]).Test.FechaInicio.Ticks).TotalSeconds;
            }
            else
            {
                ((SolicitanteViewModel)Session["Solicitante"]).Test.TiempoRestante = ((SolicitanteViewModel)Session["Solicitante"]).Test.TiempoDisponible - TimeSpan.FromTicks((DateTime.Now.Ticks - ((SolicitanteViewModel)Session["Solicitante"]).Test.FechaInicio.Ticks)).TotalSeconds;
            }
            return View(Session["Solicitante"]);
        }

        [HttpPost]
        public ActionResult ContestarTest(SolicitanteViewModel PmtPeticion)
        {
            ((SolicitanteViewModel)Session["Solicitante"]).Test = new OTest();
            return RedirectToAction("Solicitante");
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
                                test.TiempoDisponible = 2700;
                                foreach (OPregunta pregunta in test.Preguntas)
                                {
                                    pregunta.Respuesta = Enumerable.Repeat(false, 4).ToList();
                                }
                                ((SolicitanteViewModel)Session["Solicitante"]).Test = test;
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
            if (m.Success==false)
            {
                TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>El formato del CURP es incorrecto.</center></label>');");
                return RedirectToAction("Index", "Principal");
            }
            m = Regex.Match(formCollection["txtNombreUsuario"], @"^([a-zA-ZñÑ\s]*){0,150}$", RegexOptions.IgnoreCase);
            if (m.Success==false)
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
            Debug.WriteLine(formCollection["txtNombreEmpleado"].TrimEnd(MyChar));
           
            Match m = Regex.Match(formCollection["txtNombreEmpleado"].TrimEnd(MyChar), @"^([a-zA-ZñÑ\s]*){0,150}$", RegexOptions.IgnoreCase);
            Debug.WriteLine(m.Success);
            if (m.Success != true)
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
                                ORespuesta respApi = JsonConvert.DeserializeObject<ORespuesta>(responseBody);
                                if (respApi.Exitoso)
                                {
                                    Debug.WriteLine("Existoso");
                                    TempData["Mensaje"] = string.Format("bootbox.alert('<center><label>Empleado registrado correctamente.</center></label>');");
                                    return RedirectToAction("Index", "Principal");
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

    } 

}