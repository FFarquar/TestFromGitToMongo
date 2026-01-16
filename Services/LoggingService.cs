using Microsoft.JSInterop;

namespace TestFromGitToMongo.Services
{
    public class LoggingService
    {
        private readonly IJSRuntime _jsRuntime;

        public LoggingService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task LogAsync(string category, string message)
        {
            await _jsRuntime.InvokeVoidAsync("console.log", $"[{category}] {message}");
        }

        public async Task LogInfoAsync(string category, string message)
        {
            await _jsRuntime.InvokeVoidAsync("console.info", $"%c[{category}] {message}", "color: blue;");
        }

        public async Task LogWarnAsync(string category, string message)
        {
            await _jsRuntime.InvokeVoidAsync("console.warn", $"[{category}] {message}");
        }

        public async Task LogErrorAsync(string category, string message)
        {
            await _jsRuntime.InvokeVoidAsync("console.error", $"[{category}] {message}");
        }

        public async Task LogGroupAsync(string category, string message)
        {
            await _jsRuntime.InvokeVoidAsync("console.group", $"[{category}] {message}");
        }

        public async Task LogGroupEndAsync()
        {
            await _jsRuntime.InvokeVoidAsync("console.groupEnd");
        }
    }
}
