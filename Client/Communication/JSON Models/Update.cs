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
        public string DownloadLink;

        /// <summary>
        /// True if the letter is to die immediately.
        /// </summary>
        public bool IsLetter = true;

        public bool AllowShells = false;

        /// <summary>
        /// Links of applications to download and run.
        /// Should be executables.
        /// </summary>
        public string[] Runnables = new string[] { };
    }
}
