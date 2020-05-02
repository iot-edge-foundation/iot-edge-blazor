using System;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorEdgeModule.Edge
{
    public class TimerService
    {
        private System.Timers.Timer aTimer;

        public TimerService()
        {
            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            OnInputMessageReceived("Message at " + DateTime.Now);
        }

        public event EventHandler<string> InputMessageReceived;

        private void OnInputMessageReceived(string messageString)
        {
            InputMessageReceived?.Invoke(this, messageString);
        }

        public async Task SendMessage()
        {
            Console.WriteLine("Sending Message");

            await Task.Delay(0);
        }
    }
}