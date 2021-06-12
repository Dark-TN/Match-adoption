using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.Negocio.Seguridad
{
    public class OToken
    {
        public static string GenerarToken(int PmtLength)
        {
            var allChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(Enumerable.Repeat(allChar, PmtLength).Select(token => token[random.Next(token.Length)]).ToArray());
            string authToken = resultToken.ToString();
            return authToken;
        }
    }
}