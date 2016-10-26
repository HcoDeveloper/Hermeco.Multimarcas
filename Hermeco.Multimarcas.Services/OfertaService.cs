using Hermeco.Multimarcas.Services.BusinessObjects;
using Hermeco.Multimarcas.Services.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace Hermeco.Multimarcas.Services
{
    public class OfertaService
    {
        public List<Oferta> GetOfertasByNit(string Nit, int activeOferta = 0)
        {
            List<Oferta> ofertas = new List<Oferta>();
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                
                var ofertasEntity = (from o in session.Query<OfertaEntity>()
                               join co in session.Query<ClienteOfertaEntity>() on
                                 o.Id equals co.IdOferta
                               where co.IdCliente.Trim().Equals(Nit.Trim())
                               && DateTime.Now >= o.FechaPublicacion && DateTime.Now <= o.FechaVencimiento
                               select o).ToList();
                foreach (OfertaEntity ofertaEntity in ofertasEntity)
                {
                    Oferta oferta = new Oferta();
                    oferta.Id = ofertaEntity.Id;
                    oferta.Nombre = ofertaEntity.Nombre;
                    oferta.Imagen = ofertaEntity.Imagen;
                    oferta.NomUser = ofertaEntity.NomUser;
                    oferta.Tipo = ofertaEntity.Tipo;
                    oferta.Visible = ofertaEntity.Visible;
                    oferta.Bodega = ofertaEntity.Bodega;
                    oferta.Descripcion = ofertaEntity.Descripcion;
                    oferta.FechaPublicacion = ofertaEntity.FechaPublicacion;
                    oferta.FechaVencimiento = ofertaEntity.FechaVencimiento;

                    oferta.FecUser = ofertaEntity.FecUser;
                    //if (oferta.Id == activeOferta)
                    //{

                    //    ReferenciaService rs = new ReferenciaService();
                    //    List<Referencia> referencias = rs.GetAllReferenciasByOferta(Nit, ofertaEntity.Id);
                    //    //int TotalReferencia = referencias.Count();
                    //    //foreach (Referencia referencia in referencias)
                    //    //{
                    //    //    Mundo mundo = new Mundo();
                    //    //    string NombreMundo = Utility.GetMundo(referencia.Plu[0].Genero.ToString(), referencia.Plu[0].Edad.ToString());
                    //    //    /*
                    //    //    foreach (Plu plu in referencia.Plu)
                    //    //    {
                    //    //        NombreMundo = Utility.GetMundo(plu.Genero.ToString(), plu.Edad.ToString());
                    //    //    }
                    //    //    if (oferta.Mundos.Any(item => item.Nombre.Equals(NombreMundo)))
                    //    //    {
                    //    //        var mundoUpdate = oferta.Mundos.FirstOrDefault(x => x.Nombre == NombreMundo);
                    //    //        if (mundoUpdate != null)
                    //    //        {
                    //    //            mundoUpdate.Cantidad++;
                    //    //        }
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        mundo.Cantidad = 1;
                    //    //        mundo.Nombre = NombreMundo;
                    //    //        oferta.Mundos.Add(mundo);
                    //    //    }
                    //    //     */
                    //    //    if (!oferta.Mundos.Any(item => item.Nombre.Equals(NombreMundo)))
                    //    //    {
                    //    //        mundo.Nombre = NombreMundo;
                    //    //        oferta.Mundos.Add(mundo);
                    //    //    }
                    //    //}
                    //}
                    ofertas.Add(oferta);
                }
            }
            return ofertas;
        }

    }
}
