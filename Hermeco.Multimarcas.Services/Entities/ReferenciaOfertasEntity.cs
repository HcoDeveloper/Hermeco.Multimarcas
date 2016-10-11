using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.Entities
{
    public class ReferenciaOfertasEntity
    {
        public virtual string IdReferencia { get; set; }
        public virtual int IdOferta { get; set; }
        public virtual float AjustePrecio { get; set; }
        public virtual Boolean Activa { get; set; }
        public virtual string NomUser { get; set; }
        public virtual DateTime FecUser { get; set; }
        public virtual Boolean ValidarInventario { get; set; }
        public virtual string GuiaTallas { get; set; }
        public virtual string CategoriaComercial { get; set; }
        public virtual int Orden { get; set; }
        public virtual OfertaEntity  Oferta { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var o = obj as ReferenciaOfertasEntity;
            if (o == null)
                return false;
            if (IdReferencia == o.IdReferencia && IdOferta == o.IdOferta)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (IdReferencia.ToString() + "|" + IdOferta.ToString()).GetHashCode();
        }

    }
}
