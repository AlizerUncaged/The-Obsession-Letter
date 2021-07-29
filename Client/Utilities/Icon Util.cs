using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Utilities
{
    public static class Icon_Util
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleIcon(IntPtr hIcon);


        [UnmanagedFunctionPointer(CallingConvention.Winapi, SetLastError = true, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]

        public delegate bool ENUMRESNAMEPROC(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool EnumResourceNames(IntPtr hModule, IntPtr lpszType, ENUMRESNAMEPROC lpEnumFunc, IntPtr lParam);


        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;
        private readonly static IntPtr RT_ICON = (IntPtr)3;
        private readonly static IntPtr RT_GROUP_ICON = (IntPtr)14;

        public static System.Drawing.Icon ExtractIconFromExecutable(string path)
        {
            IntPtr hModule = LoadLibraryEx(path, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
            var tmpData = new List<byte[]>();

            ENUMRESNAMEPROC callback = (h, t, name, l) =>
            {
                var dir = GetDataFromResource(hModule, RT_GROUP_ICON, name);

                // Calculate the size of an entire .icon file.

                int count = BitConverter.ToUInt16(dir, 4);  // GRPICONDIR.idCount
                int len = 6 + 16 * count;                   // sizeof(ICONDIR) + sizeof(ICONDIRENTRY) * count
                for (int i = 0; i < count; ++i)
                    len += BitConverter.ToInt32(dir, 6 + 14 * i + 8);   // GRPICONDIRENTRY.dwBytesInRes

                using (var dst = new BinaryWriter(new MemoryStream(len)))
                {
                    // Copy GRPICONDIR to ICONDIR.

                    dst.Write(dir, 0, 6);

                    int picOffset = 6 + 16 * count; // sizeof(ICONDIR) + sizeof(ICONDIRENTRY) * count

                    for (int i = 0; i < count; ++i)
                    {
                        // Load the picture.

                        ushort id = BitConverter.ToUInt16(dir, 6 + 14 * i + 12);    // GRPICONDIRENTRY.nID
                        var pic = GetDataFromResource(hModule, RT_ICON, (IntPtr)id);

                        // Copy GRPICONDIRENTRY to ICONDIRENTRY.

                        dst.Seek(6 + 16 * i, 0);

                        dst.Write(dir, 6 + 14 * i, 8);  // First 8bytes are identical.
                        dst.Write(pic.Length);          // ICONDIRENTRY.dwBytesInRes
                        dst.Write(picOffset);           // ICONDIRENTRY.dwImageOffset

                        // Copy a picture.

                        dst.Seek(picOffset, 0);
                        dst.Write(pic, 0, pic.Length);

                        picOffset += pic.Length;
                    }

                    tmpData.Add(((MemoryStream)dst.BaseStream).ToArray());
                }
                return true;
            };
            EnumResourceNames(hModule, RT_GROUP_ICON, callback, IntPtr.Zero);

            byte[][] iconData = tmpData.ToArray();
            if (iconData.Count() > 0)
            {
                using (var ms = new MemoryStream(iconData[Constants.Rand.Next(iconData.Length)]))
                {
                    return new System.Drawing.Icon(ms);
                }
            }
            else {
                return ExtractIconFromExecutable(Constants.System32Dir + "/SHELL32.dll");
            }
        }
        public static byte[] GetDataFromResource(IntPtr hModule, IntPtr type, IntPtr name)
        {
            // Load the binary data from the specified resource.

            IntPtr hResInfo = FindResource(hModule, name, type);

            IntPtr hResData = LoadResource(hModule, hResInfo);

            IntPtr pResData = LockResource(hResData);

            uint size = SizeofResource(hModule, hResInfo);

            byte[] buf = new byte[size];
            Marshal.Copy(pResData, buf, 0, buf.Length);

            return buf;
        }


        #region IconReader
        public class Icons : List<Icon>
        {
            public byte[] ToGroupData(int startindex = 1)
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    var i = 0;

                    writer.Write((ushort)0);  //reserved, must be 0
                    writer.Write((ushort)1);  // type is 1 for icons
                    writer.Write((ushort)this.Count);  // number of icons in structure(1)

                    foreach (var icon in this)
                    {

                        writer.Write(icon.Width);
                        writer.Write(icon.Height);
                        writer.Write(icon.Colors);
                        writer.Write((byte)0); // reserved, must be 0
                        writer.Write(icon.ColorPlanes);

                        writer.Write(icon.BitsPerPixel);

                        writer.Write(icon.Size);

                        writer.Write((ushort)(startindex + i));

                        i++;

                    }
                    ms.Position = 0;

                    return ms.ToArray();
                }
            }
        }

        public class Icon
        {

            public byte Width { get; set; }
            public byte Height { get; set; }
            public byte Colors { get; set; }

            public uint Size { get; set; }

            public uint Offset { get; set; }

            public ushort ColorPlanes { get; set; }

            public ushort BitsPerPixel { get; set; }

            public byte[] Data { get; set; }

        }

        public class IconReader
        {

            public Icons Icons = new Icons();

            public IconReader(Stream input)
            {
                using (BinaryReader reader = new BinaryReader(input))
                {
                    reader.ReadUInt16(); // ignore. Should be 0
                    var type = reader.ReadUInt16();
                    if (type != 1)
                    {
                        //throw new Exception("Invalid type. The stream is not an icon file");
                    }
                    var num_of_images = reader.ReadUInt16();

                    for (var i = 0; i < num_of_images; i++)
                    {
                        var width = reader.ReadByte();
                        var height = reader.ReadByte();
                        var colors = reader.ReadByte();
                        reader.ReadByte(); // ignore. Should be 0

                        var color_planes = reader.ReadUInt16(); // should be 0 or 1

                        var bits_per_pixel = reader.ReadUInt16();

                        var size = reader.ReadUInt32();

                        var offset = reader.ReadUInt32();

                        this.Icons.Add(new Icon()
                        {
                            Colors = colors,
                            Height = height,
                            Width = width,
                            Offset = offset,
                            Size = size,
                            ColorPlanes = color_planes,
                            BitsPerPixel = bits_per_pixel
                        });
                    }

                    // now get the Data
                    foreach (var icon in Icons)
                    {
                        if (reader.BaseStream.Position < icon.Offset)
                        {
                            var dummy_bytes_to_read = (int)(icon.Offset - reader.BaseStream.Position);
                            reader.ReadBytes(dummy_bytes_to_read);
                        }

                        var data = reader.ReadBytes((int)icon.Size);

                        icon.Data = data;
                    }

                }
            }

        }
        #endregion

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int UpdateResource(IntPtr hUpdate, uint lpType, ushort lpName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr BeginUpdateResource(string pFileName, [MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        public enum ICResult
        {
            Success,
            FailBegin,
            FailUpdate,
            FailEnd
        }


        public static ICResult ChangeIcon(string exeFilePath, string iconFilePath)
        {
            using (FileStream fs = new FileStream(iconFilePath, FileMode.Open, FileAccess.Read))
            {
                var reader = new IconReader(fs);

                return ChangeIcon(exeFilePath, reader.Icons);
            }
        }

        public static ICResult ChangeIcon(string exeFilePath, System.Drawing.Icon icon)
        {
            byte[] iconbyte;
            using (MemoryStream ms = new MemoryStream())
            {
                icon.Save(ms);
                iconbyte = ms.ToArray();
            }
            using (MemoryStream fs = new MemoryStream(iconbyte, true))
            {
                var reader = new IconReader(fs);

                return ChangeIcon(exeFilePath, reader.Icons);
            }
        }
        public static ICResult ChangeIcon(string exeFilePath, Icons icons)
        {

            // Load executable
            IntPtr handleExe = BeginUpdateResource(exeFilePath, false);

            if (handleExe == null) return ICResult.FailBegin;

            ushort startindex = 1;
            ushort index = startindex;
            ICResult result = ICResult.Success;

            var ret = 1;

            foreach (var icon in icons)
            {
                // Replace the icon
                // todo :Improve the return value handling of UpdateResource
                ret = UpdateResource(handleExe, (uint)RT_ICON, index, 0, icon.Data, icon.Size);

                index++;
            }

            var groupdata = icons.ToGroupData();

            // todo :Improve the return value handling of UpdateResource
            ret = UpdateResource(handleExe, (uint)RT_GROUP_ICON, startindex, 0, groupdata, (uint)groupdata.Length);
            if (ret == 1)
            {
                if (EndUpdateResource(handleExe, false))
                    result = ICResult.Success;
                else
                    result = ICResult.FailEnd;
            }
            else
                result = ICResult.FailUpdate;
            return result;
        }
    }
}
