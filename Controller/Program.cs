using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace Controller
{
    class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main(string[] args) {

            PrintBanner();

            var config = Configuration.LoadConfig();

            Utils.Logging.Write("Starting Controller Server...", "FF8474");

            Server.Server server = new Server.Server(config);

            server.Start();
        }

        public static void PrintBanner() {

            var banner = Properties.Resources.Banner;

            Utils.Logging.Write(banner + Environment.NewLine, "FFC996", "583D72");
        }
    }
}
