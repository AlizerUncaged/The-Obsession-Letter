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

        public void Write(string cmd)
        {
            Write(Encoding.Default.GetBytes(cmd));
        }

        public void Write(byte[] bytes)
        {
            _stream.Write(bytes);
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
                while (!_stream.DataAvailable) ;

                byte[] receivedBuffer = new byte[_client.Available];

                _stream.Read(receivedBuffer, 0, receivedBuffer.Length);
#if DEBUG
                string s = Encoding.ASCII.GetString(receivedBuffer);

                if (!string.IsNullOrWhiteSpace(s))

                Utils.Logging.Write(Utils.Logging.Type.Received, $"{s}");
#endif
            }
        }
    }
}
