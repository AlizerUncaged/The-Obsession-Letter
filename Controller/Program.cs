using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Console = Colorful.Console;

namespace Controller
{
    class Program
    {
        private static Thread _inputthread;

        /// <summary>
        /// The index of the current client being in control.
        /// </summary>
        private static Server.Client _activeclient;

        private static Server.Server _server;
        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main(string[] args)
        {
            PrintBanner();

            var config = Configuration.LoadConfig();

            Utils.Logging.Write("Starting Controller Server...");

            _server = new Server.Server(config);

            _server.SuccessfullyConnected += _server_SuccessfullyConnected;

            _server.Start();
        }

        private static void _server_SuccessfullyConnected(object sender, EventArgs e)
        {
            _inputthread = new Thread(ParseInput);

            _inputthread.Start();
        }

        private static void ParseInput()
        {
            Console.ResetColor();

            Console.Write("[!] To open help do ", Utils.Logging.GetTypeColor(Utils.Logging.Type.Normal));

            Console.WriteLine($"> help", Utils.Logging.GetTypeColor(Utils.Logging.Type.Success));

            Console.ForegroundColor = Utils.Logging.GetTypeColor(Utils.Logging.Type.Success);

            while (true)
            {
                Console.Write("> ", Utils.Logging.GetTypeColor(Utils.Logging.Type.Error));

                var input = Console.ReadLine();

                Console.WriteLine(string.Empty);

                while (_activeclient != null && _server.Clients.Contains(_activeclient)) { 
                
                }
            }
        }
        public static void PrintBanner()
        {

            var banner = Properties.Resources.Banner;

            Utils.Logging.Write(banner + Environment.NewLine, "FF8474", "583D72");
            Utils.Logging.Write($"{Environment.NewLine}    >>>[Github] https://github.com/AlizerUncaged/The-Love-Letter {String.Concat(Enumerable.Repeat(Environment.NewLine, 2))}", "FF8474", "583D72");
        }
    }
}
