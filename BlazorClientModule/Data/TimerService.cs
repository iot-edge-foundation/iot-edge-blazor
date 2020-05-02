using System;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorEdgeModule.Edge
{
    public class TimerService
    {
        private Timer timer;

        public TimerService()
        {
            timer = new Timer(5000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
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