using System;
using System.Security.Cryptography;
using System.Text;

namespace OTLoginServer.Classes
{
    public static class Const
    {
        public static string[] Prefixes = new string[] { "http://localhost:80/login/" }; // server will listen for incoming login request at this url

        public static string Sha1Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash))).Replace("-", "").ToLower();
            }
        }
    }
}
