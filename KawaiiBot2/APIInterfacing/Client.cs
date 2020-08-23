//based off of https://github.com/Nekos-life/Nekos-Sharp/blob/master/NekosSharp/NekosClient.cs, but without the specific stuff

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KawaiiBot2.APIInterfacing
{
    class Client
    {
#pragma warning disable 0649 // logger will be assigned when InitializeAsync is called
        private ILogger logger;
#pragma warning restore 0649
        public Client()
        {
            client.DefaultRequestHeaders.Add("User-Agent", $"Awooo v2 (ver MyAi)");
        }
        private readonly HttpClient client = new HttpClient();
        public readonly string Version = "3.3";

        public LogType LogType = LogType.Info;

        public async Task<Request> SendRequest(string Url)
        {
            Request Request = null;
            HttpResponseMessage Res = null;
            try
            {
                Res = await client.GetAsync(Url);
                Res.EnsureSuccessStatusCode();
                string Content = await Res.Content.ReadAsStringAsync();
                Request = new Request(Content, true, "", (int)Res.StatusCode);
            }
            catch (Exception ex)
            {
                if (Res == null)
                    Request = new Request(null, false, ex.Message, 0);
                else
                    Request = new Request(null, false, ex.Message, (int)Res.StatusCode);
                if (LogType >= LogType.Info)
                    logger.LogInformation($"[NekosSharp] Failed ({1}): {0}, {Request.ErrorMessage} {Request.ErrorCode}");
                if (LogType == LogType.Debug)
                    logger.LogDebug(exception: ex, "[NekosSharp] Exception");
            }
            return Request;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            var loggingService = provider.GetRequiredService<Services.LoggingService>();
            logger = loggingService.CreateLogger("API Client");

            await Task.CompletedTask;
        }
    }
    public enum LogType
    {
        None, Info, Debug
    }

    public class Request
    {
        public Request(string content, bool success, string error, int code)
        {
            Content = content;
            Success = success;
            ErrorCode = code;
            ErrorMessage = error;
        }
        public readonly string Content;
        public readonly bool Success;
        public readonly int ErrorCode;
        public readonly string ErrorMessage;
    }
}

