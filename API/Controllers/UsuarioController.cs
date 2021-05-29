using API.Models.Data;
using API.Models.Negocio.Test;
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
            ORespuesta<OUsuario> res = _D.Login(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RegistroSolicitante")]
        public IHttpActionResult RegistroSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> res = _D.RegistroUsuario(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ListarPreguntas")]
        public IHttpActionResult ListarPreguntas(OUsuario PmtPeticion)
        {
            ORespuesta<OTest> res = _D.ListarPreguntas(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RegistroEmpleado")]
        public IHttpActionResult RegistroEmpleado(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> res = _D.RegistroEmpleado(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("GuardarTest")]
        public IHttpActionResult GuardarTest(OTest PmtPeticion)
        {
            ORespuesta<string> res = _D.GuardarTest(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ListarTest")]
        public IHttpActionResult ListarTest(OUsuario PmtPeticion)
        {
            ORespuesta<OTest> res = _D.ListarTest(PmtPeticion);
            return Json(res);
        }
    }
}