using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models.Data;
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
            ORespuesta res = _D.ListarCatalogoSexo();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoEstadoCivil")]
        public IHttpActionResult ListarCatalogoEstadoCivil()
        {
            ORespuesta res = _D.ListarCatalogoEstadoCivil();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoNivelEstudios")]
        public IHttpActionResult ListarCatalogoNivelEstudios()
        {
            ORespuesta res = _D.ListarCatalogoNivelEstudios();
            return Json(res);
        }
        [HttpPost]
        [ActionName("ListarCatalogoCentrosAdopcion")]
        public IHttpActionResult ListarCatalogoCentrosAdopcion()
        {
            ORespuesta res = _D.ListarCatalogoCentrosAdopcion();
            return Json(res);
        }
    }
}
