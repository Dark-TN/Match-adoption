using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC.Models.Negocio.Menores
{
    public class OMenores
    {
        public int IdUsuario { get; set; }
        public int idMenorAdopcion { get; set; }
        public int idEstatus { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public int idSexo { get; set; }
        public string Estatus { get; set; }
        public int idCentroAdopcion { get; set; }
        public string CentroAdopcion { get; set; }
        public string antecedentes { get; set; }
        public int cAl { get; set; }
        public int cAp { get; set; }
        public int cAs { get; set; }
        public int cAt { get; set; }
        public int cRp { get; set; }
        public int cEm { get; set; }
        public int cEe { get; set; }
        public int cIn { get; set; }
        public int cFl { get; set; }
        public int cRf { get; set; }
        public int cTf { get; set; }
        public int cSc { get; set; }
        public int cAg { get; set; }
        public int cDl { get; set; }
        public int edad { get; set; }
        public int edadMeses { get; set; }
        public string EdadTexto { get; set; }
        public float Porcentaje { get; set; }

        public OMenores()
        {
            this.IdUsuario = 0;
            this.idMenorAdopcion = 0;
            this.idEstatus = 0;
            this.nombres = string.Empty;
            this.apellidos = string.Empty;
            this.antecedentes = string.Empty;
            this.edad = 0;
            this.edadMeses = 0;
            this.idSexo = 0;
            this.Estatus = string.Empty;
            this.idCentroAdopcion = 0;
            this.CentroAdopcion = string.Empty;
            this.cAl = 0;
            this.cAp = 0;
            this.cAs = 0;
            this.cAt = 0;
            this.cRp = 0;
            this.cEm = 0;
            this.cEe = 0;
            this.cIn = 0;
            this.cFl = 0;
            this.cRf = 0;
            this.cSc = 0;
            this.cTf = 0;
            this.cAg = 0;
            this.cDl = 0;
            this.EdadTexto = string.Empty;
            this.Porcentaje = 0;
        }
    }
}