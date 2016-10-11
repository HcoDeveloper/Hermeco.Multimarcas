using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermeco.Multimarcas.Services.BusinessObjects
{
    public class Imagenes
    {
        public int IdImagen { get; set; }
        public string IdReferencia { get; set; }
        public byte[] Imagen { get; set; }
        public int Orden { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreada { get; set; }
        public string CreadaPor { get; set; }
        public int Clasificacion { get; set; }
        public string DescripcionColor { get; set; }
        public int Color { get; set; }
    }
}
