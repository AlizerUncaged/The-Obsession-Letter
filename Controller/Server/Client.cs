using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller.Server
{
    public class Client
    {
        private Thread _readthread;

        private TcpClient _client;

        private NetworkStream _stream;

        private bool _keepreading = true;

        public IPEndPoint EndPoint;
        public Client(TcpClient client)
        {
            _client = client;

            EndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

            _stream = _client.GetStream();
        }

        public void WriteCMD(string cmd)
        {
            List<byte> bytes = new List<byte>() { (byte)Command_Types.CMDCommand };

            bytes.AddRange(Encoding.ASCII.GetBytes(cmd));

            Write(bytes.ToArray());
        }

        public void Write(byte[] bytes)
        {
            _stream.Write(bytes);

            _stream.Flush();
        }

        public void StartRead()
        {
            _readthread = new Thread(Read);

            _readthread.Start();
        }

        public void StopRead()
        {
            _keepreading = false;
        }
        private void Read()
        {
            while (_keepreading && _client.Connected)
            {
                while (!_stream.DataAvailable);

                byte[] receivedBuffer = new byte[_client.Available];

                int bytesreceived = _stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                if (bytesreceived > 0)
                {
                    var messagetype = (Message_Types)receivedBuffer.First();

                    receivedBuffer = receivedBuffer.Skip(1).ToArray();

                    switch (messagetype)
                    {
                        case Message_Types.CMDMessage:
                            string s = Encoding.ASCII.GetString(receivedBuffer);

                            if (!string.IsNullOrWhiteSpace(s) && Program.ActiveClient == this)
                                Utils.Logging.Write(Utils.Logging.Type.Received, $"{s}");

                            break;
                        case Message_Types.Exited:
                            
                            OnCMDExited(EventArgs.Empty);

                            break;
                    }
                }

            }
        }

        public event EventHandler CMDExited;
        protected virtual void OnCMDExited(EventArgs e)
        {
            CMDExited?.Invoke(this, e);
        }
    }
}
