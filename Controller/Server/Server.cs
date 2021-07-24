using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Controller.Server
{
    public class Server
    {
        public List<Client> Clients = new List<Client>();

        private Thread _readthread;

        private Configuration _conf;

        public Server(Configuration serverconf)
        {

            _conf = serverconf;
        }
        public bool Start()
        {

            _readthread = new Thread(ContinuousReading);

            _readthread.Start();

            return true;
        }
        public void ContinuousReading()
        {
            TcpListener listener = null;
            try
            {
                // attempt to bind 
                IPAddress localAdd = IPAddress.Parse(_conf.Interface);

                listener = new TcpListener(localAdd, _conf.Port);

                listener.Start();
            }
            catch (Exception ex)
            {
                Utils.Logging.Write(Utils.Logging.Type.Error, $"An error occured while binding");

                return;
            }
            finally
            {
                Utils.Logging.Write(Utils.Logging.Type.Success, $"Server listening on {_conf.Interface}:{_conf.Port}");

                ConnectedSuccessfully(EventArgs.Empty);

                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();

                        Client parsed_client = new Client(client);

                        Utils.Logging.Write(Utils.Logging.Type.Success, $"New Shell! at {parsed_client.EndPoint.Address}:{parsed_client.EndPoint.Port}");

                        parsed_client.StartRead();

                        Clients.Add(parsed_client);
                    }
                    catch (SocketException ex)
                    {
                        // i dont get any of these
                    }
                    catch
                    {
                        // prevent from getting CPU to 100 when something bad happens
                        Thread.Sleep(100);
                    }
                }
            }
        }
        public void RemoveClient(ref Client client) {

            if (Clients.Contains(client)) {

                Clients.Remove(client);

                Utils.Logging.Write(Utils.Logging.Type.Success, $"Removed {_conf.Interface}:{_conf.Port}");
            }
        }

        public event EventHandler SuccessfullyConnected;
        protected virtual void ConnectedSuccessfully(EventArgs e)
        {
            SuccessfullyConnected?.Invoke(this, e);
        }
    }
}
