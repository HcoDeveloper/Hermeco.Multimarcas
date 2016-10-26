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
using Hermeco.Multimarcas.Services.BusinessObjects;

namespace Hermeco.Multimarcas.Controllers.Api
{
    public class CatalogoController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var Session = HttpContext.Current.Session;
            HttpResponseMessage msg = new HttpResponseMessage();
            if (Session["UserNit"] != null)
            {
                string Nit = Session["UserNit"].ToString();
                int id;
                int page;
                string RefId = string.Empty;
                List<Referencia> referencias;

                if (HttpContext.Current.Request.QueryString["oferta"] != null && HttpContext.Current.Request.QueryString["oferta"].ToString() != "undefined")
                {
                    id = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["oferta"]);
                    page = System.Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
                    ReferenciaService rs = new ReferenciaService();
                    referencias = rs.GetReferenciasByOferta(Nit, id, System.Convert.ToInt32(page), true, false);
                    HttpContext.Current.Response.AppendHeader("pages", rs.GetPagesLastQuery().ToString());
                    msg = new HttpResponseMessage();
                    msg.Content = new ObjectContent<object>(referencias, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                    msg.StatusCode = HttpStatusCode.OK;
                    return msg;
                }
                
                if (HttpContext.Current.Request.QueryString["refid"] != null)
                {
                    RefId = HttpContext.Current.Request.QueryString["refid"].ToString();
                    if (RefId != null && RefId != string.Empty)
                    {
                        ReferenciaService rs = new ReferenciaService();
                        referencias = new List<Referencia>();
                        referencias.Add(rs.GetReferencia(Nit, RefId, true, false));
                        msg = new HttpResponseMessage();
                        msg.Content = new ObjectContent<object>(referencias, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                        msg.StatusCode = HttpStatusCode.OK;
                        return msg;
                    }
                    
                }

                msg = new HttpResponseMessage();
                msg.Content = new ObjectContent<object>("No hay referencias disponibles", new System.Net.Http.Formatting.JsonMediaTypeFormatter());
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
    }
}
