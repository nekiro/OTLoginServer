using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TibiaLoginServer.Classes
{
    public class WebService
    {
        private readonly HttpListener _listener = null;
        public WebService()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            _listener = new HttpListener();
            Setup();
        }

        private void Setup()
        {
            foreach (string prefix in Const.Prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }
        }

        public async Task Run()
        {
            _listener.Start();
            Console.WriteLine($"Listening on {String.Join(",\n", Const.Prefixes)}");

            while (true)
            {
                HttpListenerContext ctx = await _listener.GetContextAsync();
                HttpListenerRequest req = ctx.Request;

                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                /*byte[] data = Encoding.UTF8.GetBytes("{}");
                HttpListenerResponse resp = ctx.Response;
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();*/
            }
        }
    }
}
