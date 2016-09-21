using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.Entities
{
    public class ClienteOfertaEntity
    {
        public virtual string IdCliente { get; set; }
        public virtual int IdOferta { get; set; }
        public virtual Boolean NotificaNuevoPortafolio { get; set; }
        public virtual DateTime FechaNotificacion { get; set; }
        public virtual string NomUser { get; set; }
        public virtual DateTime FecUser { get; set; }
        public virtual IList<OfertaEntity> Ofertas { get; set; }
        public virtual void AddOferta(OfertaEntity nuevaOferta)
        {
            nuevaOferta.clienteOfertaEntity = this;
            Ofertas.Add(nuevaOferta);
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var o = obj as ClienteOfertaEntity;
            if (o == null)
                return false;
            if (IdCliente == o.IdCliente && IdOferta == o.IdOferta && NotificaNuevoPortafolio == o.NotificaNuevoPortafolio)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (IdCliente.ToString() + "|" + IdOferta.ToString()).GetHashCode();
        }
    }
}
