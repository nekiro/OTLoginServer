using OTLoginServer.Classes;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OTLoginServer
{
    class Program
    {
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static async Task Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !IsAdministrator())
            {
                Console.WriteLine("You need to run this program as administrator, because http listener requires it.");
                Console.ReadKey();
                return;
            }

            if (!ConfigLoader.ReadConfigFile())
            {
                Console.ReadKey();
                return;
            }

            DatabaseManager dbManager = new DatabaseManager();
            bool success = await dbManager.Setup();
            if (!success)
            {
                Console.ReadKey();
                return;
            }

            WebService webService = new WebService(dbManager);
            await webService.Run();
            Console.ReadKey();
        }
    }
}
