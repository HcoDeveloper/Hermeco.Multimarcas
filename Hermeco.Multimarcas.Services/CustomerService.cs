using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Hermeco.Multimarcas.Services
{
    public class CustomerService : HMService
    {
        public DataSet ObtenerCarteraCustomerService(string Nit){
            this.queryParameters.Clear();
            this.objectName = "ObtenerCarteraCustomerService";
            this.queryParameters.Add("@kunnr", Nit);
            return this.RequestResult();
        }
        public DataSet ObtenerClienteCustomerService(string Nit)
        {
            this.queryParameters.Clear();
            this.objectName = "ObtenerClienteCustomerService";
            this.queryParameters.Add("@identificacion", Nit);
            return this.RequestResult();
        }
        public DataSet ObtenerInventarioPluCustomerService(string Plu)
        {
            this.queryParameters.Clear();
            this.objectName = "ObtenerInventarioPluCustomerService";
            this.queryParameters.Add("@plus", Plu);
            return this.RequestResult();
        }
        public DataSet ObtenerMaterialCustomerService(string Material )
        {
            this.queryParameters.Clear();
            this.objectName = "ObtenerMaterialCustomerService";
            this.queryParameters.Add("@matnr", Material);                  
            return this.RequestResult();
        }

    }
}
