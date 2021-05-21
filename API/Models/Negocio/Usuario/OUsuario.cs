using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace API.Models.Negocio.Usuario
{
    public class OUsuario
    {
        public int IdUsuario { get; set; }
        public int TipoUsuario { get; set; }
        public string CURP { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdSexo { get; set; }
        public int IdEstadoCivil { get; set; }
        public int IdNivelEstudios { get; set; }
        public string Ocupacion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public int IdEstatus { get; set; }
        public string Password { get; set; }
        public string PasswordEncriptada { get; set; }
        public string PasswordPrivada { get; set; }
        public string NuevaPassword { get; set; }
        public string ConfirmarPassword { get; set; }

        public OUsuario()
        {
            this.IdUsuario = 0;
            this.TipoUsuario = 0;
            this.CURP = string.Empty;
            this.Nombre = string.Empty;
            this.FechaNacimiento = DateTime.MinValue;
            this.IdSexo = 0;
            this.IdEstadoCivil = 0;
            this.IdNivelEstudios = 0;
            this.Ocupacion = string.Empty;
            this.Direccion = string.Empty;
            this.Telefono = string.Empty;
            this.CorreoElectronico = string.Empty;
            this.IdEstatus = 1;
            this.Password = string.Empty;
            this.PasswordEncriptada = string.Empty;
            this.PasswordPrivada = string.Empty;
            this.NuevaPassword = string.Empty;
            this.ConfirmarPassword = string.Empty;
        }

        public void GenerarPasswordPrivada()
        {
            int length = 18;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_-";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];
                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            this.PasswordPrivada = res.ToString();
        }
    }
}