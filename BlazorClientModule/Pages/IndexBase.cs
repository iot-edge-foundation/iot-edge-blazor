using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BlazorEdgeModule.Edge;
using Microsoft.AspNetCore.Components;

namespace BlazorClientModule.Pages
{
    public class IndexBase : ComponentBase, IDisposable
    {
        [Inject]
        public IoTEdgeService EdgeService { get; set; }

        public string message;

        protected override void OnInitialized()
        {
            this.EdgeService.InputMessageReceived += OnInputMessageReceived;
        }

        void IDisposable.Dispose()
        {
            this.EdgeService.InputMessageReceived -= OnInputMessageReceived;
        }

        private async void OnInputMessageReceived(object sender, string messageString)
        {
            message = messageString;

            await InvokeAsync(() => StateHasChanged());
        }

        public async Task Send()
        {
            await EdgeService.SendMessage();
        }
    }
}