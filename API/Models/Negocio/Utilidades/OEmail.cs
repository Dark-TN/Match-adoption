using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Diagnostics;

namespace API.Models.Negocio.Utilidades
{
    public class OEmail
    {
        public string CorreoElectronico { get; set; }
        public string Password { get; set; }
        public int Puerto { get; set; }
        public string Host { get; set; }
        public SmtpClient Cliente { get; set; }

        public OEmail(string PmtCorreoElectronico, string PmtPassword, string PmtHost, int PmtPuerto)
        {
            this.CorreoElectronico = PmtCorreoElectronico;
            this.Password = PmtPassword;
            this.Puerto = PmtPuerto;
            this.Host = PmtHost;
            this.Cliente = new SmtpClient();
            this.Cliente.Port = this.Puerto;
            this.Cliente.Host = this.Host;
            this.Cliente.EnableSsl = true;
            this.Cliente.Timeout = 10000;
            this.Cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            this.Cliente.UseDefaultCredentials = false;
            this.Cliente.Credentials = new NetworkCredential(this.CorreoElectronico, this.Password);
        }

        public bool EnviarCorreo(string PmtCorreoElectronico, string PmtAsunto, string PmtMensaje)
        {
            try
            {
                const String cuerpoHtml1 = "...";
                const String cuerpoHtml2 = "El equipo de Match-Adoption";
                String cuerpoCompleto = "";
                MailMessage mm;
                cuerpoCompleto = cuerpoHtml1 + "<br/><br/><p>" + PmtMensaje + "</p><br/><br/>" +cuerpoHtml2;
                mm = new MailMessage(this.CorreoElectronico, PmtCorreoElectronico, PmtAsunto, cuerpoCompleto);
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                this.Cliente.Send(mm);
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}