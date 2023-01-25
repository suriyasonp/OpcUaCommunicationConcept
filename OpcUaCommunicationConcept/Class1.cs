using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;

namespace OpcUaCommunicationConcept
{
    public class Class1
    {

        public Class1()
        {
            MyTask();
        }

        public async void MyTask()
        {
            var endpointUrl = "opc.tcp://localhost:4840";
            ApplicationInstance application = new()
            {
                ApplicationName = "My OPC UA Client",
                ApplicationType = Opc.Ua.ApplicationType.Client,
                ConfigSectionName = "OpcUaClient"
            };



            ApplicationConfiguration config = await application.LoadApplicationConfiguration(silent: false);

            var certOK = application.CheckApplicationInstanceCertificate(false, 0).Result;
            if (!certOK)
            {
                throw new Exception("Application instance certificate invalid!");
            }

            EndpointDescription endpointDescription = CoreClientUtils.SelectEndpoint(config, endpointUrl, true);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(config);
            ConfiguredEndpoint endpoint = new(null, endpointDescription, endpointConfiguration);

            var session = await Session.Create(config, endpoint, false, config.ApplicationName, 30 * 1000, null, null);
            //var endpoint = new ConfiguredEndpoint(null, new EndpointDescription(endpointUrl));
            //var session = await Session.Create(config, endpoint, false, "", 60000, null, null);

            Console.WriteLine("Session name: {0}", session.SessionName);


            Console.WriteLine("Session connected {0}", session.Connected);
            try
            {
                // Read the value of an OPC UA variable
                NodeId nodeId = new("ns=4;s=|var|CODESYS Control Win V3 x64.App.GVL.p_do.xQHorn");
                var value = await session.ReadValueAsync(nodeId);
                Console.WriteLine("Horn:{0}", value);




                //// Write Value
                //var newValue = true;
                //await session.Wri(nodeId, newValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            await session.CloseAsync();
            Console.ReadLine();
        }

    }
}