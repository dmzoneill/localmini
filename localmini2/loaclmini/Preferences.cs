using System;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Net.Mail;
using System.Net.Mime;
using System.Drawing.Imaging;


namespace LocalMiniTrayApp
{
    public partial class Form3 : Form
    {
        private Form1 m_parent;

        public Form3(Form1 frm1)
        {
            InitializeComponent();
            m_parent = frm1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void Form3_Load(object sender, EventArgs e)
        {
            populateTrees();
            populateBoxes();

        }

        private void populateTrees()
        {
            int numUploads = 0;

            try
            {
                string[] uphist = m_parent.RegistryControlFetchKeys(1,1);
                char[] splitter = { ',' };
                // this populates the menu with upload history
                foreach (string urls in uphist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    numUploads++;
                    TreeNode fest = treeView1.Nodes.Add(res[0]);
                    fest.ToolTipText = res[1];
                }

                if (numUploads == 0)
                {
                    // Seems there is no upload history
                    treeView1.Nodes.Add("No history");
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 
                treeView1.Nodes.Add("error");

            }


            numUploads = 0;

            try
            {
                string[] uphist = m_parent.RegistryControlFetchKeys(2,1);
                char[] splitter = { ',' };
                // this populates the menu with upload history
                foreach (string urls in uphist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    numUploads++;
                    TreeNode fested = treeView2.Nodes.Add(res[0]);
                    fested.ToolTipText = res[1];
                }

                if (numUploads == 0)
                {
                    // Seems there is no upload history
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 

            }

        }
        private void populateBoxes()
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            
            int mhbool = int.Parse(localmini.GetValue("MHBool").ToString());
            int lhbool = int.Parse(localmini.GetValue("LHBool").ToString());
            string mhshown = localmini.GetValue("MHshown").ToString();
            string lhshown = localmini.GetValue("LHshown").ToString();
            string server = localmini.GetValue("smtpServer").ToString();
            string port = localmini.GetValue("smtpPort").ToString();
            string user = localmini.GetValue("smtpUser").ToString();
            string pass = localmini.GetValue("smtpPass").ToString();
            int ssl = int.Parse(localmini.GetValue("smtpSsl").ToString());
            string sub = localmini.GetValue("smtpSubject").ToString();
            string message = localmini.GetValue("smtpMessage").ToString();

            smtpserver.Text = server;
            smtpport.Text = port;
            smtpuser.Text = user;
            smtppass.Text = pass;
            smtpsub.Text = sub;
            smtpmessage.Text = message;
            textBox2.Text = mhshown;
            textBox3.Text = lhshown;


            if (ssl == 1)
                smtpssl.Checked = true;
            else
                smtpssl.Checked = false;

            if (lhbool == 1)
                checkBox2.Checked = true;
            else
                checkBox2.Checked = false;

            if (mhbool == 1)
                checkBox1.Checked = true;
            else
                checkBox1.Checked = false;

            try
            {

                string path = Directory.GetCurrentDirectory() + @"\Screenshots";
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                foreach (System.IO.FileInfo f in dir.GetFiles("*.png"))
                {
                    comboBox1.Items.Add(f);
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + @"\Screenshots\";
            string png = comboBox1.SelectedItem.ToString();
            //webBrowser1.Navigate(new Uri(path + png));

            Image photoImg = Image.FromFile(path + png);
            Image thumbPhoto = photoImg.GetThumbnailImage(461, 297, null, new System.IntPtr());
            pictureBox3.Image = thumbPhoto;

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you wish to save your updates?", "Closing Preferences", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                // just close
                this.Close();
            }
            else
            {
                //save settings
                int LHBool;
                int MHBool;
                int ssl;


                RegistryKey localminiq = Registry.CurrentUser;
                localminiq = localminiq.OpenSubKey("Software\\Localmini\\minihrefUrls", true);
                foreach (String valuename in localminiq.GetValueNames())
                {
                    localminiq.DeleteValue(valuename);
                }
                foreach (TreeNode node in treeView2.Nodes)
                {
                    if (node.Checked == false)
                    {
                        m_parent.RegistryControlAddKey(node.ToolTipText, node.Text, 2);
                    }
                }
                localminiq.Close();



                RegistryKey localminie = Registry.CurrentUser;
                localminie = localminie.OpenSubKey("Software\\Localmini\\localhostrUrls", true);
                foreach (String valuename in localminie.GetValueNames())
                {
                    localminie.DeleteValue(valuename);
                }
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Checked == false)
                    {
                        m_parent.RegistryControlAddKey(node.ToolTipText, node.Text, 1);
                    }
                }
                localminie.Close();




                RegistryKey localmini = Registry.CurrentUser;
                localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
                foreach (String valuename in localmini.GetValueNames())
                {
                    localmini.DeleteValue(valuename);
                }

                if (checkBox2.Checked == true)
                    LHBool = 1;
                else
                    LHBool = 0;

                if (checkBox1.Checked == true)
                    MHBool = 1;
                else
                    MHBool = 0;

                if (smtpssl.Checked == true)
                    ssl = 1;
                else
                    ssl = 0;


                localmini.SetValue("smtpServer", smtpserver.Text.ToLower(), RegistryValueKind.String);
                localmini.SetValue("smtpPort", int.Parse(smtpport.Text), RegistryValueKind.DWord);
                localmini.SetValue("smtpUser", smtpuser.Text, RegistryValueKind.String);
                localmini.SetValue("smtpPass", smtppass.Text, RegistryValueKind.String);
                localmini.SetValue("smtpSsl", ssl, RegistryValueKind.DWord);
                localmini.SetValue("smtpSubject", smtpsub.Text, RegistryValueKind.String);
                localmini.SetValue("smtpMessage", smtpmessage.Text, RegistryValueKind.String);
                localmini.SetValue("MHBool", MHBool, RegistryValueKind.DWord);
                localmini.SetValue("LHBool", LHBool, RegistryValueKind.DWord);
                localmini.SetValue("MHshown", int.Parse(textBox2.Text), RegistryValueKind.DWord);
                localmini.SetValue("LHshown", int.Parse(textBox3.Text), RegistryValueKind.DWord);
                localmini.Close();

                this.Close();
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem!=null)
            {
                Process view = new Process();

                view.StartInfo.FileName = "iexplore.exe";
                view.StartInfo.Arguments = Directory.GetCurrentDirectory() + @"\Screenshots\" + comboBox1.SelectedItem.ToString();

                view.Start();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete all the screenshots, are you sure?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                // do nothing
            }
            else
            {

                string path = Directory.GetCurrentDirectory() + @"\Screenshots";
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
                foreach (System.IO.FileInfo f in dir.GetFiles("*.png"))
                {
                    try
                    {
                        File.Delete(path + @"\" + f);
                    }
                    catch (Exception)
                    {

                    }
                }
                comboBox1.Items.Clear();
                pictureBox3.ImageLocation = Directory.GetCurrentDirectory() + @"\localmini.png";
            }
        }  
    }
}
