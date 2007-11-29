using System;
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
using System.Net.Mail;
using System.Net.Mime;
using System.Drawing;
using System.Drawing.Imaging;


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

        public Form1()
        {
            InitializeComponent();
            IconManager.Initialize();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            // welcome message
            Report("", "", 1, 0, 0);
            // makes first time retistry settings
            RegistryCheck();

        }
     
        // Registry Control, Urlwatcher, Reports
        public int UrlWatcher(string url)
        {
            // were not logging minihref urls, at least not here
            if (url.Contains("minihref"))
                return 1;
            
            // we dont want duplicates
            if(clipurls.Contains(url))
                return 1;
            
            // if the above did not return true, then we have a new link.
            clipurls.Add(url);
            return 0;
        }
        private void Report(string reportCaption, string reportMessage, int reportNumber, int reportWaiter, int reportStatus)
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
            };

            // if for some reason the bubble timer is still running
            // where gona stop it
            if (BalloonTimer.Enabled==true)
                BalloonTimer.Stop();

            // error bubble is 10 seconds
            if (reportStatus == 1)
                bubbletimeout = 10000;
            else // normal bubble is 5 seconds
                bubbletimeout = 5000;

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
                localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
                localmini.SetValue("NotificationType", 1,RegistryValueKind.DWord);
                localmini.SetValue("MHBool", 1, RegistryValueKind.DWord);
                localmini.SetValue("LHBool", 1, RegistryValueKind.DWord);
                localmini.SetValue("MHshown", 10, RegistryValueKind.DWord);
                localmini.SetValue("LHshown", 10, RegistryValueKind.DWord);
                localmini.SetValue("smtpServer", "", RegistryValueKind.String);
                localmini.SetValue("smtpPort", "", RegistryValueKind.String);
                localmini.SetValue("smtpUser", "", RegistryValueKind.String);
                localmini.SetValue("smtpPass", "", RegistryValueKind.String);
                localmini.SetValue("smtpSsl", 1, RegistryValueKind.DWord);
                localmini.SetValue("smtpSubject", "A message from God", RegistryValueKind.String);
                localmini.SetValue("smtpMessage", "Hey dude, i came accross this cool site %link%", RegistryValueKind.String);
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

        // main menu
        public void Menu_Opening(object sender, CancelEventArgs e)
        {
            populateMenuIELinks();
            populateMenuCBLinks();
            populateMenuLhHistLinks();
            populateMenuMhHistLinks();
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

                string[] uphist = RegistryControlFetchKeys(1,0);
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
                string[] uphist = RegistryControlFetchKeys(2,0);
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

        // Menu click events
        private void ClipboardUrl_Click(object sender, EventArgs e)
        {
            // set MHclipboard to 1 and call the make mini url
            MHclipboard = 1;
            MakeMiniUrl(sender, e);

        }
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 tempDialog = new Form3(this);
            tempDialog.ShowDialog();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // hide the icon before qutting, we dont want zombie icons in your taskbar
            TrayIcon.Visible = false;
            Application.Exit();
        }
        private void uploadFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
        }
        private void uploadScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            RegistryKey localmini = Registry.CurrentUser;
            localmini = localmini.OpenSubKey("Software\\Localmini\\preferences", true);
            string user = localmini.GetValue("smtpUser").ToString();
            char splitter = '@';
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
                if (oForm2.ShowDialog() == DialogResult.OK)
                    recipient = oForm2.recipientaddy;
                else
                    return;

                if (recipient.IndexOf(splitter) < 0)
                {
                    return;
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

                MailAddress from = new MailAddress(userName);
                MailAddress to = new MailAddress(recipient);
                MailMessage message = new MailMessage(from, to);
                Attachment data = new Attachment(@"C:\Documents and Settings\Administrator\Desktop\fed.jpg", MediaTypeNames.Application.Octet);
                message.Body += "<html><body>" + msg;
                message.Body += "<br><br>localmini http://www.minihref.com | http://localhostr.com <br>";
                message.Body += "</body></html>";
                message.Attachments.Add(data);
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
        }
        // make mini url, copy to clipboard
        public void copyToClipBoard(object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(sender.ToString(), true);
            Report("", "", 10, 1, 0);

        }
        public void MakeMiniUrl(object sender, System.EventArgs e)
        {
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


        }

        // Timers for Clipboard Monitoring and Balloons
        private void ClipBoardMonitor(object sender, EventArgs e)
        {
            string iData = "dummy data";
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                iData = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

            if (IsValidURL(iData))
            {
                sendLinkViaEmail.Visible = true;
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
                sendLinkViaEmail.Visible = false;
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

        private void screenshotThumnailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int width = 800;
            int height = 600;
            //if (args.Length > 2)
            //{
             //   width = Int32.Parse(args[2]);
             //   height = Int32.Parse(args[3]);
            //}

            int thumbwidth = width;
            int thumbheight = height;

            //if (args.Length > 4)
            //{
            //    thumbwidth = Int32.Parse(args[4]);
            //    thumbheight = Int32.Parse(args[5]);
            //}

            WebPageBitmap webBitmap = new WebPageBitmap("http://www.feeditout.com", width, height, false);
            webBitmap.Fetch();
            Bitmap thumbnail = webBitmap.GetBitmap(thumbwidth, thumbheight);
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +@"\fed.jpg";
            thumbnail.Save(desktop, ImageFormat.Jpeg);
            
            thumbnail.Dispose();
            System.Console.Write("done");
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
