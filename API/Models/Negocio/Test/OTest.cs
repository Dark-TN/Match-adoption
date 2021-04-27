﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Negocio.Test
{
    public class OTest
    {
        public int IdTest { get; set; }
        public int IdUsuario { get; set; }
        public List<OPregunta> Preguntas { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaLimite { get; set; }
        public double TiempoRestante { get; set; }
        public double TiempoDisponible { get; set; }
        public List<int> CalificacionesHabilidades { get; set; }
        public int IdEstiloCrianza { get; set; }

        public OTest()
        {
            this.IdTest = 0;
            this.IdUsuario = 0;
            this.Preguntas = new List<OPregunta>();
            this.FechaInicio = DateTime.MinValue;
            this.FechaFin = DateTime.MinValue;
            this.FechaLimite = DateTime.MinValue;
            this.TiempoRestante = 0.0;
            this.TiempoDisponible = 0.0;
            this.CalificacionesHabilidades = new List<int>();
            this.IdEstiloCrianza = 0;
        }
    }
}