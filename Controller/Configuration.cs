using Newtonsoft.Json;
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
        public string Interface = "0.0.0.0";

        public int Port = 30000;

        public bool DiscordAPIEnabled = false;

        public string DiscordAPIBotToken = "";

        public static Configuration LoadConfig(string path = "config.json")
        {
            if (!File.Exists(path))
            {
                Utils.Logging.Write("config.json doesn't exist! initializing a new config file, you may modify this to your liking");

                File.Create(path).Close();

                // init a new conf because config.json doesnt exit
                File.WriteAllText(path, JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));
            }

            string configString = File.ReadAllText(path);

            var Global = JsonConvert.DeserializeObject<Configuration>(configString);

            if (Global == null)
            {
                Utils.Logging.Write("config.json cannot be parsed, rewriting it.");

                File.Delete(path);

                LoadConfig();
            }

            Utils.Logging.Write("config.json loaded.");

            // necessary to rewrite config.json in case of updates
            File.WriteAllText(path, JsonConvert.SerializeObject(Global, Formatting.Indented));

            return Global;
        }
    }
}
