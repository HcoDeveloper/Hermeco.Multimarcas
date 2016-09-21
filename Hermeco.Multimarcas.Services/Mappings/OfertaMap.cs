using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Hermeco.Multimarcas.Services.Entities;

namespace Hermeco.Multimarcas.Services.Mappings
{
    public class OfertaMap : ClassMap<OfertaEntity> 
    {
        public OfertaMap()
        {
            Table("tblOfertas");
            Id(x => x.Id).Column("intIdOferta");
            Map(x => x.Nombre).Column("strNombre");
            Map(x => x.Descripcion).Column("strDescripcion");
            Map(x => x.Tipo).Column("strTipo");
            Map(x => x.FechaPublicacion).Column("dtFechaPublicacion");
            Map(x => x.FechaVencimiento).Column("dtFechaVencimiento");
            Map(x => x.Bodega).Column("strBodega");
            Map(x => x.NomUser).Column("strNomUser");
            Map(x => x.FecUser).Column("dtFecUser");
            Map(x => x.Imagen).Column("imgImagen");
            Map(x => x.Visible).Column("btVisible");

        }
    }
}
