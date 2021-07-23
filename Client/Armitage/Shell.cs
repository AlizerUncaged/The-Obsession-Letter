using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private static Thread _reader;
        /// <summary>
        /// Starts listening for commands from the controller.
        /// </summary>
        public static void Start() {
            // start reading from remote endpoint

            IPAddress ip = IPAddress.Parse("127.0.0.1");
        }
        public static void ContinuousReading() {
            while (true) {
                try
                {

                }
                catch (SocketException ex) { 
                
                }
                catch
                {
                    // prevent from getting CPU to 100 when something bad happens
                    Thread.Sleep(100);
                }
            }
        }
    }
}
