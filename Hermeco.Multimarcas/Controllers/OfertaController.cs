using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Hermeco.Multimarcas.Services;
using Hermeco.Multimarcas.Services.Entities;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.Criterion;

namespace Hermeco.Multimarcas.Controllers
{
    public class OfertaController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                var Session = HttpContext.Current.Session;
                string id = (string) Session["UserNit"];

                OfertaEntity oferta = null;
                ClienteOfertaEntity clienteOferta = null;

                var ofertas = ( from o in session.Query<OfertaEntity>()
                                    join co in session.Query<ClienteOfertaEntity>() on
                                      o.Id equals co.IdOferta
                                      where co.IdCliente.Contains(id)
                                      && DateTime.Now >= o.FechaPublicacion && DateTime.Now  <= o.FechaVencimiento
                                select o).ToList();
                HttpResponseMessage msg = new HttpResponseMessage();
                msg.Content = new ObjectContent<object>(ofertas, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                msg.StatusCode = HttpStatusCode.OK;
                return msg;
            }
        }

    }
}
