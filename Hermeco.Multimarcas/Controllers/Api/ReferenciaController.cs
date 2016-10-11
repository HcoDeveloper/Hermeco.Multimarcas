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
    public class ReferenciaController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;

            int OfertaId = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["oferta"]);
            string ReferenciaId = HttpContext.Current.Request.QueryString["refid"].ToString();
            Boolean loadImages = System.Convert.ToBoolean(HttpContext.Current.Request.QueryString["loadImages"]);

            ReferenciaService rs = new ReferenciaService();
            var referencia = rs.GetReferencia(OfertaId, ReferenciaId, true);

            HttpResponseMessage msg = new HttpResponseMessage();
            msg.Content = new ObjectContent<object>(referencia, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            msg.StatusCode = HttpStatusCode.OK;
            return msg;
        }
    }
}
