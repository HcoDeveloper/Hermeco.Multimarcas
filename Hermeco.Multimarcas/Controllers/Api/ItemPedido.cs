using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermeco.Multimarcas.Controllers.Api
{
    public class ItemPedido
    {
        public int Plu { get; set; }
        public string Referencia { get; set; }
        public int Oferta { get; set; }
        public string Nit { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public int Cantidad { get; set; }
    }
}
