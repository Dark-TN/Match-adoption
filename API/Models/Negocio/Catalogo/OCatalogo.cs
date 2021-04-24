using System;


namespace API.Models.Negocio.Catalogo
{
    public class OCatalogo
    {
        public int Id { get; set; }
        public String Descripcion { get; set; }
        public OCatalogo()
        {
            this.Id = 0;
            this.Descripcion = "";
        }
    }
}
