//based off of https://github.com/Nekos-life/Nekos-Sharp/blob/master/NekosSharp/NekosClient.cs, but without the specific stuff


using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace KawaiiBot2.APIInterfacing
{
    class Client
    {
        public Client(string BotName)
        {
            client.DefaultRequestHeaders.Add("User-Agent", $"{BotName}");
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
                    Console.WriteLine($"[NekosSharp] Failed, {Request.ErrorMessage} {Request.ErrorCode}");
                if (LogType == LogType.Debug)
                    Console.WriteLine("[NekosSharp] Exception\n" + ex.ToString());
            }
            return Request;
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

