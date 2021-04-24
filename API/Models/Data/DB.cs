using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace API.Models.Data
{
    public class DB
    {
        public static DataSet EjecutaProcedimientoAlmacenado(string pmtProcedimientoAlmacenado, Hashtable pmtParametros, String pmtCadenaConexion)
        {
            IEnumerator enumerator = null;
            using (SqlConnection cn = new SqlConnection(pmtCadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(pmtProcedimientoAlmacenado, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (pmtParametros != null)
                {
                    try
                    {
                        enumerator = pmtParametros.Keys.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (RuntimeHelpers.GetObjectValue(enumerator.Current) != null)
                            {
                                string Col = RuntimeHelpers.GetObjectValue(enumerator.Current).ToString();
                                cmd.Parameters.Add(new SqlParameter(Col, RuntimeHelpers.GetObjectValue(pmtParametros[Col])));
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable)
                        {
                            (enumerator as IDisposable).Dispose();
                        }
                    }
                }
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    ds.Tables.Add(dt);
                    cn.Dispose();
                }
                catch (Exception e)
                {
                    throw e;
                }
                return ds;
            }
        }
    }
}