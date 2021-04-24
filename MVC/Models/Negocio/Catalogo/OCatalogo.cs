using System;


namespace MVC.Models.Negocio.Catalogo
{
    public class OCatalogo
    {
        public Int32 Id { get; set; }
        public String Descripcion { get; set; }
        public OCatalogo()
        {
            this.Id = 0;
            this.Descripcion = "";
        }
    }
}
