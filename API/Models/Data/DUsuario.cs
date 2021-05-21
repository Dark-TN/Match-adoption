using API.Models.Negocio.Catalogo;
using API.Models.Negocio.Seguridad.EncriptadoSimetrico;
using API.Models.Negocio.Test;
using API.Models.Negocio.Usuario;
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
                        if(user.IdEstatus == 2)
                        {
                            Ls.Exitoso = false;
                            Ls.Mensaje = "El usuario está inactivo.";
                        }
                        else
                        {
                            if(PmtPeticion.Password.Equals(OEncriptadoSimetrico.Decrypt<RijndaelManaged>(user.PasswordEncriptada, user.PasswordPrivada)))
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
                            if(res == 2)
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
                        int _Dato = int.Parse(ds.Tables[0].Rows[0]["reuslt"].ToString());
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
                        Ls.Respuesta.Add(test);
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
                        else if(res > 1)
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
    }
}