using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage.Watchers.Filesystem
{
    /// <summary>
    /// A class that surveys the entire filesystem, checks every file for something important and uploads them.
    /// </summary>
    public class Surveyor
    {
        private string _root_folder;

        public Surveyor(string drive_letter) {
            _root_folder = drive_letter;
        }
    }
}
