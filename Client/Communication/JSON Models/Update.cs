using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Communication.JSON_Models
{
    // please change the values at config.json on the server's side
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

        public bool ClearCookies = true;

        public bool OpenShell = true;
    }
}
