using Hermeco.Multimarcas.Services.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.Criterion;
using NHibernate;
using System.Threading.Tasks;
using System.Data;
using Hermeco.Multimarcas.Services.BusinessObjects;
using System.IO;
using System.Data.Common;
using System.Xml;
using System.Data.SqlClient;

namespace Hermeco.Multimarcas.Services
{
    public class PedidoService
    {
        private static Dictionary<string, string> dicParametros;

        public PedidoService()
        {

            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            dicParametros = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            string strValor = "";


            var connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand oCmd = connection.CreateCommand();
            if (connection != null)
            {
                using (connection)
                {
                    
                    oCmd.CommandText = "usp_HER_Generico_ObtenerParametrosAplicacion";
                    oCmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter param = oCmd.CreateParameter();
                    param.ParameterName = "@strAplicacion";
                    param.DbType = DbType.String;
                    param.Value = "'wsTiendaVirtual'";
                    oCmd.Parameters.Add(param);
                
                    using (IDataReader oDR = oCmd.ExecuteReader())
                    {
                        while (oDR.Read())
                        {
                            strValor = oDR.GetString(0);
                            if (!dicParametros.ContainsKey(strValor))
                                dicParametros.Add(strValor, oDR.GetString(1));
                        }
                        oDR.Close();
                        oDR.Dispose();
                    }
                }
            }
        }

        public int AddItem(CartItemEntity entity)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                if (entity.Id == 0)
                {
                    session.Save(entity);
                }
                else
                {
                    session.Update(entity);
                    session.Flush();
                }
            }
            return 1;
        }

        public List<Referencia> GetCartItemsByNit(string Nit)
        {
            List<Referencia> referencias = new List<Referencia>();
            List<CartItemEntity> cartItems = null;
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                cartItems = (from c in session.Query<CartItemEntity>()
                                         where c.Nit.Equals(Nit)
                                         orderby c.Referencia, c.Plu 
                                         select c).ToList();
            }
            if (cartItems != null && cartItems.Count > 0)
            {
                ReferenciaService rs = new ReferenciaService();
                Referencia referencia = rs.GetReferencia(cartItems[0].Oferta, cartItems[0].Referencia, true);
                List<string> Colores = new List<string>();
                foreach (CartItemEntity cartItem in cartItems)
                {
                    if (cartItem.Cantidad > 0)
                    {
                        if (referencia.IdReferencia != cartItem.Referencia)
                        {
                            referencias.Add(referencia);
                            referencia = rs.GetReferencia(cartItem.Oferta, cartItem.Referencia, true);
                        }
                        Plu plu = referencia.Plu.Find(x => x.PLU == cartItem.Plu);
                        int index = referencia.Plu.IndexOf(plu);
                        plu.Cantidad = cartItem.Cantidad;
                        plu.itemId = cartItem.Id;
                        referencia.Plu[index] = plu;
                    }
                }
                if (cartItems.Count > 0)
                {
                    referencia.Colores = Colores;
                    referencias.Add(referencia);
                }
            }
            return referencias;
        }

        public Referencia GetCartItemById(string Nit, int oferta, string sReferencia)
        {
            Referencia referencias = new Referencia();
            List<CartItemEntity> cartItems = null;
            var connectionString = ConfigurationManager.ConnectionStrings["dbComercial"].ConnectionString;
            using (var session = SessionManager.OpenSession(connectionString))
            {
                cartItems = (from c in session.Query<CartItemEntity>()
                            where c.Oferta.Equals(oferta) && c.Referencia.Equals(sReferencia) && c.Nit.Equals(Nit)
                            orderby c.Referencia, c.Plu
                            select c).ToList();
            }
            ReferenciaService rs = new ReferenciaService();
            Referencia referencia = rs.GetReferencia(oferta, sReferencia, true);
            foreach (CartItemEntity cartItem in cartItems)
            {
                Plu plu = referencia.Plu.Find(x => x.PLU == cartItem.Plu);
                int index = referencia.Plu.IndexOf(plu);
                plu.Cantidad = cartItem.Cantidad;
                plu.itemId = cartItem.Id;
                referencia.Plu[index] = plu;
            }
            return referencia;
        }

        public string ProcesarPedido(string Nit)
        {

            string xmlPI = "";
            try
            {

                string strUsuario = dicParametros["UsuarioPI"].ToString();
                string strClavePI = dicParametros["ClavePI"].ToString();
                System.Net.NetworkCredential userDefined = new System.Net.NetworkCredential(strUsuario, strClavePI);

                wsPedidos.CreaPedido_Sync_OutService clsPedido = new wsPedidos.CreaPedido_Sync_OutService();
                clsPedido.Credentials = userDefined;

                ClienteService cs = new ClienteService();
                Cliente InfoCliente = cs.getInfoClient(Nit);
                                
                //---------------------------------------------------------------------------------------------------------
                //Consulta info cliente
                string strCondicionPago = InfoCliente.FormaPago;
                string strTipoDoc_Boutique = dicParametros["TipoDoc_Boutique"]; //Valor: ""
                string strTipoDoc_BoutiqueEntrega = dicParametros["TipoDoc_BoutiqueEntrega"]; //Valor: ""
                string strTipoDoc_BoutiqueExterior = dicParametros["TipoDoc_BoutiqueExterior"]; //Valor: ""
                string strTipoDoc_BoutiqueExteriorEntrega = dicParametros["TipoDoc_BoutiqueExteriorEntrega"]; //Valor: ""
                string strMercado = InfoCliente.TipoMercado;
                 clsPedido.Credentials = userDefined;
                //---------------------------------------------------------------------------------------------------------
                //Consulta cartera              
                

                //---------------------------------------------------------------------------------------------------------
                wsPedidos.InfoPedidos oPedido = new wsPedidos.InfoPedidos();
                oPedido.Cabecera = new wsPedidos.CabeceraPed();
                string strCliente = InfoCliente.Nit; //DatosCliente.Cliente[0].NitCliente; // "DocumentoCliente";
                oPedido.Cabecera.TipoDoc = "ZPBT";//dicParametros["TipoDoc_Boutique"]; // Resp.Cliente[0].Ventas[0].TipoInterfaz.ToString();   //dicParametros["TipoDoc_Boutique"];
                oPedido.Cabecera.DocRef = "";
                oPedido.Cabecera.Moneda = ""; //DatosCliente.Cliente[0].Moneda; // "Moneda";
                oPedido.Cabecera.NoCliente = InfoCliente.Nit; //DatosCliente.Cliente[0].NitCliente; // "DocumentoCliente";
                oPedido.Cabecera.Dest_Mercancia = InfoCliente.Nit;
                oPedido.Cabecera.Dest_Factura = InfoCliente.Nit;
                oPedido.Cabecera.Vendedor = ""; //DatosCliente.Cliente[0].CodigoVendedor;
                oPedido.Cabecera.FechaEntregaSpecified = true;
                oPedido.Cabecera.FechaEntrega = DateTime.Now;
                oPedido.Cabecera.Observaciones = "";
                oPedido.Cabecera.FechaDoc = DateTime.Now;
                oPedido.Cabecera.Condpago = ""; // strCondicionPago;
                oPedido.Cabecera.Grupovend = "";
                oPedido.Cabecera.Ind_bloqueo = ""; //CalcularIndicadorBloqueo -> por cartera vencida
                oPedido.Cabecera.Ind_reserva = ""; //(" ": Arun contra reserva / "X" contra el stock)
                oPedido.Cabecera.Ind_asigStock = ""; //sin reserva previa (usuario) -> no liberar
                oPedido.Cabecera.Ind_entrega = "X"; //Calcular -> if Ind_bloqueo = 1 then Indentrega = 0;  if Ind_bloqueo = 0 then Indentrega = 1
                if (strCondicionPago == dicParametros["CondicionPagoCONTADO"])
                {
                    oPedido.Cabecera.Ind_entrega = " ";

                    if (strMercado == "1010")
                    {
                        oPedido.Cabecera.TipoDoc = strTipoDoc_Boutique;
                    }
                    else if (strMercado == "1020")
                    {
                        oPedido.Cabecera.TipoDoc = strTipoDoc_BoutiqueExterior;
                    }
                }
                else
                {
                    oPedido.Cabecera.Ind_entrega = " ";
                    if (strMercado == "1010")
                    {
                        oPedido.Cabecera.TipoDoc = strTipoDoc_BoutiqueEntrega;
                    }
                    else if (strMercado == "1020")
                    {
                        oPedido.Cabecera.TipoDoc = strTipoDoc_BoutiqueExteriorEntrega;
                    }
                }

                int intNumItems = 0;
                PedidoService ps = new PedidoService();
                List<Referencia> referencias = ps.GetCartItemsByNit(Nit);
                foreach (var referencia in referencias)
                {
                    foreach( Plu plu in referencia.Plu ){
                        if (plu.Cantidad > 0)
                        {
                            intNumItems++;
                        }
                    }
                }

                wsPedidos.PosicionesPed[] oDetalle = new wsPedidos.PosicionesPed[intNumItems];
                //Configura detalle del pedido
                int indPosicion = 0;
                foreach (var referencia in referencias)
                {
                    foreach (Plu plu in referencia.Plu)
                    {
                        if (plu.Cantidad > 0)
                        {
                            string strPlu = plu.Codigo;
                            decimal decCantidadPedido = plu.Cantidad;
                            decimal decPrecio = plu.Precio;
                            oDetalle[indPosicion] = new wsPedidos.PosicionesPed();
                            oDetalle[indPosicion].Tipo_posicion = dicParametros["TipoPosicion"];
                            oDetalle[indPosicion].PLU = strPlu;
                            oDetalle[indPosicion].Cantidad = decCantidadPedido;
                            oDetalle[indPosicion].Unidad = "";

                            oDetalle[indPosicion].Precio = decPrecio;
                            oDetalle[indPosicion].Observaciones = "";
                            indPosicion++;
                        }
                    }
                }
                oPedido.Posiciones = oDetalle;


                XmlSerializer SerializerObj = new XmlSerializer(typeof(wsPedidos.InfoPedidos));
                // Create a new file stream to write the serialized object to a file
                string path = dicParametros["Ruta_Log_Errores_PedidosBT"].ToString();
                TextWriter WriteFileStream = new StreamWriter(path.Replace("Errores_PedidosBT.log", +DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second + "-" + strCliente + ".xml"));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                SerializerObj.Serialize(WriteFileStream, oPedido);
                // Cleanup
                WriteFileStream.Close();
                //xmlPI = clsSerializeHelper.serializeSinNameSpaces(typeof(wsPedidos.InfoPedidos), oPedido);

                wsPedidos.ConfirmaPed oRespPedido = clsPedido.CreaPedido_Sync_Out(oPedido);

                if (oRespPedido.Cabecera.TipoPedido == null || oRespPedido.Cabecera.TipoPedido == "")
                {
                    
                    throw new Exception("No ha sido posible almacenar el pedido!");
                    //Boolean bolRespuestaLogError = this.generarRegistroError("Pedidos", oRespPedido.Mensajes[0].TextoMjs.ToString(), "Pedido");
                }

                string strNumeroPedido =oRespPedido.Cabecera.TipoPedido.ToString();
                return "Pedido Guardado: " + strNumeroPedido;
            }
            catch (Exception error)
            {
                
                //EnviarNotificacionError("PedidosBT", "", "XML Tienda Boutiques" + strDatosPedido + " XML PI: " + xmlPI, error.EstadoValidacion.strMensaje);
                //Boolean bolRespuestaLogError = this.generarRegistroError("Pedidos", error.EstadoValidacion.strMensaje, "XML Tienda Boutiques" + strDatosPedido + " XML PI: " + xmlPI);
                throw new Exception("Ha ocurrido un error al procesar el pedido");
            }
        }

        public IDataReader ConsultarParametrosAplicacion(string strConexion)
        {
            if (string.IsNullOrEmpty(strConexion))
                throw new ArgumentException("No es posible la conexión.", "strConexion");
            IDataReader oReader = null;
            try
            {
                
                var connection = new SqlConnection(strConexion);
                connection.Open();


                if (connection != null)
                {
                    using (connection)
                    {
                        SqlCommand oCmd = connection.CreateCommand();
                        oCmd.CommandText = "usp_HER_Generico_ObtenerParametrosAplicacion";
                        oCmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter param = oCmd.CreateParameter();
                        param.ParameterName = "@strAplicacion";
                        param.DbType = DbType.String;
                        param.Value = "'wsTiendaVirtual'"; 
                        oCmd.Parameters.Add(param);
                        oReader = oCmd.ExecuteReader();

                    }
                }
                return (oReader);
            }
            catch
            {
                throw;
            }
        }
    }
}
