using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Dictionary<string, int> CalificacionesHabilidades { get; set; }
        public int IdEstiloCrianza { get; set; }
        public string EstiloCrianza { get; set; }

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
            this.CalificacionesHabilidades = new Dictionary<string, int>();
            this.IdEstiloCrianza = 0;
            this.EstiloCrianza = string.Empty;
        }

        public void CalificarTest()
        {
            int dirValidez = this.Preguntas.Where(a => a.IdHabilidad == 10).Select(a => a.Respuesta).Sum();
            if (dirValidez <= 4)
            {
                for (int i = 1; i <= 17; i++)
                {
                    if (i == 4)
                    {
                        this.CalificacionesHabilidades.Add(i.ToString(), dirValidez);
                    }
                    else
                    {
                        this.CalificacionesHabilidades.Add(i.ToString(), 0);
                    }
                }
                this.IdEstiloCrianza = 5;
                return;
            }
            this.CalificacionesHabilidades.Add("10", dirValidez);
            int promDeseabilidadSocial = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 5).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("5", promDeseabilidadSocial);
            int dirCritica = this.Preguntas.Where(a => a.IdHabilidad == 17).Select(a => a.Respuesta).Sum();
            this.CalificacionesHabilidades.Add("17", dirCritica);
            int promApertura = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 1).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("1", promApertura);
            int promEquilibroEmocional = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 2).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("2", promEquilibroEmocional);
            int promAutoestima = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 3).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("3", promAutoestima);
            int promIndependencia = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 4).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("4", promIndependencia);
            int promSociabilidad = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 6).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("6", promSociabilidad);
            int promReflexividad = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 7).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("7", promReflexividad);
            int promEmpatia = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 8).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("8", promEmpatia);
            int promFlexibilidad = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 9).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("9", promFlexibilidad);
            int promToleranciaFrustracion = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 11).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("11", promToleranciaFrustracion);
            int promResolucionDuelo = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 12).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("12", promResolucionDuelo);
            int promResolverProblemas = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 13).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("13", promResolverProblemas);
            int promAltruismo = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 14).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("14", promAltruismo);
            int promAsertividad = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 15).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("15", promAsertividad);
            int promVinculosApego = (int)Math.Round(this.Preguntas.Where(a => a.IdHabilidad == 16).Select(a => a.Respuesta).Average());
            this.CalificacionesHabilidades.Add("16", promVinculosApego);

            if(promAutoestima > 3 && promAsertividad > 3 && promResolverProblemas > 3 && promEquilibroEmocional > 3 && promEmpatia > 3 && promFlexibilidad > 3
                && promReflexividad > 3 && promToleranciaFrustracion > 3 && promVinculosApego > 3)
            {
                this.IdEstiloCrianza = 1;
            }
            else if (promApertura < 2 && promAutoestima < 2 && promResolverProblemas < 2 && promEmpatia < 2 && promEquilibroEmocional < 2 && promFlexibilidad < 2 
                && promToleranciaFrustracion < 2)
            {
                this.IdEstiloCrianza = 2;
            }
            else if (promAutoestima < 2 && promAsertividad < 2 && promResolverProblemas < 2 && promIndependencia < 2 && promFlexibilidad > 3 && promVinculosApego > 2)
            {
                this.IdEstiloCrianza = 3;
            }
            else
            {
                this.IdEstiloCrianza = 4;
            }
        }
    }
}