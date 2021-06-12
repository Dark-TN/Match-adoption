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
using System.Collections;

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
        [ActionName("ModificarSolicitante")]
        public IHttpActionResult ModificarSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> res = _D.ModificarSolicitante(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ModificarEmpleado")]
        public IHttpActionResult ModificarEmpleado(OUsuario PmtPeticion)
        {
            ORespuesta<OUsuario> res = _D.ModificarEmpleado(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ListarTest")]
        public IHttpActionResult ListarTest(OUsuario PmtPeticion)
        {
            ORespuesta<OTest> res = _D.ListarTest(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RegistrarMenor")]
        public IHttpActionResult RegistrarMenor(OMenores PmtPeticion)
        {
            ORespuesta<string> res = _D.RegistrarMenor(PmtPeticion);
            return Json(res);
        }
        
        
        [HttpGet]
        [ActionName("ObtenerMenores")]
        public IHttpActionResult ObtenerMenores()
        {
            ORespuesta<OMenores> res = _D.ObtenerMenores();
            return Json(res);
        }

        [HttpPost]
        [ActionName("Match")]
        public IHttpActionResult Match(OUsuario PmtPeticion)
        {
            ORespuesta<OMenores> res = _D.Match(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("IniciarTramiteAdopcion")]
        public IHttpActionResult IniciarTramiteAdopcion(OTramiteAdopcion PmtPeticion)
        {
            ORespuesta<string> res = _D.IniciarTramiteAdopcion(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ListaTramitesAdopcionUsuario")]
        public IHttpActionResult ListaTramitesAdopcionUsuario(OUsuario PmtPeticion)
        {
            ORespuesta<OTramiteAdopcion> res = _D.ListaTramitesAdopcionUsuario(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("DetalleMenor")]
        public IHttpActionResult DetalleMenor(OMenores PmtPeticion)
        {
            ORespuesta<OMenores> res = _D.DetalleMenor(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("ModificarMenor")]
        public IHttpActionResult ModificarMenor(OMenores PmtPeticion)
        {
            ORespuesta<string> res = _D.ModificarMenor(PmtPeticion);
            return Json(res);
        }

        [HttpGet]
        [ActionName("ListaTramitesAdopcion")]
        public IHttpActionResult ListaTramitesAdopcion()
        {
            ORespuesta<OTramiteAdopcion> res = _D.ListaTramitesAdopcion();
            return Json(res);
        }

        [HttpPost]
        [ActionName("ModificarEstatusTramite")]
        public IHttpActionResult ModificarEstatusTramite(OTramiteAdopcion PmtPeticion)
        {
            ORespuesta<string> res = _D.ModificarEstatusTramite(PmtPeticion);
            return Json(res);
        }

        [HttpGet]
        [ActionName("ListaEvaluaciones")]
        public IHttpActionResult ListaEvaluaciones()
        {
            ORespuesta<OUsuario> res = _D.ListaEvaluaciones();
            return Json(res);
        }

        [HttpGet]
        [ActionName("ListaSolicitantes")]
        public IHttpActionResult ListaSolicitantes()
        {
            ORespuesta<OUsuario> res = _D.ListaSolicitantes();
            return Json(res);
        }

        [HttpPost]
        [ActionName("ModificarEstatusSolicitante")]
        public IHttpActionResult ModificarEstatusSolicitante(OUsuario PmtPeticion)
        {
            ORespuesta<string> res = _D.ModificarEstatusSolicitante(PmtPeticion);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RecuperarPassword")]
        public IHttpActionResult RecuperarPassword(OUsuario PmtPeticion)
        {
            ORespuesta<string> res = _D.RecuperarPassword(PmtPeticion);
            return Json(res);
        }

        [HttpGet]
        [ActionName("CambiarPassword")]
        public IHttpActionResult CambiarPassword(string token)
        {
            ORespuesta<OUsuario> res = _D.CambiarPassword(token);
            return Json(res);
        }

        [HttpPost]
        [ActionName("RestablecerPassword")]
        public IHttpActionResult RestablecerPassword(OUsuario PmtPeticion)
        {
            ORespuesta<string> res = _D.RestablecerPassword(PmtPeticion);
            return Json(res);
        }
    }
}