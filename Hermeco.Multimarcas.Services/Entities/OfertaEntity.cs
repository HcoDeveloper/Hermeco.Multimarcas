using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.Entities
{
    public class OfertaEntity
    {
        public virtual int Id { get; protected set; }
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Tipo { get; set; }
        public virtual DateTime FechaPublicacion { get; set; }
        public virtual DateTime FechaVencimiento { get; set; }
        public virtual string Bodega { get; set; }
        public virtual string NomUser { get; set; }
        public virtual DateTime FecUser { get; set; }
        public virtual byte[] Imagen { get; set; }
        public virtual Boolean Visible { get; set; }
        public virtual ClienteOfertaEntity clienteOfertaEntity { get; set; }

        public OfertaEntity() 
        { }
    }
}
