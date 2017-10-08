using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RSA_ChatServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //Uri tcpEndpointUri = new Uri("net.tcp://localhost:9999/Services/ChatService");
            //Uri httpEndpointUri = new Uri("http://localhost:9999/Services/ChatService");

            Uri[] endPointsUris = {
                 new Uri("net.tcp://localhost:9999/Services/ChatService")
                //,new Uri("http://localhost:9999/Services/ChatService")
            };


            ServiceHost host = new ServiceHost(typeof(RSA_ChatService.Chat), endPointsUris);
            //host.AddServiceEndpoint(typeof(RSA_ChatService.interfaces.IChat), new WSHttpBinding(), "");
            host.AddServiceEndpoint(typeof(RSA_ChatService.interfaces.IChat), new NetTcpBinding(), "");

            //host.AddServiceEndpoint(typeof(IMetadataExchange), new NetTcpBinding(), $"mex");

            var smb = new ServiceMetadataBehavior();
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            
            
          //  smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                                    MetadataExchangeBindings.CreateMexTcpBinding(), 
                                    "mex");
            try
            {

                host.Open();
                Console.WriteLine($"Service started! {DateTime.Now.ToString()}");
                Console.WriteLine($"Press any key to stop");
                Console.ReadKey();
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ERROR: {exc.Message}");
                Console.WriteLine($"Press any key to stop");
                Console.ReadKey();
            }
        }
    }
}
