using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Data;
using System.ServiceModel.Description;
using Hermeco.Multimarcas.Services.Proxy;

namespace Hermeco.Multimarcas.Services
{
    public abstract class HMService : IDisposable
    {

        private DataSet dsResultQuery;
        private string resultMessage = String.Empty;

        internal IAviableReports aviableReports;
        public string objectName;
        public Dictionary<string, string> queryParameters = null;

       
        public HMService()
        {
            queryParameters = new Dictionary<string, string>();
        }

        public DataSet RequestResult()
        {
            ChannelFactory<IAviableReports> channelFactory = HMChannelFactory.Instance;
            aviableReports = channelFactory.CreateChannel();
            if (aviableReports == null) {
                throw new Exception("It was not possible to connect to the Application Server, please contact your system administrator");
            }
            return RunService(objectName, queryParameters);
        }

        public DataSet RequestResult(string ObjectName, Dictionary<string, string> QueryParameters)
        {
            objectName = ObjectName;
            queryParameters = QueryParameters;
            return RequestResult();
        }

        private DataSet RunService(string objectName, Dictionary<string, string> QueryParameters)
        {
            try
            {
                var hresult = aviableReports.GetData(objectName, QueryParameters);
                dsResultQuery =  Utility.DecompressedData(hresult);
                hresult = null;
                GC.Collect();
            } catch (Exception ex) {
                return null;
            }
            return dsResultQuery;
        }

        public void Dispose()
        {
            if (dsResultQuery != null)
            {
                dsResultQuery.Dispose();
                dsResultQuery = null;
            }

            if (queryParameters != null)
            {
                queryParameters = null;
            }
        }
    }
}
