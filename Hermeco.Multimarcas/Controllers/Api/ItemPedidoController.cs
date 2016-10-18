using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Hermeco.Multimarcas.Controllers.Api
{
    public class ItemPedidoController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;
            HttpResponseMessage msg = new HttpResponseMessage();
            if (Session["UserNit"] != null)
            {
                string Referencia = HttpContext.Current.Request.QueryString["refid"].ToString();
                int Oferta = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["oferta"]);
                PedidoService ps = new PedidoService();
                Referencia referencias = ps.GetCartItemById(Session["UserNit"].ToString(), Oferta, Referencia);
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
    }
}
