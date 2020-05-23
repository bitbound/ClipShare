using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Client.Services
{
    public interface IClipboardService
    {
        Task<string> GetClipboardText();
        Task SetClipboardText(string text);
    }

    public class ClipboardService : IClipboardService
    {
        public ClipboardService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        private IJSRuntime JSRuntime { get; }

        public async Task<string> GetClipboardText()
        {
            return await JSRuntime.InvokeAsync<string>("getClipboard");
        }

        public async Task SetClipboardText(string text)
        {
            await JSRuntime.InvokeAsync<string>("setClipboard", text);
        }
    }
}
