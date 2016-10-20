using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Configuration;
using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.Entities;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate;
using System.Threading.Tasks;

namespace Hermeco.Multimarcas.Controllers.Api
{
    public class CatalogoController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;
            string Nit = Session["UserNit"].ToString();
            int id = System.Convert.ToInt32( HttpContext.Current.Request.QueryString["oferta"]);
            var page = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);

            ReferenciaService rs = new ReferenciaService();
            var referencias = rs.GetReferenciasByOferta(Nit ,id, System.Convert.ToInt32(page), true, false);

            HttpContext.Current.Response.AppendHeader("pages", rs.GetPagesLastQuery().ToString());
            HttpResponseMessage msg = new HttpResponseMessage();
            msg.Content = new ObjectContent<object>(referencias, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            msg.StatusCode = HttpStatusCode.OK;
            return msg;
         }
    }
}
