using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Hermeco.Multimarcas.Services.Entities;

namespace Hermeco.Multimarcas.Services.Mappings
{
    public class ClienteOfertaMap : ClassMap<ClienteOfertaEntity>
    {
        public ClienteOfertaMap()
        {
            Table("tblClienteOfertas");
            CompositeId()
                .KeyProperty(x => x.IdCliente, "strIdCliente")
                .KeyProperty(x => x.IdOferta, "intIdOferta");
            Map(x => x.NotificaNuevoPortafolio, "btNotificaNuevoPortafolio");
            Map(x => x.FechaNotificacion, "dtFechaNotificacion");
            Map(x => x.NomUser, "strNomUser");
            Map(x => x.FecUser, "dtFecUser");
            Not.LazyLoad();
        }
    }
}
