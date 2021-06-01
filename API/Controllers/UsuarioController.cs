using API.Models.Data;
using API.Models.Negocio.Test;
using API.Models.Negocio.Usuario;
using API.Models.Negocio.Menores;
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

        [HttpPost]
        [ActionName("GuardarTest")]
        public IHttpActionResult GuardarTest(OTest PmtPeticion)
        {
            ORespuesta res = _D.GuardarTest(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RegistrarMenor")]
        public IHttpActionResult RegistrarMenor(OMenores PmtPeticion)
        {
            ORespuesta res = _D.RegistrarMenor(PmtPeticion);
            return Json(res);
        }
        
        
        [HttpGet]
        [ActionName("ObtenerMenores")]
        public IHttpActionResult ObtenerMenores()
        {
            ORespuesta res = _D.ObtenerMenores();
            return Json(res);
        }


    }
}