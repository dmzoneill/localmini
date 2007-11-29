using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

[assembly: CLSCompliant(true)]
namespace LocalMiniTrayApp
{

    public partial class Form1 : Form
    {

        public int MHclipboard = 0; // clipboard url 0=no  1=yes
        public int bubbletimeout = 0; // check out report()
        public int screenshot = 0; // upload progress completed report message variable
        public string lastDirectory = ""; // to remember the last directory fileupload came from
        public string PathAndFile = ""; // cross function filenames
        public ArrayList clipurls = new ArrayList(); // unique url database
        public string globrep = ""; //temp recipient
        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;
        public delegate void WebBrowserDocumentCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e);

        // working hotspot
        public int mh_working = 0;
        public int em_working = 0;
        public int ss_working = 0;
        public int uf_working = 0;

        // global hot keys stuff
        const int WM_HOTKEY = 0x0312;
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }  //simple keymodifer access        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);// register hotkeys        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);// unregister hotkey
        // hotkey function execution
        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case WM_HOTKEY:
                    if (msg.WParam.ToInt32() == 26004)
                    {
                        // screenshot
                        uploadScreenshotToolStripMenuItem.PerformClick();
                    }
                    if (msg.WParam.ToInt32() == 26003)
                    {
                        // upload file
                        uploadFilesToolStripMenuItem.PerformClick();
                    }
                    if (msg.WParam.ToInt32() == 26002)
                    {
                        if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                        {
                            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                            {
                                // minihref
                                MakeMiniUrl(Clipboard.GetDataObject().GetData(DataFormats.Text), new System.EventArgs());
                            }
                        }
                    }
                    if (msg.WParam.ToInt32() == 26001)
                    {
                        // email
                        sendLinkViaEmailToolStripMenuItem.PerformClick();

                    }

                    break;
            }
            base.WndProc(ref msg);
        }

        static Mutex appMutex = new Mutex(false, "LocalMiniApplication"); // mutex for checking for second instance
        
        // main form
        public Form1()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                RegistryKey localmini = Registry.CurrentUser;
                localmini = localmini.OpenSubKey("Software\\Localmini\\contextcommands", true);
                localmini.SetValue(args[1].ToString(), args[1].ToString(), RegistryValueKind.String);
            }
            if (appMutex.WaitOne(0, false))
            {
                // runs as normal
                InitializeComponent();
                IconManager.Initialize();
                // makes first time retistry settings
                RegistryCheck();
                // initialize global shortcuts
                setHotKeys(0);
                // start upload registry check
                uploadRegistryCheck.Start();
            }
            else
            {
                InitializeComponent();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (appMutex.WaitOne(0, false))
            {
                TrayIcon.Visible = true;
            }
            else
            {
                this.Close();
            }
            // also add windows shell context menu option
            enableContextOption();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            // welcome message
            Report("", "", 1, 0, 0);
            Hotspot.Start();
        }

        // Urlwatcher, Reports
        public int UrlWatcher(string url)
        {
            // were not logging minihref urls, at least not here
            if (url.Contains("minihref"))
                return 1;

            // we dont want duplicates
            if (clipurls.Contains(url))
                return 1;

            // if the above did not return true, then we have a new link.
            clipurls.Add(url);
            return 0;
        }
        public void Report(string reportCaption, string reportMessage, int reportNumber, int reportWaiter, int reportStatus)
        {
            // reportCaption; // Dynamic report Caption
            // reportMessage; // Dynamic report Message
            // reportNumber;  // Static report
            // reportWaiter;  // stop clipboard monitor over-riding some notifications
            // reportStatus;  // is it an error?  1 = bad,  0 = good

            // waiter is to over ride the bubble overriding prevention 
            // as the progress upload updates the bubble in realtime
            if (reportWaiter == 0)
            {
                // if the bubbletimeout is greater than zero, another bubble is active
                // so well hold off the clipboard monitor bubble notifications
                if (bubbletimeout > 0)
                    return;
            }


            string[,] zilla = new string[,]
            {
                {"",""},//0
                {"Welcome ....","Rightclick here to start working!!"},//1
                {"Working ....","Please wait while we fetch your MiniHref URL!!"},//2
                {"Finished ....","Your MiniHref URL is now in our clipboard,"+ Environment.NewLine +"Simply paste it here you want it!!"},//3
                {"Upload Successfull ....","Your file has been uploaded successfully," + Environment.NewLine + " and a link is now in your clipboard." + Environment.NewLine + "Simply paste it where you wish!"},//4
                {"File Upload Failed ....","There was a problem uploading the file, please try again later."},
                {"New Web Address Detected ....","Rightclick here to work with this url!"},
                {"Working ....","Please wait while we fetch your MiniHref URL!!"},
                {"Finished ....", "Your MiniHref URL is now in your clipboard," + Environment.NewLine + "Simply paste it where you want it!!"},
                {"Fatal Error ....","We were unable to fetch your minihref!"+Environment.NewLine+"Make sure you are connected to the internet and you can access minihref.com"},
                {"Localmini History ....","Your link has been copied to your clipboard!"},
                {"Email Settings ....","Your email settings have not been entered corrrectly, please go to preferences and set them up!"},
                {"Screenshot Upload ....","Your screenshot has been successfully uploaded and a link is now in your clipboard!" + Environment.NewLine + "Simply paste it where you wish!"},
                {"Screenshot Failed ....","Sorry but we were unable to upload your screenshot, check your internet connection!"},
                {"Settings Imported ....","Settings have been imported, Localmini preferences have been changed and will take effect immediatley!"},
                {"Settings Saved ....","Localmini preferences have been changed and will take effect immediatley!"},
                {"Settings Exported ....","Settings have been saved, to import these settings, simply use the import button in the migration tab of the preferences window"},
            };

            // if for some reason the bubble timer is still running
            // where gona stop it
            if (BalloonTimer.Enabled == true)
                BalloonTimer.Stop();

            // error bubble is 10 seconds
            if (reportStatus == 1)
                bubbletimeout = 15000;
            else // normal bubble is 5 seconds
                bubbletimeout = 10000;

            if (reportCaption != "" && reportMessage != "")
            {
                if (reportStatus == 1)
                {
                    //TrayIcon.Icon = new Icon("red.ico");
                    TrayIcon.ShowBalloonTip(1000, reportCaption, reportMessage, ToolTipIcon.Error);
                }
                else
                {
                    //TrayIcon.Icon = new Icon("purple.ico");
                    TrayIcon.ShowBalloonTip(1000, reportCaption, reportMessage, ToolTipIcon.Info);
                }
            }

            if (reportNumber > 0)
            {
                reportCaption = zilla[reportNumber, 0];
                reportMessage = zilla[reportNumber, 1];

                if (reportStatus == 1)
                {
                    //TrayIcon.Icon = new Icon("red.ico");
                    TrayIcon.ShowBalloonTip(1000, reportCaption, reportMessage, ToolTipIcon.Error);
                }
                else
                {
                    //TrayIcon.Icon = new Icon("purple.ico");
                    TrayIcon.ShowBalloonTip(1000, reportCaption, reportMessage, ToolTipIcon.Info);
                }
            }
            // start the bubble timer to allow the report to be seen by the user
            // and not over written by the clipbard report
            BalloonTimer.Start();
        }
        public string FireFoxUrlReader(string filename)
        {
            StreamReader SR;
            string S;
            string data = "";
            string rUrl = "";
            string newVal;
            SR = File.OpenText(filename);
            S = SR.ReadLine();
            while (S != null)
            {
                if (S != null)
                {
                    data = data + S;
                }
                S = SR.ReadLine();

            }
            SR.Close();

            char[] splitter = { ',' };
            string[] vals = data.Split(splitter);

            foreach (string val in vals)
            {
                if (val.Contains("{url:"))
                {
                    if (!val.Contains("tabs:"))
                    {
                        if (!val.Contains("children:"))
                        {
                            if (!val.Contains("about:blank"))
                            {
                                if (!val.Contains("wyciwyg"))
                                {
                                    if (!val.Contains("_closedTabs:"))
                                    {
                                        newVal = val;
                                        newVal = newVal.Replace("{url:", "");
                                        newVal = newVal.Replace("{entries:[", "");
                                        newVal = newVal.Replace("\"", "");
                                        rUrl = rUrl + newVal.Trim() + ",";
                                    }
                                }
                            }
                        }
                    }
                }
            }



            return rUrl;
        }

        // registry control
        private void RegistryCheck()
        {

            RegistryKey LM = Registry.CurrentUser.OpenSubKey("Software\\Localmini");
            if (LM != null)
            {
                //MessageBox.Show("We're Installed");
            }
            else
            {
                //MessageBox.Show("installing");
                RegistryKey localmini = Registry.CurrentUser;
                localmini.OpenSubKey("Software\\Localmini", true);
                localmini.CreateSubKey("Software\\Localmini\\minihrefUrls");
                localmini.CreateSubKey("Software\\Localmini\\localhostrUrls");
                localmini.CreateSubKey("Software\\Localmini\\preferences");
                localmini.CreateSubKey("Software\\Localmini\\contextcommands");
                localmini.CreateSubKey("Software\\Localmini\\emailcontacts");
                localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
                localmini.SetValue("NotificationType", "1", RegistryValueKind.String);
                localmini.SetValue("MHBool", "1", RegistryValueKind.String);
                localmini.SetValue("LHBool", "1", RegistryValueKind.String);
                localmini.SetValue("MHshown", "10", RegistryValueKind.String);
                localmini.SetValue("LHshown", "10", RegistryValueKind.String);
                localmini.SetValue("smtpServer", "", RegistryValueKind.String);
                localmini.SetValue("smtpPort", "", RegistryValueKind.String);
                localmini.SetValue("smtpUser", "", RegistryValueKind.String);
                localmini.SetValue("smtpPass", "", RegistryValueKind.String);
                localmini.SetValue("smtpSsl", "1", RegistryValueKind.String);
                localmini.SetValue("smtpSubject", "A message from God", RegistryValueKind.String);
                localmini.SetValue("smtpMessage", "Hey dude, i came accross this cool site %link%", RegistryValueKind.String);
                localmini.SetValue("hotkeyUpload", "ctrl,F9", RegistryValueKind.String);
                localmini.SetValue("hotkeyScreenshot", "ctrl,F10", RegistryValueKind.String);
                localmini.SetValue("hotkeyMinihref", "ctrl,F11", RegistryValueKind.String);
                localmini.SetValue("hotkeyEmail", "ctrl,F12", RegistryValueKind.String);
                localmini.SetValue("hotspot-em-x", "Left", RegistryValueKind.String);
                localmini.SetValue("hotspot-em-y", "Top", RegistryValueKind.String);
                localmini.SetValue("hotspot-mh-x", "Left", RegistryValueKind.String);
                localmini.SetValue("hotspot-mh-y", "Bottom", RegistryValueKind.String);
                localmini.SetValue("hotspot-ss-x", "Right", RegistryValueKind.String);
                localmini.SetValue("hotspot-ss-y", "Top", RegistryValueKind.String);
                localmini.SetValue("hotspot-uf-x", "Right", RegistryValueKind.String);
                localmini.SetValue("hotspot-uf-y", "Bottom", RegistryValueKind.String);
                localmini.Close();
            }
        }
        public string[] RegistryControlFetchKeys(int Opt, int ignoreCount)
        {
            int total = 0;
            RegistryKey localmini = Registry.CurrentUser;
            if (Opt == 1)
            {
                RegistryKey lhvalg = Registry.CurrentUser;
                lhvalg = lhvalg.OpenSubKey("Software\\Localmini\\preferences", true);
                localmini = localmini.OpenSubKey("Software\\Localmini\\localhostrUrls", true);
                if (ignoreCount == 1)
                {
                    foreach (String valuename in localmini.GetValueNames())
                    {
                        total++;
                    }
                }
                else
                {
                    total = int.Parse(lhvalg.GetValue("LHshown").ToString());
                }
                lhvalg.Close();
            }
            if (Opt == 2)
            {
                RegistryKey lhvalf = Registry.CurrentUser;
                lhvalf = lhvalf.OpenSubKey("Software\\Localmini\\preferences", true);
                localmini = localmini.OpenSubKey("Software\\Localmini\\minihrefUrls", true);
                if (ignoreCount == 1)
                {
                    foreach (String valuename in localmini.GetValueNames())
                    {
                        total++;
                    }
                }
                else
                {
                    total = int.Parse(lhvalf.GetValue("MHshown").ToString());
                }
                lhvalf.Close();
            }
            if (Opt == 3)
            {
                localmini = localmini.OpenSubKey("Software\\Localmini\\emailcontacts", true);
                if (ignoreCount == 1)
                {
                    foreach (String valuename in localmini.GetValueNames())
                    {
                        total++;
                    }
                }
            }

            string[] keys = new string[total];

            string tempObj = "";
            int icount = 0;
            int licount = 0;
            int amount = 0;

            foreach (String valuename in localmini.GetValueNames())
            {
                amount++;
            }
            int lastvals = amount - total;
            foreach (String valuename in localmini.GetValueNames())
            {

                if (ignoreCount == 1)
                {
                    tempObj = valuename + "," + localmini.GetValue(valuename).ToString();
                    keys[icount] = tempObj;
                }
                else
                {
                    if (icount >= lastvals)
                    {
                        tempObj = valuename + "," + localmini.GetValue(valuename).ToString();
                        keys[licount] = tempObj;
                        licount++;
                    }
                }
                icount++;
            }

            localmini.Close();
            return keys;
        }
        public void RegistryControlAddKey(string FileName, string Value, int opt)
        {
            RegistryKey localmini = Registry.CurrentUser;
            if (opt == 1)
            {
                // upload history
                localmini = localmini.OpenSubKey("Software\\Localmini\\localhostrUrls", true);
                localmini.SetValue(Value, FileName, RegistryValueKind.String);
            }
            if (opt == 2)
            {
                // minihref History
                localmini = localmini.OpenSubKey("Software\\Localmini\\minihrefUrls", true);
                localmini.SetValue(Value, FileName, RegistryValueKind.String);
            }
            localmini.Close();
        }

        // hotkeys
        public void setHotKeys(int reset)
        {
            if (reset == 0)
            {
                RegistryKey hotkeys = Registry.CurrentUser;
                hotkeys = hotkeys.OpenSubKey("Software\\Localmini\\preferences", true);

                string[] hotkey = new string[4];
                hotkey[0] = hotkeys.GetValue("hotkeyUpload").ToString();
                hotkey[1] = hotkeys.GetValue("hotkeyScreenshot").ToString();
                hotkey[2] = hotkeys.GetValue("hotkeyMinihref").ToString();
                hotkey[3] = hotkeys.GetValue("hotkeyEmail").ToString();

                int[] keyFuncAssign = new int[4];
                keyFuncAssign[0] = 26003;
                keyFuncAssign[1] = 26004;
                keyFuncAssign[2] = 26002;
                keyFuncAssign[3] = 26001;


                int kNum = 1;
                char[] splitter = { ',' };
                KeyModifiers kMod = KeyModifiers.Control;

                foreach (string nKey in hotkey)
                {
                    string[] ModKey = nKey.Split(splitter);
                    KeysConverter conv = new KeysConverter();
                    Keys hKey = (Keys)conv.ConvertFromString(ModKey[1]);
                    string hMod = ModKey[0];
                    if (ModKey[0] == "alt") { kMod = KeyModifiers.Alt; }
                    if (ModKey[0] == "ctrl") { kMod = KeyModifiers.Control; }
                    RegisterHotKey(Handle, keyFuncAssign[(kNum - 1)], kMod, hKey);
                    kNum++;
                }
            }
            else if (reset == 1)
            {
                resetHotkeys();
            }
        }
        public void resetHotkeys()
        {
            UnregisterHotKey(Handle, 26001);
            UnregisterHotKey(Handle, 26002);
            UnregisterHotKey(Handle, 26003);
            UnregisterHotKey(Handle, 26004);
            setHotKeys(0);
        }

        // windows shell context menu
        public void enableContextOption()
        {
            // windows shell context menu
            // file type to register
            string FileType = "*";
            // context menu name in the registry
            string KeyName = "Simple Context Menu";
            // context menu text
            string MenuText = "Upload With Localmini";

            string menuCommand = string.Format("\"{0}\" \"%L\"", Application.ExecutablePath);
            FileShellExtension.Register(FileType, KeyName, MenuText, menuCommand);
            //FileShellExtension.Unregister(FileType, KeyName);
        }

        // main menu
        public void Menu_Opening(object sender, CancelEventArgs e)
        {
            populateMenuIELinks();
            populateMenuCBLinks();
            populateMenuLhHistLinks();
            populateMenuMhHistLinks();
            populateFirefoxLinks();
        }
        // sub menus
        private void populateMenuIELinks()
        {
            int numWins = 0;
            internetExplorerToolStripMenuItem.DropDownItems.Clear();

            try
            {
                // this populates the menu with current internet explorer links
                SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();
                string filename;

                foreach (SHDocVw.InternetExplorer ie in shellWindows)
                {
                    filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();
                    if (filename.Equals("iexplore"))
                    {
                        // populate the menu with current opened web pages
                        numWins++;
                        internetExplorerToolStripMenuItem.DropDownItems.Add(ie.LocationURL);
                        internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
                        internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Enabled = true;
                        internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(MakeMiniUrl);
                    }
                }



                // If there is nothing to add to the menus
                if (numWins == 0)
                {
                    // Seems there are no webpages open, through in a message saying so
                    internetExplorerToolStripMenuItem.DropDownItems.Add("No Web Pages Open");
                    internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
                    internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
                }

            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 
                internetExplorerToolStripMenuItem.DropDownItems.Add("Error Querying Internet Explorer");
                internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
                internetExplorerToolStripMenuItem.DropDownItems[internetExplorerToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
            }

        }
        private void populateMenuCBLinks()
        {
            int numClips = 0;
            clipboardLinksToolStripMenuItem.DropDownItems.Clear();

            try
            {
                // this populates the menu with logged clipboard links
                for (int g = (clipurls.Count - 1); g > (clipurls.Count - 15); g--)
                {
                    if (g <= 0)
                    {
                        break;
                    }
                    if (clipurls[g].ToString() == "dummy data")
                    {
                        continue;
                    }
                    numClips++;
                    clipboardLinksToolStripMenuItem.DropDownItems.Add(clipurls[g].ToString());
                    if (numClips == 1)
                    {
                        clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].ForeColor = System.Drawing.Color.Red;
                    }
                    clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.clipboard_32x32;
                    clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(MakeMiniUrl);
                }

                if (numClips == 0)
                {
                    // Seems there is no clipboard history, through in a message saying so
                    clipboardLinksToolStripMenuItem.DropDownItems.Add("No Clipboard History");
                    clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.clipboard_32x32;
                    clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 
                clipboardLinksToolStripMenuItem.DropDownItems.Add("Error Querying Clipboard History");
                clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.clipboard_32x32;
                clipboardLinksToolStripMenuItem.DropDownItems[clipboardLinksToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;

            }

        }
        private void populateMenuLhHistLinks()
        {
            int numUploads = 0;
            recentaUploadsToolStripMenuItem.DropDownItems.Clear();

            try
            {

                string[] uphist = RegistryControlFetchKeys(1, 0);
                char[] splitter = { ',' };
                // this populates the menu with upload history

                foreach (string urls in uphist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    Image icon;
                    Icon tempicon;
                    if (File.Exists(res[1]))
                    {
                        IconManager.AddIcon(res[1]);
                        tempicon = IconManager.GetIcon(res[1]);
                        icon = tempicon.ToBitmap();
                    }
                    else
                    {
                        icon = WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
                    }
                    numUploads++;
                    recentaUploadsToolStripMenuItem.DropDownItems.Add(res[0]);
                    recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].ToolTipText = res[1]; ;
                    recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].Image = icon;
                    recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(copyToClipBoard);
                }

                if (numUploads == 0)
                {
                    // Seems there is no upload history
                    recentaUploadsToolStripMenuItem.DropDownItems.Add("No Upload History");
                    recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.file_32x32;
                    recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 
                recentaUploadsToolStripMenuItem.DropDownItems.Add("Error Querying Upload History");
                recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.file_32x32;
                recentaUploadsToolStripMenuItem.DropDownItems[recentaUploadsToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;

            }

        }
        private void populateMenuMhHistLinks()
        {
            int numUploads = 0;
            minihrefThisLinkToolStripMenuItem.DropDownItems.Clear();

            try
            {
                string[] uphist = RegistryControlFetchKeys(2, 0);
                char[] splitter = { ',' };
                // this populates the menu with upload history
                foreach (string urls in uphist)
                {
                    if (urls == null) break;
                    string[] res = urls.Split(splitter);
                    numUploads++;
                    minihrefThisLinkToolStripMenuItem.DropDownItems.Add(res[0]);
                    minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].ToolTipText = res[1];
                    minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.favorites_32x321;
                    minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(copyToClipBoard);
                }

                if (numUploads == 0)
                {
                    // Seems there is no upload history
                    minihrefThisLinkToolStripMenuItem.DropDownItems.Add("No Minihref History");
                    minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.favorites_32x321;
                    minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 
                minihrefThisLinkToolStripMenuItem.DropDownItems.Add("Error Querying Minihref History");
                minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.favorites_32x321;
                minihrefThisLinkToolStripMenuItem.DropDownItems[minihrefThisLinkToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
            }

        }
        private void populateFirefoxLinks()
        {
            //firefoxToolStripMenuItem
            string userProfileIni = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Mozilla\Firefox\profiles.ini";
            char[] splitter = { '=' };
            int numWins = 0;
            string firefoxUserSessionDataFile = "";

            firefoxToolStripMenuItem.DropDownItems.Clear();

            if (File.Exists(userProfileIni))
            {
                StreamReader SR;
                string S;
                SR = File.OpenText(userProfileIni);
                S = SR.ReadLine();
                while (S != null)
                {
                    S = SR.ReadLine();
                    if (S != null)
                    {
                        if (S.Contains("Path"))
                        {
                            string[] Ss = S.Split(splitter);
                            firefoxUserSessionDataFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Mozilla\Firefox\" + Ss[1].Replace('/', '\\') + @"\sessionstore.js";
                        }
                    }
                }
                SR.Close();
            }

            try
            {
                char[] splitter1 = { ',' };
                string[] uphist;
                if (File.Exists(firefoxUserSessionDataFile))
                {
                    uphist = FireFoxUrlReader(firefoxUserSessionDataFile).Split(splitter1);
                }
                else
                {
                    uphist = ",".Split(splitter1);
                }

                // this populates the menu with upload history
                foreach (string urls in uphist)
                {
                    if (urls == "") break;
                    if (urls == null) break;
                    numWins++;
                    firefoxToolStripMenuItem.DropDownItems.Add(urls);
                    firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].ToolTipText = urls;
                    firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.firefox;
                    firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].Click += new EventHandler(MakeMiniUrl);
                }

                if (numWins == 0)
                {
                    // Seems there is no upload history
                    firefoxToolStripMenuItem.DropDownItems.Add("No Web Pages Open");
                    firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.firefox;
                    firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
                }
            }

            catch (Exception)
            {
                // If there is an exception, something horrible went wrong with the drawing of the window
                //Error 
                firefoxToolStripMenuItem.DropDownItems.Add("Error Querying Firefox");
                firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].Image = WindowsFormsApplication1.Properties.Resources.firefox;
                firefoxToolStripMenuItem.DropDownItems[firefoxToolStripMenuItem.DropDownItems.Count - 1].Enabled = false;
            }

        }

        // Menu click events
        private void ClipboardUrl_Click(object sender, EventArgs e)
        {
            // set MHclipboard to 1 and call the make mini url
            MHclipboard = 1;
            MakeMiniUrl(sender, e);

        }
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 Prefs = new Form3(this);
            Prefs.ShowDialog();            
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // hide the icon before qutting, we dont want zombie icons in your taskbar
            TrayIcon.Visible = false;
            Application.Exit();
        }
        private void uploadFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uf_working = 1;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Select a file to upload";
            if (lastDirectory.Length < 1)
                lastDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            openFile.InitialDirectory = lastDirectory;
            openFile.Filter = "Gif files (*.gif)|*.gif|Jpeg files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|Zip files (*.zip)|*.zip|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFile.FilterIndex = 6;
            openFile.ShowDialog();

            if (openFile.FileNames.Length > 0)
            {
                foreach (string filename in openFile.FileNames)
                {
                    lastDirectory = System.IO.Path.GetDirectoryName(openFile.FileName);

                    try
                    {
                        System.Net.WebClient Client = new System.Net.WebClient();
                        NameValueCollection form = new NameValueCollection();
                        Uri uri = new Uri("http://api.localhostr.com/index.php");
                        form.Add("name", filename);
                        PathAndFile = filename;
                        Client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                        Client.UploadValues(uri, form);
                        Client.Headers.Add("Content-Type", "binary/octet-stream");
                        Client.UploadFileAsync(uri, "POST", filename);
                        Client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleted);
                    }
                    catch (Exception)
                    {
                        Report("", "", 5, 0, 1);
                    }
                }
            }
            uf_working = 0;
        }
        private void uploadScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ss_working = 1;
            string path = Directory.GetCurrentDirectory() + @"\Screenshots";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }

            string filename = DateTime.Now.ToString();
            filename = filename.Replace('/', '-');
            filename = filename.Replace(' ', '-');
            filename = filename.Replace(':', '-');
            filename = Directory.GetCurrentDirectory() + @"\Screenshots\" + filename + ".png";

            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            bmpScreenshot.Save(filename, ImageFormat.Png);
            lastDirectory = Directory.GetCurrentDirectory() + @"\Screenshots";

            try
            {
                screenshot = 1;
                System.Net.WebClient Client = new System.Net.WebClient();
                NameValueCollection form = new NameValueCollection();
                Uri uri = new Uri("http://api.localhostr.com/index.php");
                form.Add("name", filename);
                PathAndFile = filename;
                Client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                Client.UploadValues(uri, form);
                Client.Headers.Add("Content-Type", "binary/octet-stream");
                Client.UploadFileAsync(uri, "POST", filename);
                Client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleted);
            }
            catch (Exception)
            {
                Report("", "", 13, 1, 0);
            }
        }
        private void sendLinkViaEmailToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            em_working = 1;
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            string user = localmini.GetValue("smtpUser").ToString();
            char splitter = '@';
            int FileAttach = 0;
            string filename = "";
            string fileNameServ = "";
            Attachment data;


            if (user.IndexOf(splitter) < 0)
            {
                Report("", "", 11, 1, 1);
                return;
            }

            string iData = "dummy data";
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                iData = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

            if (IsValidURL(iData))
            {
                string recipient = "";
                recipientbox oForm2 = new recipientbox();
                //oForm2.StartPosition = FormStartPosition.Manual;
                //oForm2.Left = Screen.PrimaryScreen.WorkingArea.Right - 306;
                //oForm2.Top = Screen.PrimaryScreen.WorkingArea.Bottom - 143;
                if (oForm2.ShowDialog() == DialogResult.OK)
                    recipient = oForm2.recipientaddy;
                else
                {
                    em_working = 0;
                    return;
                }
                if (recipient.IndexOf(splitter) < 0)
                {
                    return;
                }

                if (MessageBox.Show("Would you like to attach a screenshot of the website?", "Screenshot?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    // do nothing
                }
                else
                {

                    string path = Directory.GetCurrentDirectory() + @"\Thumbnails";
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                    }

                    filename = DateTime.Now.ToString();
                    filename = filename.Replace('/', '-');
                    filename = filename.Replace(' ', '-');
                    filename = filename.Replace(':', '-');
                    fileNameServ = filename + ".png";
                    filename = Directory.GetCurrentDirectory() + @"\Thumbnails\" + filename + ".png";

                    int width = 640;
                    int height = 480;

                    int thumbwidth = width;
                    int thumbheight = height;

                    WebPageBitmap webBitmap = new WebPageBitmap(iData, width, height, false);
                    webBitmap.Fetch();
                    Bitmap thumbnail = webBitmap.GetBitmap(thumbwidth, thumbheight);
                    thumbnail.Save(filename, ImageFormat.Png);

                    thumbnail.Dispose();

                    FileAttach = 1;

                }
                Report("Sending Email to " + recipient, "Attempting to send the following link via email" + Environment.NewLine + iData, 0, 1, 0);

                globrep = recipient;
                string server = localmini.GetValue("smtpServer").ToString();
                int port = int.Parse(localmini.GetValue("smtpPort").ToString());
                user = localmini.GetValue("smtpUser").ToString();
                string pass = localmini.GetValue("smtpPass").ToString();
                int ssl = int.Parse(localmini.GetValue("smtpSsl").ToString());
                string sub = localmini.GetValue("smtpSubject").ToString();
                string smessage = localmini.GetValue("smtpMessage").ToString();

                int googleMailPortNumber = port;//465
                string userName = user;
                string passWord = pass;

                string msg = smessage.Replace("%link%", iData);

                // save emails for later use
                RegistryKey addAddy = Registry.CurrentUser;
                addAddy = addAddy.OpenSubKey("Software\\Localmini\\emailcontacts", true);
                addAddy.SetValue(recipient, recipient, RegistryValueKind.String);
                addAddy.Close();


                MailAddress from = new MailAddress(userName);
                MailAddress to = new MailAddress(recipient);
                MailMessage message = new MailMessage(from, to);
                message.IsBodyHtml = true;
                message.Body += "<html><body>" + msg;
                if (FileAttach == 1)
                {
                    data = new Attachment(filename, MediaTypeNames.Application.Octet);
                    data.ContentId = fileNameServ;
                    message.Attachments.Add(data);
                    message.Body += "<br><br>Here is a preview of the website<br><br><img src='cid:" + fileNameServ + "'>";
                }

                message.Body += "<br><br>localmini http://www.minihref.com | http://localhostr.com <br>";
                message.Body += "</body></html>";
                message.Subject = sub;

                message.SubjectEncoding = System.Text.Encoding.UTF8;

                SmtpClient client = new SmtpClient(server, googleMailPortNumber);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(userName, passWord);

                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                string userState = sub;
                client.SendAsync(message, userState);
            }
            em_working = 0;
        }

        // Event Handlers, Upload, smtp
        private void UploadProgressCallback(object sender, UploadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            string percent = (e.ProgressPercentage * 2).ToString();
            string bytessent = (e.BytesSent / 1024).ToString();
            string totalbytes = (e.TotalBytesToSend / 1024).ToString();
            if (e.ProgressPercentage > 50)
                percent = "100";

            Report("Working ....", percent + "%, " + bytessent + " / " + totalbytes + " KB Uploaded.", 0, 1, 0);

        }
        private void UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            System.Threading.AutoResetEvent waiter = (System.Threading.AutoResetEvent)e.UserState;
            try
            {
                string reply = "http://" + System.Text.Encoding.UTF8.GetString(e.Result);
                RegistryKey localmini = Registry.CurrentUser;
                localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
                int lhbool = int.Parse(localmini.GetValue("LHBool").ToString());
                localmini.Close();
                if (lhbool == 1)
                {
                    RegistryControlAddKey(PathAndFile, reply, 1);
                }
                Clipboard.SetDataObject(reply, true);
                if (screenshot == 1)
                {
                    Report("", "", 12, 1, 0);
                    screenshot = 0;
                }
                else
                {
                    Report("", "", 4, 1, 0);
                }
            }
            catch (Exception)
            {
                Report("", "", 5, 1, 1);
            }
            uf_working = 0;
            ss_working = 0;
        }
        // smtp
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Report("Error Sending Email", e.Error.ToString(), 0, 1, 1);

            }
            else
            {
                Report("Email Sent", "Your email has been successfully sent to " + globrep, 0, 1, 0);
            }
            em_working = 0;
        }
        // make mini url, copy to clipboard
        public void copyToClipBoard(object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(sender.ToString(), true);
            Report("", "", 10, 1, 0);

        }
        public void MakeMiniUrl(object sender, System.EventArgs e)
        {
            mh_working = 1;
            // this is multifunction
            // sender can be the name (object) of the link in the internet url list
            // or sender can come from clipboard option
            // MHclipboard is set in clipboard option click event 
            // and this function is called

            string item;
            if (MHclipboard == 0)
            {
                Report("", "", 2, 0, 0);
                item = sender.ToString();
            }
            else
            {
                Report("", "", 7, 0, 0);
                item = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
            }

            try
            {
                // open post request to minihref.com
                // get the result
                // parse it
                // set clipboard value and display report

                WebRequest request = WebRequest.Create("http://www.minihref.com/");
                ((HttpWebRequest)request).UserAgent = "LocalMini v1.0";
                request.Method = "POST";
                string postData = "link=" + item;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                string sLine1 = "";

                while (sLine1 != null)
                {
                    sLine1 = reader.ReadLine();
                    if (sLine1 != null)
                    {
                        if (sLine1.Contains("maxlength"))
                        {
                            string[] arInfo;
                            char[] splitter = { '"' };
                            arInfo = sLine1.Split(splitter);
                            foreach (string element in arInfo)
                            {
                                if (IsValidURL(element))
                                {
                                    if (MHclipboard == 0)
                                    {
                                        RegistryKey localmini = Registry.CurrentUser;
                                        localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
                                        int mhbool = int.Parse(localmini.GetValue("MHBool").ToString());
                                        localmini.Close();
                                        if (mhbool == 1)
                                        {
                                            RegistryControlAddKey(item, element, 2);
                                        }
                                        Report("", "", 3, 1, 0);
                                    }
                                    else
                                    {
                                        RegistryKey localmini = Registry.CurrentUser;
                                        localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
                                        int mhbool = int.Parse(localmini.GetValue("MHBool").ToString());
                                        localmini.Close();
                                        if (mhbool == 1)
                                        {
                                            RegistryControlAddKey(item, element, 2);
                                        }
                                        Report("", "", 8, 1, 0);
                                        MHclipboard = 0;
                                    }


                                    Clipboard.SetDataObject(element, true);
                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }                    
                }

                //System.Console.WriteLine(sMini);
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception)
            {
                Report("", "", 9, 1, 1);
            }
            mh_working = 0;
        }

        // Timers for Clipboard Monitoring, Balloons, contextmenu upload watcher
        private void ClipBoardMonitor(object sender, EventArgs e)
        {
            string iData = "dummy data";
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                iData = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

            if (IsValidURL(iData))
            {
                sendLinkViaEmailToolStripMenuItem.Visible = true;

                // its text
                if (UrlWatcher(iData) == 1)
                {
                    // if we already have the url or the url consists of minhref
                    // there no point in annoying the user or making minihrefs
                    // from minihrefs

                }
                else
                {
                    // its not a minihref and its not already been detected in this session
                    Report("", "", 6, 0, 0);
                }
            }
            else
            {
                sendLinkViaEmailToolStripMenuItem.Visible = false;
                // the clipboard data is not even text
            }
        }
        private void BalloonTimer_Tick(object sender, EventArgs e)
        {
            // bit of a hack to counter the default operating system bubble timeout
            // by making the icon invisible, it stops the bubble
            // then instantly making it visible again
            // were also going to use this to prevent one ballon overriding another
            // refer to report() for clarity
            bubbletimeout = bubbletimeout - 1000;
            if (bubbletimeout <= 0)
            {
                TrayIcon.Visible = false;
                TrayIcon.Visible = true;
                bubbletimeout = 0;
                //TrayIcon.Icon = new Icon("green.ico");
                BalloonTimer.Stop();
            }
        }
        private void uploadRegistryCheck_Tick(object sender, EventArgs e)
        {
            RegistryKey commands = Registry.CurrentUser;
            commands = commands.OpenSubKey("Software\\Localmini\\contextcommands", true);
            foreach (String valuename in commands.GetValueNames())
            {
                try
                {
                    string filename = valuename.ToString();
                    System.Net.WebClient Client = new System.Net.WebClient();
                    NameValueCollection form = new NameValueCollection();
                    Uri uri = new Uri("http://api.localhostr.com/index.php");
                    form.Add("name", filename);
                    PathAndFile = filename;
                    Client.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                    Client.UploadValues(uri, form);
                    Client.Headers.Add("Content-Type", "binary/octet-stream");
                    Client.UploadFileAsync(uri, "POST", filename);
                    Client.UploadFileCompleted += new UploadFileCompletedEventHandler(UploadFileCompleted);
                    commands.DeleteValue(valuename);
                }
                catch (Exception)
                {
                    Report("", "", 5, 0, 1);
                }
            }
        }

        // URL Checking
        static bool MatchString(string str, string regexstr)
        {
            str = str.Trim();
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(regexstr);
            return pattern.IsMatch(str);
        }
        static bool IsValidURL(string strURL)
        {
            string regExPattern = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
            return MatchString(strURL, regExPattern);
        }

        // tray icon
        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form3 tempDialog = new Form3(this);
            //tempDialog.StartPosition = FormStartPosition.Manual;
            //tempDialog.Left = Screen.PrimaryScreen.WorkingArea.Right - 509;
            //tempDialog.Top = Screen.PrimaryScreen.WorkingArea.Bottom - 418;
            tempDialog.ShowDialog();
        }

        //hot spot monitor
        private void Hotspot_Tick(object sender, EventArgs e)
        {
            int Padding = 2;
            int CursorX = int.Parse(Cursor.Position.X.ToString());
            int CursorY = int.Parse(Cursor.Position.Y.ToString());
            int DesktopHeight = int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString()) - Padding;
            int DesktopWidth = int.Parse(Screen.PrimaryScreen.Bounds.Width.ToString()) - Padding;

            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);

            string mhX = localmini.GetValue("hotspot-mh-x").ToString();
            string mhY = localmini.GetValue("hotspot-mh-y").ToString();
            string emX = localmini.GetValue("hotspot-em-x").ToString();
            string emY = localmini.GetValue("hotspot-em-y").ToString();
            string ufX = localmini.GetValue("hotspot-uf-x").ToString();
            string ufY = localmini.GetValue("hotspot-uf-y").ToString();
            string ssX = localmini.GetValue("hotspot-ss-x").ToString();
            string ssY = localmini.GetValue("hotspot-ss-y").ToString();
            
            // top left
            if (CursorX < Padding && CursorY < Padding)
            {
                if (mhX == "Left" && mhY == "Top" && mh_working == 0) MakeMiniUrl(Clipboard.GetDataObject().GetData(DataFormats.Text), new System.EventArgs());
                if (emX == "Left" && emY == "Top" && em_working == 0) sendLinkViaEmailToolStripMenuItem.PerformClick();
                if (ufX == "Left" && ufY == "Top" && uf_working == 0) uploadFilesToolStripMenuItem.PerformClick();
                if (ssX == "Left" && ssY == "Top" && ss_working == 0) uploadScreenshotToolStripMenuItem.PerformClick();
            }
            // bottom left
            if (CursorX < Padding && CursorY > DesktopHeight)
            {
                if (mhX == "Left" && mhY == "Bottom" && mh_working == 0) MakeMiniUrl(Clipboard.GetDataObject().GetData(DataFormats.Text), new System.EventArgs());
                if (emX == "Left" && emY == "Bottom" && em_working == 0) sendLinkViaEmailToolStripMenuItem.PerformClick();
                if (ufX == "Left" && ufY == "Bottom" && uf_working == 0) uploadFilesToolStripMenuItem.PerformClick();
                if (ssX == "Left" && ssY == "Bottom" && ss_working == 0) uploadScreenshotToolStripMenuItem.PerformClick();
            }
            // bottom right
            if (CursorX > DesktopWidth && CursorY > DesktopHeight)
            {
                if (mhX == "Right" && mhY == "Bottom" && mh_working == 0) MakeMiniUrl(Clipboard.GetDataObject().GetData(DataFormats.Text), new System.EventArgs());
                if (emX == "Right" && emY == "Bottom" && em_working == 0) sendLinkViaEmailToolStripMenuItem.PerformClick();
                if (ufX == "Right" && ufY == "Bottom" && uf_working == 0) uploadFilesToolStripMenuItem.PerformClick();
                if (ssX == "Right" && ssY == "Bottom" && ss_working == 0) uploadScreenshotToolStripMenuItem.PerformClick();
            }
            // top right
            if (CursorX > DesktopWidth && CursorY < (int.Parse(Screen.PrimaryScreen.Bounds.Height.ToString()) - DesktopHeight))
            {
                if (mhX == "Right" && mhY == "Top" && mh_working == 0) MakeMiniUrl(Clipboard.GetDataObject().GetData(DataFormats.Text), new System.EventArgs());
                if (emX == "Right" && emY == "Top" && em_working == 0) sendLinkViaEmailToolStripMenuItem.PerformClick();
                if (ufX == "Right" && ufY == "Top" && uf_working == 0) uploadFilesToolStripMenuItem.PerformClick();
                if (ssX == "Right" && ssY == "Top" && ss_working == 0) uploadScreenshotToolStripMenuItem.PerformClick();
            }

            localmini.Close();

        }

    }

    public class Pair
    {
        public Icon icon;
        public string filename;

        public Pair(Icon ico, string file)
        {
            icon = ico;
            filename = file;
        }
    }
}
