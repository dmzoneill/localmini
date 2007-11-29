using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LocalMiniTrayApp
{

    public partial class hotspot : Form
    {
        #region Form Dragging API Support
        //The SendMessage function sends a message to a window or windows.

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]

        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        //ReleaseCapture releases a mouse capture

        [DllImportAttribute("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]

        public static extern bool ReleaseCapture();

        #endregion

        public hotspot()
        {
            InitializeComponent();
        }

        private void hotspot_Load(object sender, EventArgs e)
        {

        }

        public void setLabel(string msg)
        {
            label1.Text = msg;
        }

        private void hotspot_MouseDown(object sender, MouseEventArgs e)
        {
            // drag the form without the caption bar

            // present on left mouse button

            if (e.Button == MouseButtons.Left)
            {

                ReleaseCapture();

                SendMessage(this.Handle, 0xa1, 0x2, 0);

            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            // drag the form without the caption bar

            // present on left mouse button

            if (e.Button == MouseButtons.Left)
            {

                ReleaseCapture();

                SendMessage(this.Handle, 0xa1, 0x2, 0);

            }
        }

        public string getCurrent
        {
            get { return label1.Text; }
        }

    }
}
