using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Data;
using API.Models.Negocio.Catalogo;
using API.Models.Peticion;

namespace API.Controllers
{
    public class CatalogoController : ApiController
    {
        readonly DCatalogo _D;
        public CatalogoController()
        {
            _D = new DCatalogo();
        }
        [HttpPost]
        [ActionName("ListarCatalogoSexo")]
        public IHttpActionResult ListarCatalogoSexo()
        {
            ORespuesta<OCatalogo> res = _D.ListarCatalogoSexo();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoEstadoCivil")]
        public IHttpActionResult ListarCatalogoEstadoCivil()
        {
            ORespuesta<OCatalogo> res = _D.ListarCatalogoEstadoCivil();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoNivelEstudios")]
        public IHttpActionResult ListarCatalogoNivelEstudios()
        {
            ORespuesta<OCatalogo> res = _D.ListarCatalogoNivelEstudios();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoCentrosAdopcion")]
        public IHttpActionResult ListarCatalogoCentrosAdopcion()
        {
            ORespuesta<OCatalogo> res = _D.ListarCatalogoCentrosAdopcion();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoEstatusMenor")]
        public IHttpActionResult ListarCatalogoEstatusMenor()
        {
            ORespuesta<OCatalogo> res = _D.ListarCatalogoEstatusMenor();
            return Json(res);
        }
    }
}
