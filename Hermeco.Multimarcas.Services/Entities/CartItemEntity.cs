using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.Entities
{
    public class CartItemEntity
    {
        public virtual int Id { get; set; }
        public virtual string Nit { get; set; }
        public virtual int Oferta { get; set; }
        public virtual string Referencia { get; set; }
        public virtual int Plu { get; set; }
        public virtual string Talla { get; set; }
        public virtual string Color { get; set; }
        public virtual int Cantidad { get; set; }
    }
}
