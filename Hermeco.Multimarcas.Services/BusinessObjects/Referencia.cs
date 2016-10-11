using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.BusinessObjects
{
    public class Referencia
    {
        public string IdReferencia { get; set; }
        public int IdOferta { get; set; }
        public float AjustePrecio { get; set; }
        public Boolean Activa { get; set; }
        public string NomUser { get; set; }
        public DateTime FecUser { get; set; }
        public Boolean ValidarInventario { get; set; }
        public string GuiaTallas { get; set; }
        public string CategoriaComercial { get; set; }
        public int Orden { get; set; }
        public List<Plu> Plu = new List<Plu>();
        public List<Imagenes> Imagenes = new List<Imagenes>();
        public List<string> Tallas = new List<string>();
        public List<string> Colores = new List<string>();
    }
}
