using Hermeco.Multimarcas.Services.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hermeco.Multimarcas.Services
{
    public class PedidoService
    {
        public int AddItem(CartItemEntity entity)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                session.SaveOrUpdate(entity);
            }
            return 1;
        }

        public Boolean ValidateItem( string Plu, int Cantidad)
        {
            return false;
        }

        public void ValidarPedido()
        {

        }

    }
}
