using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Negocio.Test
{
    public class OPregunta
    {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
        public int IdHabilidad {get;set;}
        public List<bool> Respuesta { get; set; }

        public OPregunta()
        {
            this.IdPregunta = 0;
            this.Pregunta = string.Empty;
            this.IdHabilidad = 0;
            this.Respuesta = new List<bool>();
        }
    }
}