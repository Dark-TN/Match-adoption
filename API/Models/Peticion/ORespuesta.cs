using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.Peticion
{
    public class ORespuesta
    {
        public List<object> Respuesta { get; set;  }
        public String Mensaje { get; set; }
        public int Error { get; set; }
        public bool Exitoso { get; set; }
        public ORespuesta()
        {
            this.Respuesta = new List<object>();
            this.Mensaje = String.Empty;
            this.Error = 0;
            this.Exitoso = false;
        }
    }
}
