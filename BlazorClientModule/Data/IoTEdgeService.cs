using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;

namespace BlazorEdgeModule.Edge
{
    public class IoTEdgeService
    {
        private ModuleClient ioTHubModuleClient;

        private int counter = int.MinValue;

        public IoTEdgeService()
        {
            Console.WriteLine("Starting IoTEdgeService singleton");

            Init().Wait();
        }

        private async Task Init()
        {
            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            ITransportSettings[] settings = { mqttSetting };

            Console.WriteLine("Create IoTEdgeService...");

            // Open a connection to the Edge runtime
            ioTHubModuleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);

            Console.WriteLine("Open IoTEdgeService...");

            await ioTHubModuleClient.OpenAsync();

            Console.WriteLine("IoTEdgeService intialized.");
        }

        public async Task SendMessage()
        {
            Console.WriteLine("Sending Message");

            counter++;

            var messageBody = new MessageBody
            {
                counter = counter,
                timeStamp = DateTime.UtcNow,
            };

            var json = JsonConvert.SerializeObject(messageBody);

            using (var message = new Message(Encoding.UTF8.GetBytes(json)))
            {
                await ioTHubModuleClient.SendEventAsync("output1", message);

                Console.WriteLine("Message sent");
            }
        }
    }

    public class MessageBody
    {
        public int counter { get; set; }

        public DateTime timeStamp { get; set; }
    }
}