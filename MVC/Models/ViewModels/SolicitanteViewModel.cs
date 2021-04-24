using MVC.Models.Negocio.Test;
using MVC.Models.Negocio.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.ViewModels
{
    public class SolicitanteViewModel
    {
        public OUsuario Usuario { get; set; }
        public OTest Test { get; set; }

        public SolicitanteViewModel()
        {
            this.Usuario = new OUsuario();
            this.Test = new OTest();
        }
    }
}