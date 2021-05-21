using API.Models.Data;
using API.Models.Negocio.Usuario;
using API.Models.Peticion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        readonly DUsuario _D;
        public UsuarioController()
        {
            _D = new DUsuario();
        }

        [HttpPost]
        [ActionName("Login")]
        public IHttpActionResult Login(OUsuario PmtPeticion)
        {
            ORespuesta res = _D.Login(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RegistroSolicitante")]
        public IHttpActionResult RegistroSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta res = _D.RegistroUsuario(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ListarPreguntas")]
        public IHttpActionResult ListarPreguntas(OUsuario PmtPeticion)
        {
            ORespuesta res = _D.ListarPreguntas(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RegistroEmpleado")]
        public IHttpActionResult RegistroEmpleado(OUsuario PmtPeticion)
        {
            ORespuesta res = _D.RegistroEmpleado(PmtPeticion);
            return Json(res);
        }
    }
}