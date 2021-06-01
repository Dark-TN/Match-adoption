using API.Models.Negocio.Catalogo;
using API.Models.Negocio.Menores;
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
        public ORespuesta ListarCatalogoSexo()
        {
            ORespuesta Ls = new ORespuesta();
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

        public ORespuesta ListarCatalogoEstadoCivil()
        {
            ORespuesta Ls = new ORespuesta();
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

        public ORespuesta ListarCatalogoCentrosAdopcion()
        {
            ORespuesta Ls = new ORespuesta();
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

        public ORespuesta ListarCatalogoNivelEstudios()
        {
            ORespuesta Ls = new ORespuesta();
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

        public ORespuesta ListarMenoresAdopcion()
        {
            ORespuesta Ls = new ORespuesta();
            try
            {
                Hashtable Parametros = new Hashtable();
                DataSet ds = DB.EjecutaProcedimientoAlmacenado("sp_select_lista_men_adopcion", Parametros, cadenaConexionLocal);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            OMenores _Dato = new OMenores();
                            _Dato.idMenorAdopcion = int.Parse(row["idMenorAdopcion"].ToString());
                            _Dato.idEstatus = int.Parse(row["idEstatus"].ToString());
                            _Dato.idSexo = int.Parse(row["idSexo"].ToString());
                            _Dato.idCentroAdopcion = int.Parse(row["idCentroAdopcion"].ToString());
                            _Dato.edad = int.Parse(row["edad"].ToString());
                            _Dato.cAl = int.Parse(row["cAl"].ToString());
                            _Dato.cAp = int.Parse(row["cAp"].ToString());
                            _Dato.cAs = int.Parse(row["cAs"].ToString());
                            _Dato.cAt = int.Parse(row["cAt"].ToString());
                            _Dato.cRp = int.Parse(row["cRp"].ToString());
                            _Dato.cEm = int.Parse(row["cEm"].ToString());
                            _Dato.cEe = int.Parse(row["cEe"].ToString());                           
                            _Dato.cIn = int.Parse(row["cIn"].ToString());
                            _Dato.cFl = int.Parse(row["cFl"].ToString());
                            _Dato.cRf = int.Parse(row["cRf"].ToString());
                            _Dato.cSc = int.Parse(row["cSc"].ToString());
                            _Dato.cTf = int.Parse(row["cTf"].ToString());
                            _Dato.cAg = int.Parse(row["cAg"].ToString());
                            _Dato.cDl = int.Parse(row["cDl"].ToString());
                            _Dato.nombres = row["nombres"].ToString();
                            _Dato.apellidos = row["apellidos"].ToString();
                            _Dato.antecedentes = row["antecedentes"].ToString();
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