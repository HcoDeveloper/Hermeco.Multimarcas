using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Hermeco.Multimarcas.Controllers.Api
{
    public class AccountController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;
            string Nit = Session["UserNit"].ToString();
            ClienteService cs = new ClienteService();
            Cliente cliente = cs.getInfoClient(Nit);
            HttpResponseMessage msg = new HttpResponseMessage();
            msg.Content = new ObjectContent<object>(cliente, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            msg.StatusCode = HttpStatusCode.OK;
            return msg;
        }
    }
}
