using Hermeco.Multimarcas.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Data;
namespace Hermeco.Multimarcas.Services
{
    public class ReferenciaService
    {
        int pages = 0;

        public int GetPagesLastQuery()
        {
            return pages;
        }

        public List<Referencia> GetReferenciasByOferta(string Nit, int Oferta, int Page, Boolean includePlu = false, Boolean includeFirstImg = false)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            int pageSize = System.Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            using (var session = SessionManager.OpenSession(connectionString))
            {
                List<Referencia> referencias = new List<Referencia>();
                int count = (from r in session.Query<ReferenciaOfertasEntity>()
                                         join co in session.Query<ClienteOfertaEntity>() on
                                         r.IdOferta equals co.IdOferta
                                         where co.IdOferta.Equals(Oferta)
                                         && r.Activa.Equals(true) 
                                         && co.IdCliente.Equals(Nit)
                                         select r).Count();
                if (pageSize > count) {
                    pages = 1;
                }
                else {
                    pages = System.Convert.ToInt32( Math.Ceiling( (double) count / pageSize));
                }
                var referenciasEntity = (from r in session.Query<ReferenciaOfertasEntity>()
                                         join co in session.Query<ClienteOfertaEntity>() on
                                         r.IdOferta equals co.IdOferta
                                         where co.IdOferta.Equals(Oferta) && r.Activa.Equals(true)
                                         && co.IdCliente.Equals(Nit)
                                         orderby r.IdReferencia
                                         select r).Skip((Page-1) * pageSize).Take(pageSize);

                string joinReferencias = string.Join(",", referenciasEntity.Select(x => x.IdReferencia.ToString()));
                string newList = string.Join(",", joinReferencias.Split(',').Select(x => string.Format("'{0}'", x)).ToList());


                CustomerService cs = new CustomerService();
                DataSet dsMaterial = cs.ObtenerMaterialCustomerService(newList);

                foreach (ReferenciaOfertasEntity referencia in referenciasEntity)
                {
                    Referencia refe = fillReferencia(referencia);
                    foreach (DataRow dr in dsMaterial.Tables[0].Select("CODMATERIAL = " + referencia.IdReferencia))
                    {
                        if (includePlu)
                        {
                            fillPlu(ref refe, dr);
                        }
                        break;   
                    }
                    fillImages(ref refe, includeFirstImg);
                    if (refe.Plu.Count > 0)
                    {
                        referencias.Add(refe);
                    }
                }
                return referencias;
            }
        }

        public List<Referencia> GetAllReferenciasByOferta(string Nit, int Oferta)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            int pageSize = System.Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            using (var session = SessionManager.OpenSession(connectionString))
            {
                List<Referencia> referencias = new List<Referencia>();
                var referenciasEntity = (from r in session.Query<ReferenciaOfertasEntity>()
                                         join co in session.Query<ClienteOfertaEntity>() on
                                         r.IdOferta equals co.IdOferta
                                         where co.IdOferta.Equals(Oferta) && r.Activa.Equals(true)
                                         && co.IdCliente.Equals(Nit)
                                         orderby r.IdReferencia
                                         select r);
                string joinReferencias = string.Join(",", referenciasEntity.Select(x => x.IdReferencia.ToString()));
                string newList = string.Join(",", joinReferencias.Split(',').Select(x => string.Format("'{0}'", x)).ToList());

                CustomerService cs = new CustomerService();
                DataSet dsMaterial = cs.ObtenerMaterialCustomerService(newList);

                foreach (ReferenciaOfertasEntity referencia in referenciasEntity)
                {
                    Referencia refe = fillReferencia(referencia);
                    foreach (DataRow dr in dsMaterial.Tables[0].Select("CODMATERIAL = " + referencia.IdReferencia))
                    {
                        fillPlu(ref refe, dr);
                    }
                    if (refe.Plu.Count > 0)
                    {
                        referencias.Add(refe);
                    }
                }
                return referencias;
            }
        }

        public Referencia GetReferencia(int OfertaId, String RefId, Boolean loadImages = false, Boolean juegoCompletoImagenes = true)
        {
            Referencia referencia = null;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
                int pageSize = System.Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
                
                using (var session = SessionManager.OpenSession(connectionString)){
                      var  referenciaOfertasEntity = (from r in session.Query<ReferenciaOfertasEntity>()
                                            where r.IdReferencia.Equals(RefId) && r.IdOferta.Equals(OfertaId) && r.Activa.Equals(true)
                                            orderby r.IdReferencia
                                            select r).Single();
                    referencia = fillReferencia(referenciaOfertasEntity);
                    fillPlu(ref referencia);
                    if (loadImages)
                    {
                        fillImages(ref referencia, juegoCompletoImagenes);
                    }
                }
            }
            catch (Exception)
            {
                //TODO: Mensaje de error;
            }
            return referencia;
        }

        public Referencia GetReferencia(String Nit, String RefId, Boolean loadImages = false, Boolean juegoCompletoImagenes = true)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            Referencia referencia = null;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                var referenciaEntity = (from r in session.Query<ReferenciaOfertasEntity>()
                                        join co in session.Query<ClienteOfertaEntity>() on r.IdOferta equals co.IdOferta
                                        join o in session.Query<OfertaEntity>() on co.IdOferta equals o.Id
                                        where r.IdReferencia.Equals(RefId)  && r.Activa.Equals(true) &&
                                        co.IdCliente.Trim().Equals(Nit.Trim())
                                        && DateTime.Now >= o.FechaPublicacion && DateTime.Now <= o.FechaVencimiento
                                        orderby r.IdReferencia
                                        select r).Single();
                referencia = fillReferencia(referenciaEntity);
                fillPlu(ref referencia);
                if (loadImages)
                {
                    fillImages(ref referencia, juegoCompletoImagenes);
                }
            }
            return referencia;
        }

        private Referencia fillReferencia(ReferenciaOfertasEntity RefEntity ){
            Referencia refe = new Referencia
            {
                    IdReferencia = RefEntity.IdReferencia,
                    Activa = RefEntity.Activa,
                    AjustePrecio = RefEntity.AjustePrecio,
                    CategoriaComercial = RefEntity.CategoriaComercial,
                    FecUser = RefEntity.FecUser,
                    GuiaTallas = RefEntity.GuiaTallas,
                    IdOferta = RefEntity.IdOferta,
                    NomUser = RefEntity.NomUser,
                    Orden = RefEntity.Orden,
                    ValidarInventario = RefEntity.ValidarInventario
            };
            return refe;
        }

        public void fillPlu(ref Referencia referencia, DataRow dr)
        {
            Plu plu = new Plu();
            plu.Codigo = dr["PLU"] == DBNull.Value ? "" : dr["PLU"].ToString();
            plu.Color = dr["COLOR"] == DBNull.Value ? "" : dr["COLOR"].ToString();
            plu.Talla = dr["CODIGOTALLA"] == DBNull.Value ? "" : dr["CODIGOTALLA"].ToString();
            plu.Precio = System.Convert.ToInt32(dr["PRECIO"] == DBNull.Value ? "" : dr["PRECIO"]);
            plu.GrupoArticulo = dr["GRUPOART"] == DBNull.Value ? "" : dr["GRUPOART"].ToString();
            plu.AnoVenta = dr["ANOVENTA"] == DBNull.Value ? "" : dr["ANOVENTA"].ToString();
            plu.Clase = dr["CLASE"] == DBNull.Value ? "" : dr["CLASE"].ToString();
            plu.CodigoColor = dr["CODIGOCOLOR"] == DBNull.Value ? "" : dr["CODIGOCOLOR"].ToString();
            plu.Coleccion = dr["COLECCION"] == DBNull.Value ? "" : dr["COLECCION"].ToString();
            plu.ConcDiseno = dr["CONCDISENO"] == DBNull.Value ? "" : dr["CONCDISENO"].ToString();
            plu.DenomGrupoArtiulo = dr["DENOMGRUPOART"] == DBNull.Value ? "" : dr["DENOMGRUPOART"].ToString();
            plu.DenomIVA = System.Convert.ToDouble(dr["DENOMIVA"] == DBNull.Value ? "" : dr["DENOMIVA"].ToString());
            plu.Descripcion = dr["DESCRIPMATERIAL"] == DBNull.Value ? "" : dr["DESCRIPMATERIAL"].ToString();
            plu.Destino = dr["DESTINO"] == DBNull.Value ? "" : dr["DESTINO"].ToString();
            plu.Edad = dr["EDAD"] == DBNull.Value ? "" : dr["EDAD"].ToString();
            plu.Edad1 = dr["EDAD1"] == DBNull.Value ? "" : dr["EDAD1"].ToString();
            plu.FechaInicio = dr["FECHAINICIO"] == DBNull.Value ? "" : dr["FECHAINICIO"].ToString();
            plu.FechaFin = dr["FECHAFIN"] == DBNull.Value ? "" : dr["FECHAFIN"].ToString();
            plu.Genero = dr["GENERO"] == DBNull.Value ? "" : dr["GENERO"].ToString();
            plu.Genero1 = dr["GENERO1"] == DBNull.Value ? "" : dr["GENERO1"].ToString();
            plu.GrupoArticuloExt = dr["GRUPOARTEXT"] == DBNull.Value ? "" : dr["GRUPOARTEXT"].ToString();
            plu.IVA = System.Convert.ToInt32(dr["IVA"] == DBNull.Value ? "" : dr["IVA"].ToString());
            plu.Marca = dr["MARCA"] == DBNull.Value ? "" : dr["MARCA"].ToString();
            plu.MesVenta = dr["MESVENTA"] == DBNull.Value ? "" : dr["MESVENTA"].ToString();
            plu.Moneda = dr["MONEDA"] == DBNull.Value ? "" : dr["MONEDA"].ToString();
            plu.PaisOrigen = dr["PAISORIG"] == DBNull.Value ? "" : dr["PAISORIG"].ToString();
            plu.PLU = dr["PLU"] == DBNull.Value ? "" : dr["PLU"].ToString();
            //Saldo = System.Convert.ToDouble(dr["SALDO"] == DBNull.Value ? "":  dr["SALDO"].ToString());
            plu.stock = System.Convert.ToDouble(dr["STOCK"] == DBNull.Value ? "" : dr["STOCK"].ToString());
            plu.SubLinea = dr["SUBLINEA"] == DBNull.Value ? "" : dr["SUBLINEA"].ToString();
            plu.SubLinea1 = dr["SUBLINEA1"] == DBNull.Value ? "" : dr["SUBLINEA1"].ToString();
            plu.TipoMaterial = dr["TIPOMAT"] == DBNull.Value ? "" : dr["TIPOMAT"].ToString();
            plu.TipoNegocio = dr["TIPONEGOCIO"] == DBNull.Value ? "" : dr["TIPONEGOCIO"].ToString();
            plu.TipoNegocio1 = dr["TIPONEGOC"] == DBNull.Value ? "" : dr["TIPONEGOC"].ToString();
            plu.TipoReferencia = dr["TIPOREFERENCIA"] == DBNull.Value ? "" : dr["TIPOREFERENCIA"].ToString();
            plu.TipoTejido = dr["TIPOTEJIDO"] == DBNull.Value ? "" : dr["TIPOTEJIDO"].ToString();
            plu.ValorComposicion = dr["VALORCOMPOSICION"] == DBNull.Value ? "" : dr["VALORCOMPOSICION"].ToString();
            plu.Mundo = Utility.GetMundo(plu.Genero, plu.Edad);
            referencia.Plu.Add(plu);
            if (!referencia.Colores.Contains(dr["COLOR"].ToString()))
            {
                referencia.Colores.Add(dr["COLOR"].ToString());
            }

            if (!referencia.Tallas.Contains(dr["CODIGOTALLA"].ToString()))
            {
                referencia.Tallas.Add(dr["CODIGOTALLA"].ToString());
            }
        }

        public void fillPlu(ref Referencia referencia)
        {
            CustomerService cs = new CustomerService();
            DataSet dsMaterial = cs.ObtenerMaterialCustomerService("'" + referencia.IdReferencia + "'");
            foreach(DataRow dr in dsMaterial.Tables[0].Rows ){
                Plu plu = new Plu();
                plu.Codigo = dr["PLU"] == DBNull.Value ? "":  dr["PLU"].ToString();
                plu.Color = dr["COLOR"] == DBNull.Value ? "" : dr["COLOR"].ToString();
                plu.Talla = dr["CODIGOTALLA"] == DBNull.Value ? "" : dr["CODIGOTALLA"].ToString(); 
                plu.Precio = System.Convert.ToInt32( dr["PRECIO"] == DBNull.Value ? "":  dr["PRECIO"]);
                plu.GrupoArticulo = dr["GRUPOART"] == DBNull.Value ? "":  dr["GRUPOART"].ToString();
                plu.AnoVenta = dr["ANOVENTA"] == DBNull.Value ? "":  dr["ANOVENTA"].ToString();
                plu.Clase = dr["CLASE"] == DBNull.Value ? "":  dr["CLASE"].ToString();
                plu.CodigoColor = dr["CODIGOCOLOR"] == DBNull.Value ? "":  dr["CODIGOCOLOR"].ToString();
                plu.Coleccion = dr["COLECCION"] == DBNull.Value ? "":  dr["COLECCION"].ToString();
                plu.ConcDiseno = dr["CONCDISENO"] == DBNull.Value ? "":  dr["CONCDISENO"].ToString();
                plu.DenomGrupoArtiulo = dr["DENOMGRUPOART"] == DBNull.Value ? "":  dr["DENOMGRUPOART"].ToString();
                plu.DenomIVA = System.Convert.ToDouble(dr["DENOMIVA"] == DBNull.Value ? "":  dr["DENOMIVA"].ToString());
                plu.Descripcion = dr["DESCRIPMATERIAL"] == DBNull.Value ? "" : dr["DESCRIPMATERIAL"].ToString();
                plu.Destino = dr["DESTINO"] == DBNull.Value ? "":  dr["DESTINO"].ToString();
                plu.Edad = dr["EDAD"] == DBNull.Value ? "":  dr["EDAD"].ToString();
                plu.Edad1 = dr["EDAD1"] == DBNull.Value ? "":  dr["EDAD1"].ToString();
                plu.FechaInicio = dr["FECHAINICIO"] == DBNull.Value ? "":  dr["FECHAINICIO"].ToString();
                plu.FechaFin = dr["FECHAFIN"] == DBNull.Value ? "":  dr["FECHAFIN"].ToString();
                plu.Genero = dr["GENERO"] == DBNull.Value ? "":  dr["GENERO"].ToString();
                plu.Genero1 = dr["GENERO1"] == DBNull.Value ? "":  dr["GENERO1"].ToString();
                plu.GrupoArticuloExt = dr["GRUPOARTEXT"] == DBNull.Value ? "":  dr["GRUPOARTEXT"].ToString();
                plu.IVA = System.Convert.ToInt32(dr["IVA"] == DBNull.Value ? "":  dr["IVA"].ToString());
                plu.Marca = dr["MARCA"] == DBNull.Value ? "":  dr["MARCA"].ToString();
                plu.MesVenta = dr["MESVENTA"] == DBNull.Value ? "":  dr["MESVENTA"].ToString();
                plu.Moneda = dr["MONEDA"] == DBNull.Value ? "":  dr["MONEDA"].ToString();
                plu.PaisOrigen = dr["PAISORIG"] == DBNull.Value ? "":  dr["PAISORIG"].ToString();
                plu.PLU = dr["PLU"] == DBNull.Value ? "":  dr["PLU"].ToString();
                //Saldo = System.Convert.ToDouble(dr["SALDO"] == DBNull.Value ? "":  dr["SALDO"].ToString());
                plu.stock = System.Convert.ToDouble(dr["STOCK"] == DBNull.Value ? "":  dr["STOCK"].ToString());
                plu.SubLinea = dr["SUBLINEA"] == DBNull.Value ? "":  dr["SUBLINEA"].ToString();
                plu.SubLinea1 = dr["SUBLINEA1"] == DBNull.Value ? "":  dr["SUBLINEA1"].ToString();
                plu.TipoMaterial = dr["TIPOMAT"] == DBNull.Value ? "":  dr["TIPOMAT"].ToString();
                plu.TipoNegocio = dr["TIPONEGOCIO"] == DBNull.Value ? "":  dr["TIPONEGOCIO"].ToString();
                plu.TipoNegocio1 = dr["TIPONEGOC"] == DBNull.Value ? "":  dr["TIPONEGOC"].ToString();
                plu.TipoReferencia = dr["TIPOREFERENCIA"] == DBNull.Value ? "":  dr["TIPOREFERENCIA"].ToString();
                plu.TipoTejido = dr["TIPOTEJIDO"] == DBNull.Value ? "":  dr["TIPOTEJIDO"].ToString();
                plu.ValorComposicion = dr["VALORCOMPOSICION"] == DBNull.Value ? "":  dr["VALORCOMPOSICION"].ToString();
                plu.Mundo = Utility.GetMundo(plu.Genero, plu.Edad);
                referencia.Plu.Add(plu);
                

                if (!referencia.Tallas.Contains(dr["CODIGOTALLA"].ToString()))
                {
                    referencia.Tallas.Add(dr["CODIGOTALLA"].ToString());
                }

                if (!referencia.Colores.Contains(dr["COLOR"].ToString()))
                {
                    referencia.Colores.Add(dr["COLOR"].ToString());
                }

            }
        }

        

        private void fillImages(ref Referencia referencia, Boolean juegoCompleto = true)
        {
            string IdReferencia = referencia.IdReferencia;
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                //List<string> Colores = new List<string>();
                List<ImagenesReferenciaEntity> imagenes = null;
                if (juegoCompleto) {
                    imagenes = (from i in session.Query<ImagenesReferenciaEntity>()
                                                               where i.IdReferencia.Equals(IdReferencia)
                                                               && i.Clasificacion.Equals(1)
                                                               orderby i.Orden descending
                                                               select i).ToList();
                } else
                {
                    imagenes = (from i in session.Query<ImagenesReferenciaEntity>()
                                                               where i.IdReferencia.Equals(IdReferencia)
                                                               && i.Clasificacion.Equals(1)
                                                               orderby i.Orden descending
                                                               select i).Take(1).ToList();
                }

                foreach (ImagenesReferenciaEntity imagen in imagenes)
                {

                    //if (referencia.Colores.Contains(imagen.DescripcionColor))
                    //{
                    //    if (!Colores.Contains(imagen.DescripcionColor))
                    //    {
                    //        Colores.Add(imagen.DescripcionColor);
                    //    }
                    //}
                    referencia.Imagenes.Add(new Imagenes
                    {
                        Clasificacion = imagen.Clasificacion,
                        Color = imagen.Color,
                        CreadaPor = imagen.CreadaPor,
                        Descripcion = imagen.Descripcion,
                        DescripcionColor = imagen.DescripcionColor,
                        FechaCreada = imagen.FechaCreada,
                        IdImagen = imagen.IdImagen,
                        IdReferencia = imagen.IdReferencia,
                        //Imagen = juegoCompleto? imagen.Imagen : Utility.resizeImage( imagen.Imagen, 300, 300),
                        Imagen = Utility.resizeImage(imagen.Imagen, 300, 300),
                        Orden = imagen.Orden
                    });             
                }
                //referencia.Colores = Colores;
            }
        }
    }
}
