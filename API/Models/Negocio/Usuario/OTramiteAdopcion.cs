using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Negocio.Usuario
{
    public class OTramiteAdopcion
    {
        public int IdTramite { get; set; }
        public int IdUsuario { get; set; }
        public int IdMenorAdopcion { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaTramite { get; set; }
        public string CentroAdopcion { get; set; }
        public string Menor { get; set; }
        public string Solicitante { get; set; }

        public OTramiteAdopcion()
        {
            this.IdTramite = 0;
            this.IdUsuario = 0;
            this.IdMenorAdopcion = 0;
            this.IdEstatus = 0;
            this.Estatus = string.Empty;
            this.FechaTramite = DateTime.MinValue;
            this.CentroAdopcion = string.Empty;
            this.Menor = string.Empty;
            this.Solicitante = string.Empty;
        }
    }
}