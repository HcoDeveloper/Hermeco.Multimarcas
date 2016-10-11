using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using Hermeco.Multimarcas.Services.Entities;

namespace Hermeco.Multimarcas.Services.Mappings
{
    public class ImagenesReferenciaMap : ClassMap<ImagenesReferenciaEntity>
    {
        public ImagenesReferenciaMap()
        {
            Table("tblImagenesReferencia");
            CompositeId()
                .KeyProperty(x => x.IdImagen, "intIdImagen")
                .KeyProperty(x => x.IdReferencia, "strIdReferencia");
            Map(x => x.Imagen, "imgImagen");
            Map(x => x.Orden, "intOrden");
            Map(x => x.Descripcion, "strDescripcion");
            Map(x => x.FechaCreada, "dtFechaCreada");
            Map(x => x.CreadaPor, "strCreadaPor");
            Map(x => x.Clasificacion, "intClasificacion");
            Map(x => x.DescripcionColor, "strDescripcionColor");
            Map(x => x.Color, "intColor");
        }
    }
}
