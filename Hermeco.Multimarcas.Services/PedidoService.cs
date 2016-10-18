using Hermeco.Multimarcas.Services.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate;
using System.Threading.Tasks;
using System.Data;
using Hermeco.Multimarcas.Services.BusinessObjects;

namespace Hermeco.Multimarcas.Services
{
    public class PedidoService
    {
        public int AddItem(CartItemEntity entity)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                if (entity.Id == 0)
                {
                    session.Save(entity);
                }
                else
                {
                    session.Update(entity);
                    session.Flush();
                }
            }
            return 1;
        }

        public List<Referencia> GetCartItemsByNit(string Nit)
        {
            List<Referencia> referencias = new List<Referencia>();
            List<CartItemEntity> cartItems = null;
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                cartItems = (from c in session.Query<CartItemEntity>()
                                         where c.Nit.Equals(Nit)
                                         orderby c.Referencia, c.Plu 
                                         select c).ToList();
            }
            if (cartItems != null && cartItems.Count > 0)
            {
                ReferenciaService rs = new ReferenciaService();
                Referencia referencia = rs.GetReferencia(cartItems[0].Oferta, cartItems[0].Referencia, true);
                foreach (CartItemEntity cartItem in cartItems)
                {
                    if (referencia.IdReferencia != cartItem.Referencia)
                    {
                        referencias.Add(referencia);
                        referencia = rs.GetReferencia(cartItem.Oferta, cartItem.Referencia, true);
                    }
                    Plu plu = referencia.Plu.Find(x => x.PLU == cartItem.Plu);
                    int index = referencia.Plu.IndexOf(plu);
                    plu.Cantidad = cartItem.Cantidad;
                    plu.itemId = cartItem.Id;
                    referencia.Plu[index] = plu;
                }
                if (cartItems.Count > 0)
                {
                    referencias.Add(referencia);
                }
            }
            return referencias;
        }

        public Referencia GetCartItemById(string Nit, int oferta, string sReferencia)
        {
            Referencia referencias = new Referencia();
            List<CartItemEntity> cartItems = null;
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                cartItems = (from c in session.Query<CartItemEntity>()
                            where c.Oferta.Equals(oferta) && c.Referencia.Equals(sReferencia) && c.Nit.Equals(Nit)
                            orderby c.Referencia, c.Plu
                            select c).ToList();
            }
            ReferenciaService rs = new ReferenciaService();
            Referencia referencia = rs.GetReferencia(oferta, sReferencia, true);
            foreach (CartItemEntity cartItem in cartItems)
            {
                Plu plu = referencia.Plu.Find(x => x.PLU == cartItem.Plu);
                int index = referencia.Plu.IndexOf(plu);
                plu.Cantidad = cartItem.Cantidad;
                plu.itemId = cartItem.Id;
                referencia.Plu[index] = plu;
            }
            return referencia;
        }


        public Boolean ValidateItem( string Plu, int Cantidad)
        {
            return false;
        }

        public void ValidarPedido()
        {

        }

        public void ProcesarPedido()
        {

        }

    }
}
