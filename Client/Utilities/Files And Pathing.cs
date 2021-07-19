using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public static class Files_And_Pathing
    {
        public static void DeleteDirectory(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToLowerInvariant();
        }
        public static string GetTempExePath()
        {
            return Path.GetTempFileName() + ".exe";
        }

        public static string GetRandomSystem32Executable()
        {
            return GetRandomFileFromDir(Environment.SystemDirectory, new string[] { ".exe" });
        }
        /// <summary>
        /// Gets a random filename from a directory, not recursive
        /// </summary>
        /// <param name="dir">The directory name</param>
        /// <param name="extensions">Allowed extensions</param>
        public static string GetRandomFileFromDir(string dir, string[] extensions)
        {
            try
            {
                var di = new DirectoryInfo(dir);

                // hopefully this wont crash considering the number of
                // files on the directoru
                var rgFiles = di.GetFiles("*.*")
                         .Where(f => extensions.Contains(f.Extension
                                                           .ToLower())).ToArray();


                return Path.GetFileName(rgFiles[Constants.Rand.Next(rgFiles.Length)].FullName);

            }
            catch { }
            return null;
        }
    }
}
