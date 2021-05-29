using API.Models.Negocio.Catalogo;
using API.Models.Peticion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Models.Data
{
    public class DCatalogo
    {
        private readonly string cadenaConexionLocal;
        public DCatalogo()
        {
            cadenaConexionLocal = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }
        public ORespuesta<OCatalogo> ListarCatalogoSexo()
        {
            ORespuesta<OCatalogo> Ls = new ORespuesta<OCatalogo>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_cat_sexo", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OCatalogo _Dato = new OCatalogo();
                            _Dato.Id = int.Parse(row["idSexo"].ToString());
                            _Dato.Descripcion = row["descripcion"].ToString();
                            Ls.Respuesta.Add(_Dato);
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

        public ORespuesta<OCatalogo> ListarCatalogoEstadoCivil()
        {
            ORespuesta<OCatalogo> Ls = new ORespuesta<OCatalogo>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_cat_estado_civil", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OCatalogo _Dato = new OCatalogo();
                            _Dato.Id = int.Parse(row["idEstadoCivil"].ToString());
                            _Dato.Descripcion = row["descripcion"].ToString();
                            Ls.Respuesta.Add(_Dato);
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

        public ORespuesta<OCatalogo> ListarCatalogoCentrosAdopcion()
        {
            ORespuesta<OCatalogo> Ls = new ORespuesta<OCatalogo>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_cat_centros_laborales", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OCatalogo _Dato = new OCatalogo();
                            _Dato.Id = int.Parse(row["idCentroAdopcion"].ToString());
                            _Dato.Descripcion = row["nombre"].ToString();
                            Ls.Respuesta.Add(_Dato);
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

        public ORespuesta<OCatalogo> ListarCatalogoNivelEstudios()
        {
            ORespuesta<OCatalogo> Ls = new ORespuesta<OCatalogo>();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_cat_nivel_estudios", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OCatalogo _Dato = new OCatalogo();
                            _Dato.Id = int.Parse(row["idNivelEstudios"].ToString());
                            _Dato.Descripcion = row["descripcion"].ToString();
                            Ls.Respuesta.Add(_Dato);
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
    }
}