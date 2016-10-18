using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.BusinessObjects
{
    public class Oferta
    {
        public int  Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Bodega { get; set; }
        public string NomUser { get; set; }
        public DateTime FecUser { get; set; }
        public byte[] Imagen  { get; set; }
        public bool Visible { get; set; }

        public List<Mundo> Mundos = new List<Mundo>();

    }
}
