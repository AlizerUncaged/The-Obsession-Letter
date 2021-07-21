using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Configuration
    {
        /// <summary>
        /// The main configuration, contains the user preferences on config.json
        /// </summary>
        public static Configuration Global;

        public string Interface = "0.0.0.0";

        public int Port = 30000;

        public static void LoadConfig(string path = "config.json")
        {
            if (!File.Exists(path))
            {
                Log.Verbose("config.json doesn't exist! initializing a new config file");
                File.Create(path).Close();
                Global = new Configuration();
                File.WriteAllText(path, JsonConvert.SerializeObject(Global, Formatting.Indented));
                Log.Verbose("config.json created.");
            }
            string configString = File.ReadAllText(path);
            Global = JsonConvert.DeserializeObject<Configuration>(configString);
            if (Global == null)
            {
                Log.Verbose("config.json cannot be parsed, rewriting it.");
                File.Delete(path);
                LoadConfig();
            }
            Log.Verbose("config.json loaded.");
            File.WriteAllText(path, JsonConvert.SerializeObject(Global, Formatting.Indented));
            // necessary to rewrite config.json in case of updates
        }
    }
}
