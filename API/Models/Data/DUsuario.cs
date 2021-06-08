using API.Models.Negocio.Catalogo;
using API.Models.Negocio.Seguridad.EncriptadoSimetrico;
using API.Models.Negocio.Test;
using API.Models.Negocio.Usuario;
using API.Models.Negocio.Menores;
using API.Models.Peticion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using API.Models.Negocio.Recomendacion;

namespace API.Models.Data
{
    public class DUsuario
    {
        private readonly string cadenaConexionLocal;
        public DUsuario()
        {
            cadenaConexionLocal = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        public ORespuesta<OUsuario> Login(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> Ls = new ORespuesta<OUsuario>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@email", PmtPeticion.CorreoElectronico}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_login", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        OUsuario user = new OUsuario();
                        user.IdUsuario = int.Parse(ds.Tables[0].Rows[0]["idUsuario"].ToString());
                        user.TipoUsuario = int.Parse(ds.Tables[0].Rows[0]["idTipoUsuario"].ToString());
                        user.Nombre = ds.Tables[0].Rows[0]["nombre"].ToString();
                        user.IdEstadoCivil = int.Parse(ds.Tables[0].Rows[0]["idEstadoCivil"].ToString());
                        user.Ocupacion = ds.Tables[0].Rows[0]["ocupacion"].ToString();
                        user.IdNivelEstudios = int.Parse(ds.Tables[0].Rows[0]["idNivelEstudios"].ToString());
                        user.CorreoElectronico = ds.Tables[0].Rows[0]["email"].ToString();
                        user.Telefono = ds.Tables[0].Rows[0]["telefono"].ToString();
                        user.CURP = ds.Tables[0].Rows[0]["curp"].ToString();
                        user.Direccion = ds.Tables[0].Rows[0]["direccion"].ToString();
                        user.IdSexo = int.Parse(ds.Tables[0].Rows[0]["idSexo"].ToString());
                        user.IdEstatus = int.Parse(ds.Tables[0].Rows[0]["idEstatusUsuario"].ToString());
                        user.FechaNacimiento = DateTime.Parse(ds.Tables[0].Rows[0]["fechaNacimiento"].ToString());
                        user.PasswordEncriptada = ds.Tables[0].Rows[0]["password"].ToString();
                        user.PasswordPrivada = ds.Tables[0].Rows[0]["passwordPrivada"].ToString();
                        if (user.IdEstatus == 2)
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El usuario está inactivo. Contacta a un administrador.";
                        }
                        else if (user.IdEstatus == 4)
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El usuario está pendiente de validación.";
                        }
                        else
                        {
                            if (PmtPeticion.Password.Equals(OEncriptadoSimetrico.Decrypt<RijndaelManaged>(user.PasswordEncriptada, user.PasswordPrivada)))
                            {
                                Ls.Exitoso = true;
                                Ls.Respuesta.Add(user);
                            }
                            else
                            {
                                Ls.Exitoso = false;
                                Ls.Mensaje = "La contraseña es incorrecta.";
                            }
                        }
                    }
                    else
                    {
                        Ls.Exitoso = false;
                        Ls.Mensaje = "El correo electrónico es incorrecto.";
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OUsuario> RegistroUsuario(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> Ls = new ORespuesta<OUsuario>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@curp", PmtPeticion.CURP},
                    {"@email", PmtPeticion.CorreoElectronico}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_usuario_existente", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int res = int.Parse(ds.Tables[0].Rows[0]["result"].ToString());
                        if (res > 1)
                        {
                            Ls.Exitoso = false;
                            if (res == 2)
                            {
                                Ls.Mensaje = "El CURP y el correo electrónico proporcionados ya están registrados.";
                            }
                            else if (res == 3)
                            {
                                Ls.Mensaje = "El CURP proporcionado ya está registrado.";
                            }
                            else
                            {
                                Ls.Mensaje = "El correo electrónico proporcionado ya está registrado.";
                            }
                        }
                        else
                        {
                            PmtPeticion.GenerarPasswordPrivada();
                            PmtPeticion.PasswordEncriptada = OEncriptadoSimetrico.Encrypt<RijndaelManaged>(PmtPeticion.Password, PmtPeticion.PasswordPrivada);
                            OUsuario user = new OUsuario();
                            Parametros = new Hashtable()
                            {
                                {"@curp", PmtPeticion.CURP},
                                {"@nombre", PmtPeticion.Nombre },
                                {"@fechaNacimiento", PmtPeticion.FechaNacimiento },
                                {"@idSexo", PmtPeticion.IdSexo },
                                {"@idEstadoCivil", PmtPeticion.IdEstadoCivil },
                                {"@idNivelEstudios", PmtPeticion.IdNivelEstudios },
                                {"@ocupacion", PmtPeticion.Ocupacion },
                                {"@direccion", PmtPeticion.Direccion },
                                {"@telefono", PmtPeticion.Telefono },
                                {"@email", PmtPeticion.CorreoElectronico},
                                {"@password", PmtPeticion.PasswordEncriptada },
                                {"@passwordPrivada", PmtPeticion.PasswordPrivada }
                            };
                            ds = DB.EjecutaProcedimientoAlmacenado("sp_insert_registro_solicitante", Parametros, cadenaConexionLocal);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                res = int.Parse(ds.Tables[0].Rows[0]["idUsuario"].ToString());
                                user.IdUsuario = res;
                            }
                            user.TipoUsuario = 2;
                            user.CURP = PmtPeticion.CURP;
                            user.Nombre = PmtPeticion.Nombre;
                            user.FechaNacimiento = PmtPeticion.FechaNacimiento;
                            user.IdSexo = PmtPeticion.IdSexo;
                            user.IdEstadoCivil = PmtPeticion.IdEstadoCivil;
                            user.IdNivelEstudios = PmtPeticion.IdNivelEstudios;
                            user.Ocupacion = PmtPeticion.Ocupacion;
                            user.Direccion = PmtPeticion.Direccion;
                            user.Telefono = PmtPeticion.Telefono;
                            user.CorreoElectronico = PmtPeticion.CorreoElectronico;
                            Ls.Exitoso = true;
                            Ls.Respuesta.Add(user);
                        }
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OTest> ListarPreguntas(OUsuario PmtPeticion)
        {
            ORespuesta<OTest> Ls = new ORespuesta<OTest>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_preguntas", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        int _Dato = int.Parse(ds.Tables[0].Rows[0]["result"].ToString());
                        Ls.Exitoso = false;
                        if (_Dato == 1)
                        {
                            Ls.Mensaje = "Has alcanzado el número máximo de intentos.";
                        }
                        else
                        {
                            Ls.Mensaje = "Debes esperar al menos 3 meses a partir de la fecha de término de tu último test para poder realizar otro intento.";
                        }
                    }
                    else if (ds.Tables[0].Rows.Count > 1)
                    {
                        OTest test = new OTest();
                        test.IdUsuario = PmtPeticion.IdUsuario;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OPregunta _Dato = new OPregunta();
                            _Dato.IdPregunta = int.Parse(row["idPregunta"].ToString());
                            _Dato.IdHabilidad = int.Parse(row["idHabilidad"].ToString());
                            _Dato.Pregunta = row["pregunta"].ToString();
                            test.Preguntas.Add(_Dato);
                        }
                        Ls.Exitoso = true;
                        Ls.Respuesta.Add(test);
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<string> GuardarTest(OTest PmtPeticion)
        {
            ORespuesta<string> Ls = new ORespuesta<string>();
            try
            {
                int Errores = 0;
                string MensajeError = "Se encontraron los siguientes errores al validar su prueba:";
                if (PmtPeticion.FechaFin > PmtPeticion.FechaLimite)
                {
                    MensajeError += "<br />La fecha del test presenta inconsistencias.";
                    Errores++;
                }
                if (PmtPeticion.Preguntas.Count < 189)
                {
                    MensajeError += "<br />El número de preguntas recibidas presenta inconsistencias.";
                    Errores++;
                }
                foreach (OPregunta pregunta in PmtPeticion.Preguntas)
                {
                    if (pregunta.IdPregunta > 189 || pregunta.IdPregunta <= 0)
                    {
                        MensajeError += "<br />La pregunta número " + pregunta.IdPregunta.ToString() + " presenta inconsistencias.";
                        Errores++;
                    }
                    if (pregunta.Respuesta > 4 || pregunta.Respuesta <= 0)
                    {
                        MensajeError += "<br />La pregunta número " + pregunta.IdPregunta.ToString() + " presenta error en la respuesta.";
                        Errores++;
                    }
                }
                if (Errores > 0)
                {
                    Ls.Exitoso = false;
                    Ls.Mensaje = MensajeError;
                }
                else
                {
                    PmtPeticion.CalificarTest();
                    Hashtable Parametros = new Hashtable()
                    {
                        {"@idUsuario", PmtPeticion.IdUsuario},
                        {"@fechaInicio", PmtPeticion.FechaInicio },
                        {"@fechaFin", PmtPeticion.FechaFin },
                        {"@idEstiloCrianza", PmtPeticion.IdEstiloCrianza },
                        {"@cAl", PmtPeticion.CalificacionesHabilidades["14"] },
                        {"@cAp", PmtPeticion.CalificacionesHabilidades["1"] },
                        {"@cAs", PmtPeticion.CalificacionesHabilidades["15"] },
                        {"@cAt", PmtPeticion.CalificacionesHabilidades["3"] },
                        {"@cRp", PmtPeticion.CalificacionesHabilidades["13"] },
                        {"@cEm", PmtPeticion.CalificacionesHabilidades["8"] },
                        {"@cEe", PmtPeticion.CalificacionesHabilidades["2"] },
                        {"@cIn", PmtPeticion.CalificacionesHabilidades["4"] },
                        {"@cFl", PmtPeticion.CalificacionesHabilidades["9"] },
                        {"@cRf", PmtPeticion.CalificacionesHabilidades["7"] },
                        {"@cSc", PmtPeticion.CalificacionesHabilidades["6"] },
                        {"@cTf", PmtPeticion.CalificacionesHabilidades["11"] },
                        {"@cAg", PmtPeticion.CalificacionesHabilidades["16"] },
                        {"@cDl", PmtPeticion.CalificacionesHabilidades["12"] },
                        {"@cCr", PmtPeticion.CalificacionesHabilidades["17"] },
                        {"@cDs", PmtPeticion.CalificacionesHabilidades["5"] },
                        {"@cVl", PmtPeticion.CalificacionesHabilidades["10"] }
                    };
                    DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_insert_test", Parametros, cadenaConexionLocal);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int _Dato = int.Parse(ds.Tables[0].Rows[0]["result"].ToString());
                            Ls.Exitoso = false;
                            if (_Dato == -1)
                            {
                                Ls.Mensaje = "Has alcanzado el número máximo de intentos.";
                            }
                            else if (_Dato == -2)
                            {
                                Ls.Mensaje = "Debes esperar al menos 3 meses a partir de la fecha de término de tu último test para poder realizar otro intento.";
                            }
                            else
                            {
                                PmtPeticion.IdTest = _Dato;
                                foreach (OPregunta pregunta in PmtPeticion.Preguntas)
                                {
                                    Parametros = new Hashtable()
                                    {
                                        {"@idTest", PmtPeticion.IdTest},
                                        {"@idPregunta", pregunta.IdPregunta },
                                        {"@respuesta", pregunta.Respuesta }
                                    };
                                    ds = DB.EjecutaProcedimientoAlmacenado("sp_insert_respuestas_test", Parametros, cadenaConexionLocal);
                                }
                                Ls.Exitoso = true;
                            }
                        }
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OUsuario> ModificarSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> Ls = new ORespuesta<OUsuario>();
            try
            {
                if (string.IsNullOrEmpty(PmtPeticion.Nombre) || string.IsNullOrEmpty(PmtPeticion.Nombre) ||
                PmtPeticion.FechaNacimiento == DateTime.MinValue || PmtPeticion.IdSexo == 0 ||
                PmtPeticion.IdEstadoCivil == 0 || string.IsNullOrEmpty(PmtPeticion.CorreoElectronico) ||
                PmtPeticion.IdNivelEstudios == 0 || string.IsNullOrEmpty(PmtPeticion.Ocupacion) ||
                string.IsNullOrEmpty(PmtPeticion.Direccion) || string.IsNullOrEmpty(PmtPeticion.Telefono) ||
                string.IsNullOrEmpty(PmtPeticion.Password))
                {
                    Ls.Exitoso = false;
                    Ls.Mensaje = "Asegurate de llenar todos los datos correctamente";
                    return Ls;
                }
                if (!string.IsNullOrEmpty(PmtPeticion.NuevaPassword) && !PmtPeticion.NuevaPassword.Equals(PmtPeticion.ConfirmarPassword))
                {
                    Ls.Exitoso = false;
                    Ls.Mensaje = "Las contraseñas no coinciden";
                    return Ls;
                }
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_user_password", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        OUsuario user = new OUsuario();
                        user.PasswordEncriptada = ds.Tables[0].Rows[0]["password"].ToString();
                        user.PasswordPrivada = ds.Tables[0].Rows[0]["passwordPrivada"].ToString();
                        if (PmtPeticion.Password.Equals(OEncriptadoSimetrico.Decrypt<RijndaelManaged>(user.PasswordEncriptada, user.PasswordPrivada)))
                        {
                            Parametros = new Hashtable()
                            {
                                { "@idUsuario", PmtPeticion.IdUsuario },
                                { "@usuario", PmtPeticion.CorreoElectronico },
                                { "@nombre", PmtPeticion.Nombre },
                                { "@idEstadoCivil", PmtPeticion.IdEstadoCivil },
                                { "@ocupacion", PmtPeticion.Ocupacion },
                                { "@idNivelEstudios", PmtPeticion.IdNivelEstudios },
                                { "@email", PmtPeticion.CorreoElectronico },
                                { "@telefono", PmtPeticion.Telefono },
                                { "@curp", PmtPeticion.CURP },
                                { "@direccion", PmtPeticion.Direccion },
                                { "@idSexo", PmtPeticion.IdSexo },
                                { "@fechaNacimiento", PmtPeticion.FechaNacimiento }
                            };
                            ds = DB.EjecutaProcedimientoAlmacenado("sp_update_solicitante", Parametros, cadenaConexionLocal);
                            if(!string.IsNullOrEmpty(PmtPeticion.NuevaPassword))
                            {
                                PmtPeticion.GenerarPasswordPrivada();
                                PmtPeticion.PasswordEncriptada = OEncriptadoSimetrico.Encrypt<RijndaelManaged>(PmtPeticion.NuevaPassword, PmtPeticion.PasswordPrivada);
                                Parametros = new Hashtable()
                                {
                                    { "@idUsuario", PmtPeticion.IdUsuario },
                                    { "@password", PmtPeticion.PasswordEncriptada },
                                    { "@passwordPrivada", PmtPeticion.PasswordPrivada }
                                };
                                ds = DB.EjecutaProcedimientoAlmacenado("sp_update_password", Parametros, cadenaConexionLocal);
                            }
                            PmtPeticion.Password = string.Empty;
                            PmtPeticion.NuevaPassword = string.Empty;
                            PmtPeticion.PasswordEncriptada = string.Empty;
                            Ls.Exitoso = true;
                            Ls.Respuesta.Add(PmtPeticion);
                        }
                        else
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "La contraseña es incorrecta.";
                        }
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }


        public ORespuesta<OUsuario> RegistroEmpleado(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> Ls = new ORespuesta<OUsuario>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@curp", PmtPeticion.CURP},
                    {"@email", PmtPeticion.CorreoElectronico},
                    {"@idCentroAdopcion", PmtPeticion.IdCentroLaboral }
                };

                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_empleado_verificador", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int res = int.Parse(ds.Tables[0].Rows[0]["result"].ToString());
                        if (res == 0)
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El CURP no se encuentra registrado como empleado, contacte a su Centro";
                        }
                        else if (res > 1)
                        {
                            if (res == 2)
                            {
                                Ls.Mensaje = "El CURP y el correo electrónico proporcionados ya están registrados.";
                            }
                            else if (res == 3)
                            {
                                Ls.Mensaje = "El CURP proporcionado ya está registrado.";
                            }
                            else
                            {
                                Ls.Mensaje = "El correo electrónico proporcionado ya está registrado.";
                            }
                        }
                        else
                        {
                            PmtPeticion.GenerarPasswordPrivada();
                            PmtPeticion.PasswordEncriptada = OEncriptadoSimetrico.Encrypt<RijndaelManaged>(PmtPeticion.Password, PmtPeticion.PasswordPrivada);
                            OUsuario user = new OUsuario();
                            Parametros = new Hashtable()
                            {
                                {"@curp", PmtPeticion.CURP},
                                {"@nombre", PmtPeticion.Nombre },
                                {"@fechaIngreso", PmtPeticion.FechaIngreso },
                                {"@idSexo", PmtPeticion.IdSexo },
                                {"@idCentroLaboral", PmtPeticion.IdCentroLaboral},
                                {"@telefono", PmtPeticion.Telefono },
                                {"@email", PmtPeticion.CorreoElectronico},
                                {"@password", PmtPeticion.PasswordEncriptada },
                                {"@passwordPrivada", PmtPeticion.PasswordPrivada }
                            };
                            ds = DB.EjecutaProcedimientoAlmacenado("sp_select_registro_empleado", Parametros, cadenaConexionLocal);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                res = int.Parse(ds.Tables[0].Rows[0]["idUsuario"].ToString());
                                user.IdUsuario = res;
                            }
                            user.TipoUsuario = 1;
                            user.CURP = PmtPeticion.CURP;
                            user.Nombre = PmtPeticion.Nombre;
                            user.FechaIngreso = PmtPeticion.FechaIngreso;
                            user.IdSexo = PmtPeticion.IdSexo;
                            user.IdCentroLaboral = PmtPeticion.IdCentroLaboral;
                            user.Telefono = PmtPeticion.Telefono;
                            user.CorreoElectronico = PmtPeticion.CorreoElectronico;
                            Ls.Exitoso = true;
                            Ls.Respuesta.Add(user);
                        }
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }


        public ORespuesta<string> RegistrarMenor(OMenores PmtPeticion)
        {
            ORespuesta<string> Ls = new ORespuesta<string>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@nombres", PmtPeticion.nombres},
                    {"@apellidos", PmtPeticion.apellidos},
                    {"@idSexo", PmtPeticion.idSexo },
                    {"@idCentroAdopcion", PmtPeticion.idCentroAdopcion }
                };
                Debug.WriteLine(PmtPeticion.nombres);
                Debug.WriteLine(PmtPeticion.apellidos);
                Debug.WriteLine(PmtPeticion.idSexo);
                Debug.WriteLine(PmtPeticion.idCentroAdopcion);

                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_verify_menor_register", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int res = int.Parse(ds.Tables[0].Rows[0]["result"].ToString());
                        if (res == 0)
                        {
                            Parametros = new Hashtable()
                            {
                                {"@IdUsuario", PmtPeticion.IdUsuario },
                                {"@nombres", PmtPeticion.nombres},
                                {"@apellidos", PmtPeticion.apellidos},
                                {"@idSexo", PmtPeticion.idSexo },
                                {"@antecedentes", PmtPeticion.antecedentes },
                                {"@idCentroAdopcion", PmtPeticion.idCentroAdopcion },
                                {"@cAl", PmtPeticion.cAl },
                                {"@cAp", PmtPeticion.cAp },
                                {"@cAs", PmtPeticion.cAs },
                                {"@cAt", PmtPeticion.cAs },
                                {"@cRp", PmtPeticion.cRp },
                                {"@cEm", PmtPeticion.cEm },
                                {"@cEe", PmtPeticion.cEe },
                                {"@cIn", PmtPeticion.cIn },
                                {"@cFl", PmtPeticion.cFl },
                                {"@cRf", PmtPeticion.cRf },
                                {"@cSc", PmtPeticion.cSc },
                                {"@cTf", PmtPeticion.cTf },
                                {"@cAg", PmtPeticion.cAg },
                                {"@cDl", PmtPeticion.cDl },
                                {"@edad", PmtPeticion.edad },
                                {"@edadMeses", PmtPeticion.edadMeses }
                            };
                            ds = DB.EjecutaProcedimientoAlmacenado("sp_select_agregar_menor", Parametros, cadenaConexionLocal);
                            Ls.Exitoso = true;
                        }
                        else
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El menor que intenta ingresar ya se encuentra registrado.";
                        }
                    }

                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }


        public ORespuesta<OMenores> ObtenerMenores()
        {
            ORespuesta<OMenores> Ls = new ORespuesta<OMenores>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_menores", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OMenores menor = new OMenores();
                            menor.idMenorAdopcion = int.Parse(row["idMenorAdopcion"].ToString());
                            menor.nombres = row["nombres"].ToString();
                            menor.antecedentes = row["antecedentes"].ToString();
                            menor.EdadTexto = row["edadTexto"].ToString();
                            menor.CentroAdopcion = row["centroAdopcion"].ToString();
                            menor.Estatus = row["estatus"].ToString();
                            Ls.Respuesta.Add(menor);
                        }
                        Ls.Exitoso = true;
                        return Ls;
                    }
                    else
                    {
                        Ls.Exitoso = true;
                        Ls.Mensaje = "No hay menores en adopción registrados.";
                    }


                }
                Ls.Exitoso = false;
                Ls.Mensaje = "Error en la base de datos";
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OTest> ListarTest(OUsuario PmtPeticion)
        {
            ORespuesta<OTest> Ls = new ORespuesta<OTest>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_tests", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OTest _Dato = new OTest();
                            _Dato.IdTest = int.Parse(row["idTest"].ToString());
                            _Dato.FechaInicio = DateTime.Parse(row["fechaInicio"].ToString());
                            _Dato.FechaFin = DateTime.Parse(row["fechaFin"].ToString());
                            _Dato.EstiloCrianza = row["estiloCrianza"].ToString();
                            _Dato.CalificacionesHabilidades.Add("Altruismo", int.Parse(row["cAl"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Apertura", int.Parse(row["cAp"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Asertividad", int.Parse(row["cAs"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Autoestima", int.Parse(row["cAt"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Resolución de problemas", int.Parse(row["cRp"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Empatía", int.Parse(row["cEm"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Equilibrio emocional", int.Parse(row["cEe"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Independencia", int.Parse(row["cIn"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Flexibilidad", int.Parse(row["cFl"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Reflexividad", int.Parse(row["cRf"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Sociabilidad", int.Parse(row["cSc"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Tolerancia a la frustración", int.Parse(row["cTf"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Establecer vínculos afectivos", int.Parse(row["cAg"].ToString()));
                            _Dato.CalificacionesHabilidades.Add("Resolución de duelo", int.Parse(row["cDl"].ToString()));
                            Ls.Respuesta.Add(_Dato);
                        }
                        Ls.Exitoso = true;
                    }
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OMenores> Match(OUsuario PmtPeticion)
        {
            ORespuesta<OMenores> Ls = new ORespuesta<OMenores>();
            OTest _Dato = new OTest();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_ultimo_test", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _Dato.IdTest = int.Parse(ds.Tables[0].Rows[0]["idTest"].ToString());
                        _Dato.FechaInicio = DateTime.Parse(ds.Tables[0].Rows[0]["fechaInicio"].ToString());
                        _Dato.FechaFin = DateTime.Parse(ds.Tables[0].Rows[0]["fechaFin"].ToString());
                        _Dato.IdEstiloCrianza = int.Parse(ds.Tables[0].Rows[0]["idEstiloCrianza"].ToString());
                        _Dato.CalificacionesHabilidades.Add("cAl", int.Parse(ds.Tables[0].Rows[0]["cAl"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cAp", int.Parse(ds.Tables[0].Rows[0]["cAp"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cAs", int.Parse(ds.Tables[0].Rows[0]["cAs"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cAt", int.Parse(ds.Tables[0].Rows[0]["cAt"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cRp", int.Parse(ds.Tables[0].Rows[0]["cRp"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cEm", int.Parse(ds.Tables[0].Rows[0]["cEm"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cEe", int.Parse(ds.Tables[0].Rows[0]["cEe"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cIn", int.Parse(ds.Tables[0].Rows[0]["cIn"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cFl", int.Parse(ds.Tables[0].Rows[0]["cFl"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cRf", int.Parse(ds.Tables[0].Rows[0]["cRf"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cSc", int.Parse(ds.Tables[0].Rows[0]["cSc"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cTf", int.Parse(ds.Tables[0].Rows[0]["cTf"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cAg", int.Parse(ds.Tables[0].Rows[0]["cAg"].ToString()));
                        _Dato.CalificacionesHabilidades.Add("cDl", int.Parse(ds.Tables[0].Rows[0]["cDl"].ToString()));
                    }
                    else
                    {
                        Ls.Exitoso = false;
                        Ls.Mensaje = "No existen tests aprobados para realizar el match.";
                        return Ls;
                    }
                }
                List<OMenores> listMenores = new List<OMenores>();
                Parametros = new Hashtable();
                ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_menores_disponibles", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OMenores menor = new OMenores();
                            menor.idMenorAdopcion = int.Parse(row["idMenorAdopcion"].ToString());
                            menor.CentroAdopcion = row["centroAdopcion"].ToString();
                            menor.nombres = row["nombres"].ToString();
                            menor.antecedentes = row["antecedentes"].ToString();
                            menor.cAl = int.Parse(row["cAl"].ToString());
                            menor.cAp = int.Parse(row["cAp"].ToString());
                            menor.cAs = int.Parse(row["cAs"].ToString());
                            menor.cAt = int.Parse(row["cAt"].ToString());
                            menor.cRp = int.Parse(row["cRp"].ToString());
                            menor.cEm = int.Parse(row["cEm"].ToString());
                            menor.cEe = int.Parse(row["cEe"].ToString());
                            menor.cIn = int.Parse(row["cIn"].ToString());
                            menor.cFl = int.Parse(row["cFl"].ToString());
                            menor.cRf = int.Parse(row["cRf"].ToString());
                            menor.cTf = int.Parse(row["cTf"].ToString());
                            menor.cSc = int.Parse(row["cSc"].ToString());
                            menor.cAg = int.Parse(row["cAg"].ToString());
                            menor.cDl = int.Parse(row["cDl"].ToString());
                            listMenores.Add(menor);
                        }
                    }
                }
                List<OMenores> results = ORecomendacion.Match(_Dato, listMenores);
                if (results.Count == 0)
                {
                    Ls.Exitoso = false;
                    Ls.Mensaje = "No existen menores compatibles con su perfil.";
                    return Ls;
                }
                foreach(OMenores menor in results)
                {
                    Parametros = new Hashtable()
                    {
                        {"@idTest", _Dato.IdTest },
                        {"@idMenorAdopcion", menor.idMenorAdopcion }
                    };
                    ds = DB.EjecutaProcedimientoAlmacenado("sp_insert_match", Parametros, cadenaConexionLocal);
                }
                Ls.Exitoso = true;
                Ls.Respuesta = results;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<string> IniciarTramiteAdopcion (OTramiteAdopcion PmtPeticion)
        {
            ORespuesta<string> Ls = new ORespuesta<string>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario},
                    {"@idMenorAdopcion", PmtPeticion.IdMenorAdopcion}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_insert_tramite_adopcion", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int res = int.Parse(ds.Tables[0].Rows[0]["result"].ToString());
                        if (res == 0)
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "No se encuentran tests realizados.";
                            return Ls;
                        }
                        else if (res == 1)
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El menor solicitado no es compatible con su perfil. Realice una nueva búsqueda para encontrar menores compatibles con su perfil.";
                            return Ls;
                        }
                        else
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El trámite con este menor ya fué iniciado.";
                            return Ls;
                        }
                    }

                }
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OTramiteAdopcion> ListaTramitesAdopcionUsuario(OUsuario PmtPeticion)
        {
            ORespuesta<OTramiteAdopcion> Ls = new ORespuesta<OTramiteAdopcion>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_tramites_usuario", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                OTramiteAdopcion tr = new OTramiteAdopcion();
                                tr.IdTramite = int.Parse(row["idTramite"].ToString());
                                tr.FechaTramite = DateTime.Parse(row["fechaTramite"].ToString());
                                tr.Estatus = row["estatus"].ToString();
                                tr.CentroAdopcion = row["centroAdopcion"].ToString();
                                tr.Menor = row["nombreMenor"].ToString();
                                Ls.Respuesta.Add(tr);
                            }
                        }
                    }
                }
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OMenores> DetalleMenor(OMenores PmtPeticion)
        {
            ORespuesta<OMenores> Ls = new ORespuesta<OMenores>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idMenorAdopcion", PmtPeticion.idMenorAdopcion}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_datos_menor", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        OMenores _Dato = new OMenores();
                        _Dato.idMenorAdopcion = int.Parse(ds.Tables[0].Rows[0]["idMenorAdopcion"].ToString());
                        _Dato.idEstatus = int.Parse(ds.Tables[0].Rows[0]["idEstatus"].ToString());
                        _Dato.nombres = ds.Tables[0].Rows[0]["nombres"].ToString();
                        _Dato.apellidos = ds.Tables[0].Rows[0]["apellidos"].ToString();
                        _Dato.antecedentes = ds.Tables[0].Rows[0]["antecedentes"].ToString();
                        _Dato.idSexo = int.Parse(ds.Tables[0].Rows[0]["idSexo"].ToString());
                        _Dato.idCentroAdopcion = int.Parse(ds.Tables[0].Rows[0]["idCentroAdopcion"].ToString());
                        _Dato.cAl = int.Parse(ds.Tables[0].Rows[0]["cAl"].ToString());
                        _Dato.cAp = int.Parse(ds.Tables[0].Rows[0]["cAp"].ToString());
                        _Dato.cAs = int.Parse(ds.Tables[0].Rows[0]["cAs"].ToString());
                        _Dato.cAt = int.Parse(ds.Tables[0].Rows[0]["cAt"].ToString());
                        _Dato.cRp = int.Parse(ds.Tables[0].Rows[0]["cRp"].ToString());
                        _Dato.cEm = int.Parse(ds.Tables[0].Rows[0]["cEm"].ToString());
                        _Dato.cEe = int.Parse(ds.Tables[0].Rows[0]["cEe"].ToString());
                        _Dato.cIn = int.Parse(ds.Tables[0].Rows[0]["cIn"].ToString());
                        _Dato.cFl = int.Parse(ds.Tables[0].Rows[0]["cFl"].ToString());
                        _Dato.cRf = int.Parse(ds.Tables[0].Rows[0]["cRf"].ToString());
                        _Dato.cTf = int.Parse(ds.Tables[0].Rows[0]["cTf"].ToString());
                        _Dato.cSc = int.Parse(ds.Tables[0].Rows[0]["cSc"].ToString());
                        _Dato.cAg = int.Parse(ds.Tables[0].Rows[0]["cAg"].ToString());
                        _Dato.cDl = int.Parse(ds.Tables[0].Rows[0]["cDl"].ToString());
                        _Dato.edad = int.Parse(ds.Tables[0].Rows[0]["edad"].ToString());
                        _Dato.edadMeses = int.Parse(ds.Tables[0].Rows[0]["edadMeses"].ToString());
                        Ls.Respuesta.Add(_Dato);
                        Ls.Exitoso = true;
                    }
                }
                else
                {
                    Ls.Exitoso = false;
                    Ls.Mensaje = "El menor que intenta buscar no existe.";
                }
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<string> ModificarMenor(OMenores PmtPeticion)
        {
            ORespuesta<string> Ls = new ORespuesta<string>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idMenorAdopcion", PmtPeticion.idMenorAdopcion},
                    {"@idUsuario", PmtPeticion.IdUsuario },
                    {"@idEstatus", PmtPeticion.idEstatus},
                    {"@nombres", PmtPeticion.nombres},
                    {"@apellidos", PmtPeticion.apellidos},
                    {"@idSexo", PmtPeticion.idSexo},
                    {"@antecedentes", PmtPeticion.antecedentes},
                    {"@idCentroAdopcion", PmtPeticion.idCentroAdopcion},
                    {"@cAl", PmtPeticion.cAl},
                    {"@cAp", PmtPeticion.cAp},
                    {"@cAs", PmtPeticion.cAs},
                    {"@cAt", PmtPeticion.cAt},
                    {"@cRp", PmtPeticion.cRp},
                    {"@cEm", PmtPeticion.cEm},
                    {"@cEe", PmtPeticion.cEe},
                    {"@cIn", PmtPeticion.cIn},
                    {"@cFl", PmtPeticion.cFl},
                    {"@cRf", PmtPeticion.cRf},
                    {"@cSc", PmtPeticion.cSc},
                    {"@cTf", PmtPeticion.cTf},
                    {"@cAg", PmtPeticion.cAg},
                    {"@cDl", PmtPeticion.cDl},
                    {"@edad", PmtPeticion.edad},
                    {"@edadMeses", PmtPeticion.edadMeses}
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_update_menor_adopcion", Parametros, cadenaConexionLocal);
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OTramiteAdopcion> ListaTramitesAdopcion()
        {
            ORespuesta<OTramiteAdopcion> Ls = new ORespuesta<OTramiteAdopcion>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_tramite", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                OTramiteAdopcion tr = new OTramiteAdopcion();
                                tr.IdTramite = int.Parse(row["idTramite"].ToString());
                                tr.FechaTramite = DateTime.Parse(row["fechaTramite"].ToString());
                                tr.Estatus = row["estatus"].ToString();
                                tr.CentroAdopcion = row["centroAdopcion"].ToString();
                                tr.Menor = row["nombreMenor"].ToString();
                                tr.Solicitante = row["nombreSolicitante"].ToString();
                                Ls.Respuesta.Add(tr);
                            }
                        }
                    }
                }
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<string> ModificarEstatusTramite(OTramiteAdopcion PmtPeticion)
        {
            ORespuesta<string> Ls = new ORespuesta<string>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idTramite", PmtPeticion.IdTramite},
                    {"@idEstatusTramite", PmtPeticion.IdEstatus }
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_update_estatus_tramite", Parametros, cadenaConexionLocal);
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OUsuario> ListaEvaluaciones()
        {
            ORespuesta<OUsuario> Ls = new ORespuesta<OUsuario>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_evaluaciones", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                OUsuario user = new OUsuario();
                                user.Nombre = row["nombre"].ToString();
                                user.CURP = row["curp"].ToString();
                                user.CorreoElectronico = row["email"].ToString();
                                user.Telefono = row["telefono"].ToString();
                                user.EstiloCrianza = row["estiloCrianza"].ToString();
                                user.FechaTest = DateTime.Parse(row["fechaTest"].ToString());
                                Ls.Respuesta.Add(user);
                            }
                        }
                    }
                }
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<OUsuario> ListaSolicitantes()
        {
            ORespuesta<OUsuario> Ls = new ORespuesta<OUsuario>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_solicitantes", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                OUsuario user = new OUsuario();
                                user.IdUsuario = int.Parse(row["idUsuario"].ToString());
                                user.Nombre = row["nombre"].ToString();
                                user.CURP = row["curp"].ToString();
                                user.CorreoElectronico = row["email"].ToString();
                                user.Telefono = row["telefono"].ToString();
                                user.Estatus = row["estatus"].ToString();
                                Ls.Respuesta.Add(user);
                            }
                        }
                    }
                }
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }

        public ORespuesta<string> ModificarEstatusSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<string> Ls = new ORespuesta<string>();
            try
            {
                Hashtable Parametros = new Hashtable()
                {
                    {"@idUsuario", PmtPeticion.IdUsuario},
                    {"@idEstatusUsuario", PmtPeticion.IdEstatus }
                };
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_update_estatus_solicitante", Parametros, cadenaConexionLocal);
                Ls.Exitoso = true;
                return Ls;
            }
            catch (SqlException e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
            catch (Exception e)
            {
                Ls.Mensaje = e.Message;
                Ls.Exitoso = false;
                return Ls;
            }
        }
    }
}