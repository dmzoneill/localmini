using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;


namespace LocalMiniTrayApp
{
    public partial class recipientbox : Form
    {

        public recipientbox()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AutoCompleteStringCollection data = new AutoCompleteStringCollection();

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\emailcontacts", true);

            foreach (string value in localmini.GetValueNames())
            {
                data.Add(value.ToString());
            }
            
            emailaddress.AutoCompleteCustomSource = data;

            localmini.Close();
        }

        public string recipientaddy
        {
            get { return emailaddress.Text; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            this.Activate();
        }

    }
}
