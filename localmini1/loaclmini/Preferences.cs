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
using System.Xml;


namespace LocalMiniTrayApp
{

    public partial class Form3 : Form
    {
        // access to forms1 public functions
        private Form1 m_parent;

        public Form3(Form1 frm1)
        {
            InitializeComponent();
            m_parent = frm1;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            populateTrees();
            populateBoxes();
            populateHotKeys();
            //hotKeyWins();
        }

        // onForm load fill in forms values
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
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 

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
            int mhshown = int.Parse(localmini.GetValue("MHshown").ToString());
            int lhshown = int.Parse(localmini.GetValue("LHshown").ToString());
            string server = localmini.GetValue("smtpServer").ToString();
            int port = int.Parse(localmini.GetValue("smtpPort").ToString());
            string user = localmini.GetValue("smtpUser").ToString();
            string pass = localmini.GetValue("smtpPass").ToString();
            int ssl = int.Parse(localmini.GetValue("smtpSsl").ToString());
            string sub = localmini.GetValue("smtpSubject").ToString();
            string message = localmini.GetValue("smtpMessage").ToString();

            string emX = localmini.GetValue("hotspot-em-x").ToString();
            string emY = localmini.GetValue("hotspot-em-y").ToString();
            string mhX = localmini.GetValue("hotspot-mh-x").ToString();
            string mhY = localmini.GetValue("hotspot-mh-y").ToString();
            string ufX = localmini.GetValue("hotspot-uf-x").ToString();
            string ufY = localmini.GetValue("hotspot-uf-y").ToString();
            string ssX = localmini.GetValue("hotspot-ss-x").ToString();
            string ssY = localmini.GetValue("hotspot-ss-y").ToString();

            if (mhX == "Left" && mhY == "Top") comboBox3.SelectedIndex = 0;
            if (emX == "Left" && emY == "Top") comboBox2.SelectedIndex = 0;
            if (ufX == "Left" && ufY == "Top") comboBox4.SelectedIndex = 0;
            if (ssX == "Left" && ssY == "Top") comboBox5.SelectedIndex = 0;
            if (mhX == "Left" && mhY == "Bottom") comboBox3.SelectedIndex = 1;
            if (emX == "Left" && emY == "Bottom") comboBox2.SelectedIndex = 1;
            if (ufX == "Left" && ufY == "Bottom") comboBox4.SelectedIndex = 1;
            if (ssX == "Left" && ssY == "Bottom") comboBox5.SelectedIndex = 1;
            if (mhX == "Right" && mhY == "Bottom") comboBox3.SelectedIndex = 2;
            if (emX == "Right" && emY == "Bottom") comboBox2.SelectedIndex = 2;
            if (ufX == "Right" && ufY == "Bottom") comboBox4.SelectedIndex = 2;
            if (ssX == "Right" && ssY == "Bottom") comboBox5.SelectedIndex = 2;
            if (mhX == "Right" && mhY == "Top") comboBox3.SelectedIndex = 3;
            if (emX == "Right" && emY == "Top") comboBox2.SelectedIndex = 3;
            if (ufX == "Right" && ufY == "Top") comboBox4.SelectedIndex = 3;
            if (ssX == "Right" && ssY == "Top") comboBox5.SelectedIndex = 3;

            smtpserver.Text = server;
            numericUpDown3.Value = port;
            smtpuser.Text = user;
            smtppass.Text = pass;
            smtpsub.Text = sub;
            smtpmessage.Text = message;
            numericUpDown1.Value = mhshown;
            numericUpDown2.Value = lhshown;


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

                string path = Directory.GetCurrentDirectory().ToString() + @"\Screenshots";
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
        private void populateHotKeys()
        {

            // initialize global shortcuts
            RegistryKey hotkeys = Registry.CurrentUser;
            hotkeys = hotkeys.OpenSubKey("Software\\Localmini\\preferences", true);
            char[] splitter = { ',' };

            // hot key upload
            string hotkey = hotkeys.GetValue("hotkeyUpload").ToString();
            string[] modKey = hotkey.Split(splitter);
            if(modKey[0]=="ctrl")
            {
                checkBox10.Checked = true;
            }
            else 
            {
                checkBox9.Checked = true;
            }
            textBox6.Text = modKey[1];


            // hot key screenshot
            hotkey = hotkeys.GetValue("hotkeyScreenshot").ToString();
            modKey = hotkey.Split(splitter);
            if (modKey[0] == "ctrl")
            {
                checkBox5.Checked = true;
            }
            else
            {
                checkBox6.Checked = true;
            }
            textBox4.Text = modKey[1];


            // hot key minihref
            hotkey = hotkeys.GetValue("hotkeyMinihref").ToString();
            modKey = hotkey.Split(splitter);
            if (modKey[0] == "ctrl")
            {
                checkBox8.Checked = true;
            }
            else
            {
                checkBox7.Checked = true;
            }
            textBox5.Text = modKey[1];


            //  email hot key
            hotkey = hotkeys.GetValue("hotkeyEmail").ToString();
            modKey = hotkey.Split(splitter);
            if (modKey[0] == "ctrl")
            {
                checkBox12.Checked = true;
            }
            else
            {
                checkBox11.Checked = true;
            }
            textBox7.Text = modKey[1];
        }

        // screenhot viewer updater
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + @"\Screenshots\";
            string png = comboBox1.SelectedItem.ToString();
            Image photoImg = Image.FromFile(path + png);
            Image thumbPhoto = photoImg.GetThumbnailImage(461, 297, null, new System.IntPtr());
            pictureBox3.Image = thumbPhoto;

        }

        // screenshots tab
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
        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            { //Check for right button click
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
            else if (e.Button == MouseButtons.Left)
            { //Check for Left button click
                if (comboBox1.SelectedItem != null)
                {
                    Process view = new Process();

                    view.StartInfo.FileName = "iexplore.exe";
                    view.StartInfo.Arguments = Directory.GetCurrentDirectory() + @"\Screenshots\" + comboBox1.SelectedItem.ToString();

                    view.Start();
                }
            }
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                string Ofilename = Directory.GetCurrentDirectory() + @"\Screenshots\" + comboBox1.SelectedItem.ToString();
                File.Copy(Ofilename, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\" + comboBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to copy to desktop");
            }
        }
        
        //import / export
        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "localmini.xml";
            saveFileDialog1.Filter = "XML file (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.InitialDirectory = System.Environment.SpecialFolder.DesktopDirectory.ToString();
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string xmlSaveDir = saveFileDialog1.FileName;

                XmlTextWriter textWriter = new XmlTextWriter(xmlSaveDir, null);
                textWriter.WriteStartDocument();
                textWriter.WriteComment("LocalMini Preferences");

                // Write first element
                textWriter.WriteStartElement("localmini");
                
                // preferences
                textWriter.WriteStartElement("preferences");
                RegistryKey settings = Registry.CurrentUser;
                settings = settings.OpenSubKey("Software\\Localmini\\preferences", true);
                foreach (string prefkey in settings.GetValueNames())
                {
                    textWriter.WriteStartElement(prefkey.ToString(), "");
                    textWriter.WriteString(settings.GetValue(prefkey).ToString());
                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();


                // localhostrs
                textWriter.WriteComment("Localmini History");
                textWriter.WriteStartElement("localhostrs");
                string[] lhhist = m_parent.RegistryControlFetchKeys(1, 1);
                char[] splitter = { ',' };
                foreach (string urls in lhhist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    textWriter.WriteStartElement("lhUrl", "");
                    textWriter.WriteString(res[0] + "," + res[1]);
                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();


                // minihrefs
                textWriter.WriteComment("Minihref History");
                textWriter.WriteStartElement("minihrefs");
                string[] mhhist = m_parent.RegistryControlFetchKeys(2, 1);
                foreach (string urls in mhhist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    textWriter.WriteStartElement("mhUrl", "");
                    textWriter.WriteString(res[0] + "," + res[1]);
                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();

                // email autocomplete history
                textWriter.WriteComment("Email autocomplete history");
                textWriter.WriteStartElement("emailcontacts");
                string[] echist = m_parent.RegistryControlFetchKeys(3, 1);
                foreach (string urls in echist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    textWriter.WriteStartElement("contact", "");
                    textWriter.WriteString(res[0]);
                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();

                // Ends the document.
                textWriter.WriteEndDocument();
                // close writer
                textWriter.Close();
                m_parent.Report("", "", 16, 1, 0);
                this.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "XML file (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                if (MessageBox.Show("Are you sure you wish to import these settings?" + System.Environment.NewLine + "Any incorrect changes made by the import have to be undone manually!", "Import Settings", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    // just close
                }
                else
                {
                    char[] splitter = { ',' };

                    string filename = openFileDialog1.FileName;
                    XmlDocument localmini = new XmlDocument();
                    localmini.Load(filename);

                    XmlNodeList minihrefs = localmini.GetElementsByTagName("mhUrl");
                    XmlNodeList localhostrs = localmini.GetElementsByTagName("lhUrl");
                    XmlNodeList contacts = localmini.GetElementsByTagName("contact");
                    XmlNodeList preferences = localmini.GetElementsByTagName("preferences");


                    // delete old localhostr urls and add the ones from the xml doc
                    RegistryKey oldvals1 = Registry.CurrentUser;
                    oldvals1 = oldvals1.OpenSubKey("Software\\Localmini\\localhostrUrls", true);
                    foreach (String valuename in oldvals1.GetValueNames())
                    {
                        oldvals1.DeleteValue(valuename);
                    }
                    foreach (XmlElement lh in localhostrs)
                    {
                        string[] Val = lh.InnerText.Split(splitter);
                        m_parent.RegistryControlAddKey(Val[1], Val[0], 1);
                    }


                    // delete old contacts and add the new ones
                    RegistryKey oldvals3 = Registry.CurrentUser;
                    oldvals3 = oldvals3.OpenSubKey("Software\\Localmini\\emailcontacts", true);
                    foreach (String valuename in oldvals3.GetValueNames())
                    {
                        oldvals3.DeleteValue(valuename);
                    }
                    foreach (XmlElement ec in contacts)
                    {
                        oldvals3.SetValue(ec.InnerText, ec.InnerText, RegistryValueKind.String);
                    }

                    // delete old minihref urls and add the ones from the xml doc
                    RegistryKey oldvals2 = Registry.CurrentUser;
                    oldvals2 = oldvals2.OpenSubKey("Software\\Localmini\\minihrefUrls", true);
                    foreach (String valuename in oldvals2.GetValueNames())
                    {
                        oldvals2.DeleteValue(valuename);
                    }
                    foreach (XmlElement mh in minihrefs)
                    {

                        string[] Val = mh.InnerText.Split(splitter);
                        m_parent.RegistryControlAddKey(Val[1], Val[0], 2);
                    }



                    // update the preferences
                    RegistryKey settings = Registry.CurrentUser;
                    settings = settings.OpenSubKey("Software\\Localmini\\preferences", true);

                    foreach (XmlElement var in preferences)
                    {
                        foreach (XmlElement pref in var)
                        {
                            settings.SetValue(pref.LocalName.ToString(), pref.InnerText.ToString(), RegistryValueKind.String);
                        }
                    }
                    m_parent.Report("", "", 14, 1, 0);
                    m_parent.setHotKeys(1);
                    this.Close();
                }
            }
        }

        // treeview Delete all nodes
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey localminie = Registry.CurrentUser;
            localminie = localminie.OpenSubKey("Software\\Localmini\\minihrefUrls", true);

            foreach (TreeNode node in treeView2.Nodes)
            {
                try
                {
                    localminie.DeleteValue(node.Text);
                    node.Remove();
                }
                catch (Exception ep)
                {
                    //
                }
            }

            localminie.Close();
        }
        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey localminie = Registry.CurrentUser;
            localminie = localminie.OpenSubKey("Software\\Localmini\\localhostrUrls", true);

            foreach (TreeNode node in treeView1.Nodes)
            {
                try
                {
                    localminie.DeleteValue(node.Text);
                    node.Remove();
                }
                catch (Exception ep)
                {
                    //
                }
            }

            localminie.Close();
        }

        // save updates change events
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            if (checkBox2.Checked == true)
                localmini.SetValue("LHBool", 1, RegistryValueKind.DWord);
            else
                localmini.SetValue("LHBool", 0, RegistryValueKind.DWord);

            localmini.Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            if (checkBox1.Checked == true)
                localmini.SetValue("MHBool", 1, RegistryValueKind.DWord);
            else
                localmini.SetValue("MHBool", 0, RegistryValueKind.DWord);

            localmini.Close();
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("MHshown", numericUpDown1.Value.ToString(), RegistryValueKind.DWord);
            localmini.Close();
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("LHshown", numericUpDown2.Value.ToString(), RegistryValueKind.DWord);
            localmini.Close();
        }
        private void smtpserver_TextChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpServer", smtpserver.Text, RegistryValueKind.String);
            localmini.Close();
        }
        private void smtpuser_TextChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpUser", smtpuser.Text, RegistryValueKind.String);
            localmini.Close();
        }
        private void smtppass_TextChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpPass", smtppass.Text.ToString(), RegistryValueKind.String);
            localmini.Close();
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpPass", numericUpDown3.Value.ToString(), RegistryValueKind.DWord);
            localmini.Close();
        }
        private void smtpssl_CheckedChanged(object sender, EventArgs e)
        {
            int ssl;
                                
            if (smtpssl.Checked == true)
                ssl = 1;                
            else
                ssl = 0;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpSsl", ssl, RegistryValueKind.DWord);
            localmini.Close();
        }
        private void smtpsub_TextChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpSubject", smtpsub.Text.ToString(), RegistryValueKind.String);
            localmini.Close();
        }
        private void smtpmessage_TextChanged(object sender, EventArgs e)
        {
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("smtpMessage", smtpmessage.Text.ToString(), RegistryValueKind.String);
            localmini.Close();
        }
        private void treeView2_AfterCheck(object sender, TreeViewEventArgs e)
        {
            RegistryKey localminie = Registry.CurrentUser;
            localminie = localminie.OpenSubKey("Software\\Localmini\\minihrefUrls", true);

            foreach (TreeNode node in treeView2.Nodes)
            {
                if (node.Checked == true)
                {
                    localminie.DeleteValue(node.Text);
                    node.Remove();
                }
            }

            localminie.Close();
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            RegistryKey localminie = Registry.CurrentUser;
            localminie = localminie.OpenSubKey("Software\\Localmini\\localhostrUrls", true);

            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Checked == true)
                {
                    localminie.DeleteValue(node.Text);
                    node.Remove();
                }
            }

            localminie.Close();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            char[] splitter = { ' ' };
            string[] dim = comboBox2.SelectedItem.ToString().Split(splitter);
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("hotspot-em-x", dim[1], RegistryValueKind.String);
            localmini.SetValue("hotspot-em-y", dim[0], RegistryValueKind.String);
            localmini.Close();
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            char[] splitter = { ' ' };
            string[] dim = comboBox3.SelectedItem.ToString().Split(splitter);
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("hotspot-mh-x", dim[1], RegistryValueKind.String);
            localmini.SetValue("hotspot-mh-y", dim[0], RegistryValueKind.String);
            localmini.Close();
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            char[] splitter = { ' ' };
            string[] dim = comboBox4.SelectedItem.ToString().Split(splitter);
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("hotspot-uf-x", dim[1], RegistryValueKind.String);
            localmini.SetValue("hotspot-uf-y", dim[0], RegistryValueKind.String);
            localmini.Close();
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            char[] splitter = { ' ' };
            string[] dim = comboBox5.SelectedItem.ToString().Split(splitter);
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            localmini.SetValue("hotspot-ss-x", dim[1], RegistryValueKind.String);
            localmini.SetValue("hotspot-ss-y", dim[0], RegistryValueKind.String);
            localmini.Close();
        }
        private void checkBox5_Click(object sender, EventArgs e)
        {
            checkBox6.Checked = false;
            checkBox5.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox5.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyScreenshot", Moddy + "," + textBox4.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void checkBox6_Click(object sender, EventArgs e)
        {
            checkBox5.Checked = false;
            checkBox6.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox5.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyScreenshot", Moddy + "," + textBox4.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);
        }
        private void checkBox8_Click(object sender, EventArgs e)
        {
            checkBox7.Checked = false;
            checkBox8.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox8.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyMinihref", Moddy + "," + textBox5.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void checkBox7_Click(object sender, EventArgs e)
        {
            checkBox8.Checked = false;
            checkBox7.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox8.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyMinihref", Moddy + "," + textBox5.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void checkBox10_Click(object sender, EventArgs e)
        {
            checkBox9.Checked = false;
            checkBox10.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox10.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyUpload", Moddy + "," + textBox6.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void checkBox9_Click(object sender, EventArgs e)
        {
            checkBox10.Checked = false;
            checkBox9.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox10.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyUpload", Moddy + "," + textBox6.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void checkBox12_Click(object sender, EventArgs e)
        {
            checkBox11.Checked = false;
            checkBox12.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox12.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyEmail", Moddy + "," + textBox7.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void checkBox11_Click(object sender, EventArgs e)
        {
            checkBox12.Checked = false;
            checkBox11.Checked = true;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox12.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyEmail", Moddy + "," + textBox7.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            textBox4.Text = e.KeyData.ToString();

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox5.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyScreenshot", Moddy + "," + textBox4.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            textBox5.Text = e.KeyData.ToString();

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox8.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyMinihref", Moddy + "," + textBox5.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            textBox6.Text = e.KeyData.ToString();

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox10.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyUpload", Moddy + "," + textBox6.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }
        private void textBox7_KeyUp(object sender, KeyEventArgs e)
        {
            textBox7.Text = e.KeyData.ToString();

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string Moddy = "ctrl";

            if (checkBox12.Checked == true)
            {
                Moddy = "ctrl";
            }
            else
            {
                Moddy = "alt";
            }
            localmini.SetValue("hotkeyEmail", Moddy + "," + textBox7.Text, RegistryValueKind.String);

            localmini.Close();

            m_parent.setHotKeys(1);

        }

    }
}
