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

namespace API.Models.Data
{
    public class DUsuario
    {
        private readonly string cadenaConexionLocal;
        public DUsuario()
        {
            cadenaConexionLocal = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        public ORespuesta Login(OUsuario PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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
                            Ls.Mensaje = "El usuario está inactivo.";
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

        public ORespuesta RegistroUsuario(OUsuario PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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

        public ORespuesta ListarPreguntas(OUsuario PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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

        public ORespuesta GuardarTest(OTest PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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
                    Hashtable Parametros = new Hashtable()
                    {
                        {"@idUsuario", PmtPeticion.IdUsuario},
                        {"@fechaInicio", PmtPeticion.FechaInicio },
                        {"@fechaFin", PmtPeticion.FechaFin }
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
                                        {"@idTest", PmtPeticion.IdUsuario},
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

        public ORespuesta ModificarSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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
                if (!PmtPeticion.NuevaPassword.Equals(PmtPeticion.ConfirmarPassword))
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


        public ORespuesta RegistroEmpleado(OUsuario PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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


        public ORespuesta RegistrarMenor(OMenores PmtPeticion)
        {
            ORespuesta Ls = new ORespuesta();
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
                            OMenores menor = new OMenores();
                            Parametros = new Hashtable()
                            {
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
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                res = int.Parse(ds.Tables[0].Rows[0]["idMenorAdopcion"].ToString());
                                menor.idMenorAdopcion = res;
                            }
                            menor.idEstatus = PmtPeticion.idEstatus;
                            menor.nombres = PmtPeticion.nombres;
                            menor.apellidos = PmtPeticion.apellidos;
                            menor.idSexo = PmtPeticion.idSexo;
                            menor.antecedentes = PmtPeticion.antecedentes;
                            menor.idCentroAdopcion = PmtPeticion.idCentroAdopcion;
                            menor.cAl = PmtPeticion.cAl;
                            menor.cAp = PmtPeticion.cAp;
                            menor.cAs = PmtPeticion.cAs;
                            menor.cRp = PmtPeticion.cRp;
                            menor.cEm = PmtPeticion.cEm;
                            menor.cEe = PmtPeticion.cEe;
                            menor.cIn = PmtPeticion.cIn;
                            menor.cFl = PmtPeticion.cFl;
                            menor.cRf = PmtPeticion.cRf;
                            menor.cSc = PmtPeticion.cSc;
                            menor.cTf = PmtPeticion.cTf;
                            menor.cAg = PmtPeticion.cAg;
                            menor.cDl = PmtPeticion.cDl;
                            menor.edad = PmtPeticion.edad;
                            menor.edadMeses = PmtPeticion.edadMeses;
                            Ls.Exitoso = true;
                            Ls.Respuesta.Add(menor);

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


        public ORespuesta ObtenerMenores()
        {
            ORespuesta Ls = new ORespuesta();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_men_adopcion", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    { ArrayList listMenores = new ArrayList();
                       for(int i=0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            OMenores menor = new OMenores();
                            menor.idMenorAdopcion= int.Parse(ds.Tables[0].Rows[i]["idMenorAdopcion"].ToString());
                            menor.idEstatus = int.Parse(ds.Tables[0].Rows[i]["idEstatus"].ToString());
                            menor.idSexo = int.Parse(ds.Tables[0].Rows[i]["idSexo"].ToString());
                            menor.idCentroAdopcion = int.Parse(ds.Tables[0].Rows[i]["idCentroAdopcion"].ToString());
                            menor.nombres = ds.Tables[0].Rows[i]["nombres"].ToString();
                            menor.apellidos = ds.Tables[0].Rows[i]["apellidos"].ToString();
                            menor.antecedentes = ds.Tables[0].Rows[i]["antecedentes"].ToString();
                            menor.cAl = int.Parse(ds.Tables[0].Rows[i]["cAl"].ToString());
                            menor.cAp = int.Parse(ds.Tables[0].Rows[i]["cAp"].ToString());
                            menor.cAs = int.Parse(ds.Tables[0].Rows[i]["cAs"].ToString());
                            menor.cAt = int.Parse(ds.Tables[0].Rows[i]["cAt"].ToString());
                            menor.cRp = int.Parse(ds.Tables[0].Rows[i]["cRp"].ToString());
                            menor.cEm = int.Parse(ds.Tables[0].Rows[i]["cEm"].ToString());
                            menor.cEe = int.Parse(ds.Tables[0].Rows[i]["cEe"].ToString());
                            menor.cIn = int.Parse(ds.Tables[0].Rows[i]["cIn"].ToString());
                            menor.cFl = int.Parse(ds.Tables[0].Rows[i]["cFl"].ToString());
                            menor.cRf = int.Parse(ds.Tables[0].Rows[i]["cRf"].ToString());
                            menor.cTf = int.Parse(ds.Tables[0].Rows[i]["cTf"].ToString());
                            menor.cSc = int.Parse(ds.Tables[0].Rows[i]["cSc"].ToString());
                            menor.cAg = int.Parse(ds.Tables[0].Rows[i]["cAg"].ToString());
                            menor.cDl = int.Parse(ds.Tables[0].Rows[i]["cDl"].ToString());
                            menor.edad = int.Parse(ds.Tables[0].Rows[i]["edad"].ToString());
                            menor.edadMeses = int.Parse(ds.Tables[0].Rows[i]["edadMeses"].ToString());
                            listMenores.Add(menor);
                        }
                        Ls.Exitoso = true;
                        Ls.Respuesta.Add(listMenores);
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





    }

}