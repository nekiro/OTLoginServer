using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TibiaLoginServer.Classes
{
    public static class ConfigLoader
    {
        private static IDictionary<string, object> configs = new Dictionary<string, object>();

        public static int GetInteger(string name)
        {
            return configs[name] != null && configs[name] is int ? (int)configs[name] : 0;
        }

        public static string GetString(string name)
        {
            return configs[name] != null && configs[name] is string? configs[name] as string : "";
        }

        public static bool GetBoolean(string name)
        {
            return configs[name] != null && configs[name] is bool ? (bool)configs[name] : false;
        }

        public static bool ReadConfigFile()
        {
            try
            {
                configs.Clear();
                foreach (var line in File.ReadAllLines("config.lua"))
                {
                    if (line.StartsWith("--"))
                    {
                        continue;
                    }

                    string cleanedUpLine = Regex.Replace(line, @"\s+", "");
                    int indexOfSeparator = cleanedUpLine.IndexOf('=');
                    if (indexOfSeparator == -1)
                    {
                        continue;
                    }

                    string varName = cleanedUpLine.Substring(0, indexOfSeparator);
                    object value = cleanedUpLine.Substring(indexOfSeparator + 1);

                    try
                    {
                        if (value.ToString().IndexOf("\"") != -1)
                        {
                            // value must be a string
                            value = Regex.Match(value as string, "\"[^\"\\\\]*(?:\\\\.[^\"\\\\]*)*\"").Value.Replace("\"", "");
                            // ^ yeah regex XD
                        }
                        else
                        {
                            // might be bool or integer
                            string strValue = value as string;
                            if (strValue == "true" || strValue == "false")
                            {
                                value = bool.Parse(strValue);
                            }
                            else
                            {
                                value = int.Parse(strValue);
                            }
                        }

                        configs.Add(varName, value);
                    }
                    catch
                    {
                        // this will typically fail for special integers like 10 * 60 or so, we don't need these configs anyway
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse config.lua mysql info {ex}");
                return false;
            }
        }
    }
}
