using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Hermeco.Multimarcas.Services.Entities;

namespace Hermeco.Multimarcas.Services.Mappings
{
    public class ReferenciaOfertasMap : ClassMap<ReferenciaOfertasEntity>
    {
        public ReferenciaOfertasMap()
        {
            Table("tblReferenciaOfertas");
            CompositeId()
                .KeyProperty(x => x.IdReferencia, "strIdReferencia")
                .KeyProperty(x => x.IdOferta, "intIdOferta");
            Map(x => x.AjustePrecio, "flAjustePrecio");
            Map(x => x.Activa, "btActiva");
            Map(x => x.NomUser, "strNomUser");
            Map(x => x.FecUser, "dtFecUser");
            Map(x => x.ValidarInventario, "btValidarInventario");
            Map(x => x.GuiaTallas, "strGuiaTallas");
            Map(x => x.CategoriaComercial, "strCategoriaComercial");
            Map(x => x.Orden, "intOrden");
        }
    }
}
