using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OTLoginServer.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OTLoginServer.Classes
{
    public class WebService
    {
        private readonly HttpListener _listener;
        private readonly DatabaseManager _db;

        public WebService(DatabaseManager db)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            _listener = new HttpListener();
            Setup();

            _db = db;
        }

        private void Setup()
        {
            foreach (string prefix in Const.Prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }
        }

        public async Task<bool> Run()
        {
            try
            {
                _listener.Start();
            }
            catch (HttpListenerException ex)
            {
                switch (ex.ErrorCode)
                {
                    // access denied exception
                    case 5: Console.WriteLine("Access denied, run this application with administrator privileges."); break;
                    default: Console.WriteLine(ex.ToString()); break;
                }
                return false;
            }
            Console.WriteLine($"Began listening on {String.Join(",\n", Const.Prefixes)}");

            while (true)
            {
                HttpListenerContext ctx = await _listener.GetContextAsync();
                switch (ctx.Request.HttpMethod)
                {
                    case "POST":
                        {
                            var obj = JsonConvert.DeserializeObject<JObject>(new StreamReader(ctx.Request.InputStream).ReadToEnd());
                            await ProcessRequest(obj, ctx);
                            break;
                        }
                    default: Console.WriteLine("Unknown http method"); break;
                }
            }
        }

        private string CustomJsonSerialize(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new LowercaseContractResolver()
            };
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        public async Task ProcessRequest(JObject obj, HttpListenerContext ctx)
        {
            try
            {
                string type = obj.Value<string>("type");
                if (type == "login")
                {
                    string email = obj.Value<string>("email");
                    string password = obj.Value<string>("password");
                    string token = obj.Value<string>("token");
                    string time = DateTime.Now.AddMinutes(30).ToString();
                    Account account = await _db.GetAccount(email, password);
                    if (account == null)
                    {
                        await SendResponse(ctx, JsonConvert.SerializeObject(new ErrorResponse() { ErrorCode = 3, ErrorMessage = "Tibia account email address or Tibia password is not correct." }));
                        return;
                    }

                    LoginResponse loginResponse = new LoginResponse
                    {
                        Session = new Session
                        {
                            //SessionKey = $"{email}\n{password}",
                            SessionKey = $"{email}\n{password}\n{token}\n{time}",
                            IsPremium = account.IsPremium,
                            PremiumUntil = account.PremiumUntil
                        },

                        PlayData = new PlayData
                        {
                            Characters = await _db.GetAccountCharacters(account.Id),
                            Worlds = await Task.Run(() => _db.GetWorlds())
                        }
                    };
                    await SendResponse(ctx, CustomJsonSerialize(loginResponse));
                }
                else if (type == "boostedcreature")
                {
                    await SendResponse(ctx, CustomJsonSerialize(_db.GetBoostedCreature()));
                }
                else if (type == "cacheinfo")
                {
                    await SendResponse(ctx, CustomJsonSerialize(_db.GetCacheInfo()));
                }
                else if (type == "eventschedule")
                {
                    await SendResponse(ctx, CustomJsonSerialize(_db.GetScheduledEvents()));
                }
            }
            catch (Exception ex)
            {
                HttpListenerResponse resp = ctx.Response;
                resp.StatusCode = 400;
                resp.Close();
                Console.WriteLine("Received invalid request." + ex);
            }
        }

        public async Task SendResponse(HttpListenerContext ctx, string body)
        {
            HttpListenerResponse resp = ctx.Response;
            resp.ContentType = "application/json; charset=utf-8";
            resp.ContentEncoding = Encoding.UTF8;
            resp.StatusCode = 200;
            resp.KeepAlive = true;

            byte[] data = Encoding.UTF8.GetBytes(body);
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }
}
