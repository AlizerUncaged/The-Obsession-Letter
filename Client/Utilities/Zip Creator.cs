using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    public class Zip_Creator
    {
        private Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>();
        public void AddFile(byte[] content, string filename)
        {
            _files.Add(filename, content);
        }
        public void AddTextFile(string content, string filename)
        {
            AddFile(Encoding.UTF8.GetBytes(content), filename);
        }
        public byte[] CreateZip()
        {
            byte[] zipbytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    // add binary files
                    foreach (var binf in _files)
                    {
                        var zipEntry = zipArchive.CreateEntry(binf.Key, CompressionLevel.Optimal);

                        using (Stream entryStream = zipEntry.Open())
                        {
                            entryStream.Write(binf.Value, 0, binf.Value.Length);
                        }
                    }
                }
                zipbytes = memoryStream.ToArray();
            }
            return zipbytes;
        }
    }
}
