using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Hermeco.Multimarcas.Services.Proxy;

namespace Hermeco.Multimarcas.Services
{
    public class HMChannelFactory
    {
        private static ChannelFactory<IAviableReports> channelFactory = null;
        private static readonly object olock = new object();

        public static ChannelFactory<IAviableReports> Instance
        {
            get
            {
                lock (olock)
                {
                    if (channelFactory == null)
                    {
                        EndpointAddress Serviceaddress = new EndpointAddress("http://hdbappsvr.hermeco.com:8080/IPCService");
                        /// Set behaviour of binding Same setting as ##Server##
                        BasicHttpBinding httpBinding = new BasicHttpBinding("BasicHttpBinding_IAviableReports");
                        httpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                        httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

                        channelFactory = new ChannelFactory<IAviableReports>(httpBinding, Serviceaddress);
                        var defaultCredentials = channelFactory.Endpoint.Behaviors.Find<ClientCredentials>();

                        channelFactory.Credentials.UserName.UserName = "HDBSERVICE";
                        channelFactory.Credentials.UserName.Password = "66d83ed49068abab7d83ae9286fbe2ba";
                    }
                    return channelFactory;
                }
            }
        }
    }
}
