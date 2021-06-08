using API.Models.Negocio.Menores;
using API.Models.Negocio.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace API.Models.Negocio.Recomendacion
{
    public class ORecomendacion
    {

        public static List<OMenores> Match(OTest Test, List<OMenores> Menores)
        {
            float maxLen = (float)Math.Sqrt(126);
            List<OMenores> results = new List<OMenores>();
            List<int> userScores = new List<int>()
            {
                Test.CalificacionesHabilidades["cAl"],
                Test.CalificacionesHabilidades["cAp"],
                Test.CalificacionesHabilidades["cAs"],
                Test.CalificacionesHabilidades["cAt"],
                Test.CalificacionesHabilidades["cRp"],
                Test.CalificacionesHabilidades["cEm"],
                Test.CalificacionesHabilidades["cEe"],
                Test.CalificacionesHabilidades["cIn"],
                Test.CalificacionesHabilidades["cFl"],
                Test.CalificacionesHabilidades["cRf"],
                Test.CalificacionesHabilidades["cSc"],
                Test.CalificacionesHabilidades["cTf"],
                Test.CalificacionesHabilidades["cAg"],
                Test.CalificacionesHabilidades["cDl"]
            };
            foreach(OMenores menor in Menores)
            {
                List<int> ls = new List<int>()
                {
                    menor.cAl,
                    menor.cAp,
                    menor.cAs,
                    menor.cAt,
                    menor.cRp,
                    menor.cEm,
                    menor.cEe,
                    menor.cIn,
                    menor.cFl,
                    menor.cRf,
                    menor.cSc,
                    menor.cTf,
                    menor.cAg,
                    menor.cDl
                };
                float pows = 0;
                for (int i = 0; i < userScores.Count; i++)
                {
                    pows += (float)Math.Pow(userScores[i] - ls[i], 2);
                }
                menor.Porcentaje = (float)Math.Round(100 - (float)Math.Sqrt(pows) * 100 / maxLen, 2);
                results.Add(menor);
            }
            return results;
        }
    }
}