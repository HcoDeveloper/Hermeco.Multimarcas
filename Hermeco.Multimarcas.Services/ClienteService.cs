using Hermeco.Multimarcas.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Services
{
    public class ClienteService
    {
        public Cliente getInfoClient(string Nit)
        {
            Cliente cliente = new Cliente(); ;
            CustomerService cs = new CustomerService();
            DataSet infoCliente = cs.ObtenerClienteCustomerService(Nit);
            
            if (infoCliente != null)
            {
                cliente.CanalDistribucion = infoCliente.Tables[0].Rows[0]["CANALDISTRIBUCION"].ToString();
                cliente.Codigo = infoCliente.Tables[0].Rows[0]["CODCLIENTEINTER"].ToString();
                cliente.EstadoCliente = infoCliente.Tables[0].Rows[0]["ESTADOCLIENTE"].ToString();
                cliente.FormaPago = infoCliente.Tables[0].Rows[0]["FORMAPAGO"].ToString();
                cliente.ListaPrecios = infoCliente.Tables[0].Rows[0]["LISTAPRECIOS"].ToString();
                cliente.Moneda = infoCliente.Tables[0].Rows[0]["MONEDA"].ToString();
                cliente.Nit = infoCliente.Tables[0].Rows[0]["NITCLIENTE"].ToString();
                cliente.NombreCanal = infoCliente.Tables[0].Rows[0]["NOMBRECANAL"].ToString();
                cliente.NombreCliente = infoCliente.Tables[0].Rows[0]["NOMBRECLIENTENIT"].ToString();
                cliente.NombreClienteNit = infoCliente.Tables[0].Rows[0]["NOMBRECLIENTENIT"].ToString();
                cliente.TipoMercado = infoCliente.Tables[0].Rows[0]["TIPOMERCADO"].ToString();
            }

            
            DataSet carteraCliente = cs.ObtenerCarteraCustomerService(cliente.Codigo);
            if (carteraCliente != null)
            {
                if (carteraCliente.Tables[0].Rows.Count > 0)
                {
                    cliente.Cartera = carteraCliente.Tables[0].Rows[0]["CARTERA"].ToString();
                    cliente.CupoCredito = carteraCliente.Tables[0].Rows[0]["CUPOCREDITO"].ToString();
                    cliente.CupoDisponible = carteraCliente.Tables[0].Rows[0]["CUPODISPONIBLE"].ToString();
                }
            }
            return cliente;
        }
    }
}
