using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Console = Colorful.Console;

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

            while (true) {
                if (server.Clients.Count > 0)
                {
                    string inp = Console.ReadLine();

                    server.Clients.FirstOrDefault().Write(inp);
                }
                Thread.Sleep(100);
            }
        }

        public static void PrintBanner() {

            var banner = Properties.Resources.Banner;

            Utils.Logging.Write(banner + Environment.NewLine, "FFC996", "583D72");
        }
    }
}
