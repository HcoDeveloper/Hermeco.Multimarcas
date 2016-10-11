using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services.BusinessObjects
{
    public class Cliente
    {
        public string Codigo { get; set; }
        public string Nit { get; set; }
        public string CanalDistribucion { get; set; }
        public string ListaPrecios { get; set; }
        public string TipoMercado { get; set; }
        public string FormaPago { get; set; }
        public string NombreCanal { get; set; }
        public string EstadoCliente { get; set; }
        public string Moneda { get; set; }
        public string NombreCliente { get; set; }
        public string NombreClienteNit { get; set; }
        public string Cartera { get; set; }
        public string CupoCredito { get; set; }
        public string CupoDisponible { get; set; }
    }
}
