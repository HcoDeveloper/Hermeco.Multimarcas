using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.Entities;
using NHibernate.Linq;
using Hermeco.Multimarcas.Services.BusinessObjects;

namespace Hermeco.Multimarcas.Controllers
{
    public class OfertaController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;
            string id = (string)Session["UserNit"];
            int idOferta = 0;
            if (HttpContext.Current.Request.QueryString["oferta"] != null && HttpContext.Current.Request.QueryString["oferta"].ToString() != "undefined")
            {
                idOferta = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["oferta"]);
            }
            
            HttpContext.Current.Response.AppendHeader("ofertaActiva", idOferta.ToString());
            OfertaService os = new OfertaService();
            List<Oferta>  ofertas = os.GetOfertasByNit(id, idOferta);
            HttpResponseMessage msg = new HttpResponseMessage();
            msg.Content = new ObjectContent<object>(ofertas, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            msg.StatusCode = HttpStatusCode.OK;
            return msg;
        }
    }
}
