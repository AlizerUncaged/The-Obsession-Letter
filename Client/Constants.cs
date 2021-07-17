using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class Constants
    {
        public static string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string MMCFolder = System.IO.Path.GetFullPath(AppDataFolder + @"\Microsoft\MMC");

        /// <summary>
        /// Target location to victim PC
        /// </summary>
        public static string MMCFile = System.IO.Path.GetFullPath(MMCFolder + @"\SessionManager.exe");
    }
}
