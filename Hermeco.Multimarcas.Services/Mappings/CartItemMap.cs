using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Hermeco.Multimarcas.Services.Entities;

namespace Hermeco.Multimarcas.Services.Mappings
{
    class CartItemMap : ClassMap<CartItemEntity>
    {
        public CartItemMap()
        {
            Table("tblCartItems");
            Id(x => x.Id).Column("Id").GeneratedBy.Identity();
            Map(x => x.Nit).Column("strNit");
            Map(x => x.Plu).Column("strPlu");
            Map(x => x.Talla).Column("strTalla");
            Map(x => x.Color).Column("strColor");
            Map(x => x.Cantidad).Column("intCantidad");
            Map(x => x.Oferta).Column("intOferta");
            Map(x => x.Referencia).Column("strReferencia");

        }
    }
}
