using System;
using System.Threading.Tasks;
using TibiaLoginServer.Classes;

namespace TibiaLoginServer
{
    class MainClass
    {
        public static async Task Main(string[] args)
        {
            WebService webService = new WebService();
            await webService.Run();
        }
    }
}
