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
                foreach (ReferenciaOfertasEntity referencia in referenciasEntity)
                {
                    Referencia refe = fillReferencia(referencia);
                    if (includePlu)
                    {
                        fillPlu(ref refe);
                    }
                    if (includeFirstImg)
                    {
                        fillImages(ref refe, false);
                    }
                    referencias.Add(refe);
                }

               
                return referencias;
            }
        }

        public Referencia GetReferencia(int OfertaId,String RefId, Boolean loadImages = false){
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            int pageSize = System.Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            Referencia referencia = null;
            using (var session = SessionManager.OpenSession(connectionString)){
                    var referenciaEntity = (from r in session.Query<ReferenciaOfertasEntity>()
                                        where r.IdReferencia.Equals(RefId) && r.IdOferta.Equals(OfertaId) && r.Activa.Equals(true)
                                        orderby r.IdReferencia
                                        select r).Single();
                referencia = fillReferencia(referenciaEntity);
                fillPlu(ref referencia);
                if (loadImages)
                {
                    fillImages(ref referencia, true);
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

        private void fillPlu(ref Referencia referencia)
        {
            CustomerService cs = new CustomerService();
            DataSet dsMaterial = cs.ObtenerMaterialCustomerService(referencia.IdReferencia);
            foreach(DataRow dr in dsMaterial.Tables[0].Rows ){
                referencia.Plu.Add(new Plu { 
                    Codigo = dr["PLU"] == DBNull.Value ? "":  dr["PLU"].ToString(),
                    Color = dr["COLOR"] == DBNull.Value ? "" : dr["COLOR"].ToString(),
                    Talla = dr["CODIGOTALLA"] == DBNull.Value ? "" : dr["CODIGOTALLA"].ToString(), 
                    Precio = System.Convert.ToInt32( dr["PRECIO"] == DBNull.Value ? "":  dr["PRECIO"]),
                    GrupoArticulo = dr["GRUPOART"] == DBNull.Value ? "":  dr["GRUPOART"].ToString(),
                    AnoVenta = dr["ANOVENTA"] == DBNull.Value ? "":  dr["ANOVENTA"].ToString(),
                    Clase = dr["CLASE"] == DBNull.Value ? "":  dr["CLASE"].ToString(),
                    CodigoColor = dr["CODIGOCOLOR"] == DBNull.Value ? "":  dr["CODIGOCOLOR"].ToString(),
                    Coleccion = dr["COLECCION"] == DBNull.Value ? "":  dr["COLECCION"].ToString(),
                    ConcDiseno = dr["CONCDISENO"] == DBNull.Value ? "":  dr["CONCDISENO"].ToString(),
                    DenomGrupoArtiulo = dr["DENOMGRUPOART"] == DBNull.Value ? "":  dr["DENOMGRUPOART"].ToString(),
                    DenomIVA = System.Convert.ToDouble(dr["DENOMIVA"] == DBNull.Value ? "":  dr["DENOMIVA"].ToString()),
                    Descripcion = dr["DESCRIPMATERIAL"] == DBNull.Value ? "" : dr["DESCRIPMATERIAL"].ToString(),
                    Destino = dr["DESTINO"] == DBNull.Value ? "":  dr["DESTINO"].ToString(),
                    Edad = dr["EDAD"] == DBNull.Value ? "":  dr["EDAD"].ToString(),
                    Edad1 = dr["EDAD1"] == DBNull.Value ? "":  dr["EDAD1"].ToString(),
                    FechaInicio = dr["FECHAINICIO"] == DBNull.Value ? "":  dr["FECHAINICIO"].ToString(),
                    FechaFin = dr["FECHAFIN"] == DBNull.Value ? "":  dr["FECHAFIN"].ToString(),
                    Genero = dr["GENERO"] == DBNull.Value ? "":  dr["GENERO"].ToString(),
                    Genero1 = dr["GENERO1"] == DBNull.Value ? "":  dr["GENERO1"].ToString(),
                    GrupoArticuloExt = dr["GRUPOARTEXT"] == DBNull.Value ? "":  dr["GRUPOARTEXT"].ToString(),
                    IVA = System.Convert.ToInt32(dr["IVA"] == DBNull.Value ? "":  dr["IVA"].ToString()),
                    Marca = dr["MARCA"] == DBNull.Value ? "":  dr["MARCA"].ToString(),
                    MesVenta = dr["MESVENTA"] == DBNull.Value ? "":  dr["MESVENTA"].ToString(),
                    Moneda = dr["MONEDA"] == DBNull.Value ? "":  dr["MONEDA"].ToString(),
                    PaisOrigen = dr["PAISORIG"] == DBNull.Value ? "":  dr["PAISORIG"].ToString(),
                    PLU = dr["PLU"] == DBNull.Value ? "":  dr["PLU"].ToString(),
                    //Saldo = System.Convert.ToDouble(dr["SALDO"] == DBNull.Value ? "":  dr["SALDO"].ToString()),
                    stock = System.Convert.ToDouble(dr["STOCK"] == DBNull.Value ? "":  dr["STOCK"].ToString()),
                    SubLinea = dr["SUBLINEA"] == DBNull.Value ? "":  dr["SUBLINEA"].ToString(),
                    SubLinea1 = dr["SUBLINEA1"] == DBNull.Value ? "":  dr["SUBLINEA1"].ToString(),
                    TipoMaterial = dr["TIPOMAT"] == DBNull.Value ? "":  dr["TIPOMAT"].ToString(),
                    TipoNegocio = dr["TIPONEGOCIO"] == DBNull.Value ? "":  dr["TIPONEGOCIO"].ToString(),
                    TipoNegocio1 = dr["TIPONEGOC"] == DBNull.Value ? "":  dr["TIPONEGOC"].ToString(),
                    TipoReferencia = dr["TIPOREFERENCIA"] == DBNull.Value ? "":  dr["TIPOREFERENCIA"].ToString(),
                    TipoTejido = dr["TIPOTEJIDO"] == DBNull.Value ? "":  dr["TIPOTEJIDO"].ToString(),
                    ValorComposicion = dr["VALORCOMPOSICION"] == DBNull.Value ? "":  dr["VALORCOMPOSICION"].ToString()
                });

                if (!referencia.Colores.Contains(dr["COLOR"].ToString()))
                {
                    referencia.Colores.Add(dr["COLOR"].ToString());
                }

                if (!referencia.Tallas.Contains(dr["CODIGOTALLA"].ToString()))
                {
                    referencia.Tallas.Add(dr["CODIGOTALLA"].ToString());
                }
            }
        }

        private void fillImages(ref Referencia referencia, Boolean juegoCompleto = true)
        {
            string IdReferencia = referencia.IdReferencia;
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
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
                        Imagen = juegoCompleto? imagen.Imagen : Utility.resizeImage( imagen.Imagen, 300, 300),
                        Orden = imagen.Orden
                    });
                }
            }
        }
    }
}
