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

        private int counter = 0;

        public IoTEdgeService()
        {
            Console.WriteLine("Starting IoTEdgeService singleton");

            Task.Run(() => this.Init()).Wait();
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

            await ioTHubModuleClient.SetInputMessageHandlerAsync("input1", PipeMessageInputOne, ioTHubModuleClient);

            Console.WriteLine("Input1 handler attached");
        }

        public event EventHandler<string> InputMessageReceived;

        private void OnInputMessageReceived(string messageString)
        {
            InputMessageReceived?.Invoke(this, messageString);
        }

        /// <summary>
        /// EXPERIMENTAL; WORK IN PROGRESS
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private async Task<MessageResponse> PipeMessageInputOne(Message message, object userContext)
        {
            var moduleClient = userContext as ModuleClient;
            if (moduleClient == null)
            {
                throw new InvalidOperationException("UserContext doesn't contain " + "expected values");
            }

            byte[] messageBytes = message.GetBytes();
            string messageString = Encoding.UTF8.GetString(messageBytes);

            Console.WriteLine($"-> Received echo message: '{messageString}'");

            if (!string.IsNullOrEmpty(messageString))
            {
                OnInputMessageReceived(messageString);

                //      await Task.Delay(TimeSpan.FromSeconds(0)); // added just to make this method awaitable
            }

            return MessageResponse.Completed;
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

    public class InputMessageEventArgs : EventArgs
    {
        public string MessageString { get; set; }
    }
}