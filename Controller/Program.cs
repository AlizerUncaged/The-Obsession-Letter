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
        /// Main entry point.
        /// </summary>
        static void Main(string[] args)
        {
            _inputthread = new Thread(ParseInput);

            _inputthread.Start();

            PrintBanner();

            var config = Configuration.LoadConfig();

            Utils.Logging.Write("Starting Controller Server...");

            Server.Server server = new Server.Server(config);

            server.Start();

        }
        private static void ParseInput()
        {
            while (true)
            {
                var inp = Console.ReadLine();
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
