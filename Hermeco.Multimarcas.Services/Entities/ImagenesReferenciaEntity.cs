using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermeco.Multimarcas.Services.Entities
{
    public class ImagenesReferenciaEntity
    {
        public virtual int IdImagen { get; set; }
        public virtual string  IdReferencia { get; set; }
        public virtual byte[] Imagen { get; set; }
        public virtual int  Orden { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual DateTime FechaCreada { get; set; }
        public virtual string CreadaPor { get; set; }
        public virtual int Clasificacion { get; set; }
        public virtual string DescripcionColor { get; set; }
        public virtual int Color { get; set; }
        public virtual ReferenciaOfertasEntity ReferenciaOfertasEntity { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var o = obj as ImagenesReferenciaEntity;
            if (o == null)
                return false;
            if (IdImagen == o.IdImagen && IdReferencia == o.IdReferencia)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (IdImagen.ToString() + "|" + IdReferencia.ToString()).GetHashCode();
        }
    }
}
