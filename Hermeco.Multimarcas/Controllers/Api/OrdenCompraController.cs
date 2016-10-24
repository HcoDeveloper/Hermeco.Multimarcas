using Hermeco.Multimarcas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Hermeco.Multimarcas.Controllers.Api
{
    public class OrdenCompraController : ApiController
    {
        public HttpResponseMessage Post()
        {
            var Session = HttpContext.Current.Session;
            HttpResponseMessage msg = new HttpResponseMessage();
            if (Session["UserNit"] != null)
            {
                 string Nit = Session["UserNit"].ToString();
                PedidoService ps = new PedidoService();
                string Result = ps.ProcesarPedido(Nit);
                msg.Content = new ObjectContent<object>(Result, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                msg.StatusCode = HttpStatusCode.OK;
                return msg;
            } else
            {
                msg.Content = new ObjectContent<object>("La sesión ha sido finalizada por inactividad", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                msg.StatusCode = HttpStatusCode.OK;
                return msg;
            }
        }
    }
}
