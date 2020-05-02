using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorEdgeModule.Edge
{
    public class CounterService
    {
        private System.Timers.Timer aTimer;

        public CounterService()
        {
            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            await OnInputMessageReceived("Message at " + DateTime.Now);
        }

        public event EventHandler<string> InputMessageReceived;

        private async Task OnInputMessageReceived(string messageString)
        {
            await Task.Run(() => { InputMessageReceived?.Invoke(this, messageString); });
        }

        public async Task SendMessage()
        {
            Console.WriteLine("Sending Message");

            await Task.Delay(0);
        }
    }
}