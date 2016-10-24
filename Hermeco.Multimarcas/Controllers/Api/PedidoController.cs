using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Web;
using Hermeco.Multimarcas.Controllers.Api;
using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.Entities;
using Hermeco.Multimarcas.Services.BusinessObjects;

namespace Hermeco.Multimarcas.Controllers
{
    public class PedidoController : ApiController
    {

        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;
            HttpResponseMessage msg = new HttpResponseMessage();
            if (Session["UserNit"] != null)
            {
                string Nit = Session["UserNit"].ToString();
                PedidoService ps = new PedidoService();
                List<Referencia> referencias = ps.GetCartItemsByNit(Nit);
                msg.Content = new ObjectContent<object>(referencias, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                msg.StatusCode = HttpStatusCode.OK;
                return msg;
            }
            else
            {
                msg.Content = new ObjectContent<object>("La sesión ha sido finalizada por inactividad", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                msg.StatusCode = HttpStatusCode.NoContent;
                return msg;
            }
        }

        public HttpResponseMessage Post([FromBody] List<ItemPedido> items){
  
            var Session = HttpContext.Current.Session;
            HttpResponseMessage msg = new HttpResponseMessage();
            try
            {
                if (Session["UserNit"] != null)
                {
                    PedidoService ps = new PedidoService();
                    foreach (ItemPedido item in items)
                    {
                        if (item.Cantidad > 0)
                        {
                            CartItemEntity cie = new CartItemEntity();
                            cie.Id = item.Id;
                            cie.Nit = Session["UserNit"].ToString();
                            cie.Referencia = item.Referencia;
                            cie.Oferta = item.Oferta;
                            cie.Plu = item.Plu;
                            cie.Talla = item.Talla;
                            cie.Color = item.Color;
                            cie.Cantidad = item.Cantidad;
                            ps.AddItem(cie);
                        }
                    }
                    msg.Content = new ObjectContent<object>("El producto ha sido agregado al carro de compras", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                    msg.StatusCode = HttpStatusCode.OK;
                    return msg;
                }
                else
                {
                    msg.Content = new ObjectContent<object>("La sesión ha sido finalizada por inactividad", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                    msg.StatusCode = HttpStatusCode.Gone;
                    return msg;
                }
            }
            catch
            {
                msg.Content = new ObjectContent<object>("Ha ocurrido un error agregando el producto", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                msg.StatusCode = HttpStatusCode.BadRequest;
                return msg;
            }
        }
    }
}
