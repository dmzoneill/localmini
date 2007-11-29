namespace LocalMiniTrayApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.sendLinkViaEmailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localhostrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentaUploadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minihrefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minihrefThisLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clipboardLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.internetExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.uploadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.skluptjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skultjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UploadFileSelect = new System.Windows.Forms.OpenFileDialog();
            this.ClipBoardTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.BalloonTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ExitLocalMini = new System.Windows.Forms.ToolStripMenuItem();
            this.PreferencesBox = new System.Windows.Forms.ToolStripMenuItem();
            this.sendLinkViaEmail = new System.Windows.Forms.ToolStripMenuItem();
            this.UploadLoadFileOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InternetExplorerLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.InternetExplorerLinksList = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScreenshot = new System.Windows.Forms.SaveFileDialog();
            this.screenshotThumnailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.MainMenu;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "LocalMini";
            this.TrayIcon.Visible = true;
            // 
            // MainMenu
            // 
            this.MainMenu.AllowDrop = true;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.toolStripSeparator3,
            this.sendLinkViaEmailToolStripMenuItem,
            this.localhostrToolStripMenuItem,
            this.minihrefToolStripMenuItem,
            this.screenshotThumnailToolStripMenuItem});
            this.MainMenu.Name = "contextMenuStrip1";
            this.MainMenu.Size = new System.Drawing.Size(227, 188);
            this.MainMenu.Opening += new System.ComponentModel.CancelEventHandler(this.Menu_Opening);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.firefox_32x32;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.configuration_settings_32x32;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(223, 6);
            // 
            // sendLinkViaEmailToolStripMenuItem
            // 
            this.sendLinkViaEmailToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.e_mail_32x32;
            this.sendLinkViaEmailToolStripMenuItem.Name = "sendLinkViaEmailToolStripMenuItem";
            this.sendLinkViaEmailToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.sendLinkViaEmailToolStripMenuItem.Text = "Send Clipboard Link Via Email";
            this.sendLinkViaEmailToolStripMenuItem.Click += new System.EventHandler(this.sendLinkViaEmailToolStripMenuItem_Click_1);
            // 
            // localhostrToolStripMenuItem
            // 
            this.localhostrToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadFilesToolStripMenuItem,
            this.uploadScreenshotToolStripMenuItem,
            this.recentaUploadsToolStripMenuItem});
            this.localhostrToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.localhostrToolStripMenuItem.Name = "localhostrToolStripMenuItem";
            this.localhostrToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.localhostrToolStripMenuItem.Text = "Localhostr";
            // 
            // uploadFilesToolStripMenuItem
            // 
            this.uploadFilesToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.uploadFilesToolStripMenuItem.Name = "uploadFilesToolStripMenuItem";
            this.uploadFilesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.uploadFilesToolStripMenuItem.Text = "Upload File";
            this.uploadFilesToolStripMenuItem.Click += new System.EventHandler(this.uploadFilesToolStripMenuItem_Click);
            // 
            // uploadScreenshotToolStripMenuItem
            // 
            this.uploadScreenshotToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.uploadScreenshotToolStripMenuItem.Name = "uploadScreenshotToolStripMenuItem";
            this.uploadScreenshotToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.uploadScreenshotToolStripMenuItem.Text = "Upload Screenshot";
            this.uploadScreenshotToolStripMenuItem.Click += new System.EventHandler(this.uploadScreenshotToolStripMenuItem_Click);
            // 
            // recentaUploadsToolStripMenuItem
            // 
            this.recentaUploadsToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.recentaUploadsToolStripMenuItem.Name = "recentaUploadsToolStripMenuItem";
            this.recentaUploadsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.recentaUploadsToolStripMenuItem.Text = "Recent Uploads";
            // 
            // minihrefToolStripMenuItem
            // 
            this.minihrefToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minihrefThisLinkToolStripMenuItem,
            this.clipboardLinksToolStripMenuItem,
            this.internetExplorerToolStripMenuItem});
            this.minihrefToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
            this.minihrefToolStripMenuItem.Name = "minihrefToolStripMenuItem";
            this.minihrefToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.minihrefToolStripMenuItem.Text = "Minihref";
            // 
            // minihrefThisLinkToolStripMenuItem
            // 
            this.minihrefThisLinkToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.favorites_32x32;
            this.minihrefThisLinkToolStripMenuItem.Name = "minihrefThisLinkToolStripMenuItem";
            this.minihrefThisLinkToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.minihrefThisLinkToolStripMenuItem.Text = "Recent Minihrefs";
            // 
            // clipboardLinksToolStripMenuItem
            // 
            this.clipboardLinksToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.clipboard_32x32;
            this.clipboardLinksToolStripMenuItem.Name = "clipboardLinksToolStripMenuItem";
            this.clipboardLinksToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.clipboardLinksToolStripMenuItem.Text = "Clipboard Links";
            // 
            // internetExplorerToolStripMenuItem
            // 
            this.internetExplorerToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
            this.internetExplorerToolStripMenuItem.Name = "internetExplorerToolStripMenuItem";
            this.internetExplorerToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.internetExplorerToolStripMenuItem.Text = "Internet Explorer";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 21);
            // 
            // uploadFile
            // 
            this.uploadFile.Name = "uploadFile";
            this.uploadFile.Size = new System.Drawing.Size(32, 19);
            // 
            // skluptjeToolStripMenuItem
            // 
            this.skluptjeToolStripMenuItem.Name = "skluptjeToolStripMenuItem";
            this.skluptjeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.skluptjeToolStripMenuItem.Text = "skluptje";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.AutoSize = false;
            this.testToolStripMenuItem.AutoToolTip = true;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.testToolStripMenuItem.Text = "test";
            // 
            // skultjeToolStripMenuItem
            // 
            this.skultjeToolStripMenuItem.Name = "skultjeToolStripMenuItem";
            this.skultjeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.skultjeToolStripMenuItem.Text = "Skultje";
            // 
            // UploadFileSelect
            // 
            this.UploadFileSelect.FileName = "openFileDialog1";
            // 
            // ClipBoardTimer
            // 
            this.ClipBoardTimer.Enabled = true;
            this.ClipBoardTimer.Interval = 1000;
            this.ClipBoardTimer.Tick += new System.EventHandler(this.ClipBoardMonitor);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 35);
            this.label1.TabIndex = 2;
            this.label1.Text = "LocalMini";
            // 
            // BalloonTimer
            // 
            this.BalloonTimer.Interval = 1000;
            this.BalloonTimer.Tick += new System.EventHandler(this.BalloonTimer_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(234, 252);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ExitLocalMini
            // 
            this.ExitLocalMini.Name = "ExitLocalMini";
            this.ExitLocalMini.Size = new System.Drawing.Size(32, 19);
            // 
            // PreferencesBox
            // 
            this.PreferencesBox.Name = "PreferencesBox";
            this.PreferencesBox.Size = new System.Drawing.Size(32, 19);
            // 
            // sendLinkViaEmail
            // 
            this.sendLinkViaEmail.Name = "sendLinkViaEmail";
            this.sendLinkViaEmail.Size = new System.Drawing.Size(32, 19);
            // 
            // UploadLoadFileOptions
            // 
            this.UploadLoadFileOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UploadLoadFileOptions.ForeColor = System.Drawing.Color.Black;
            this.UploadLoadFileOptions.Image = ((System.Drawing.Image)(resources.GetObject("UploadLoadFileOptions.Image")));
            this.UploadLoadFileOptions.Name = "UploadLoadFileOptions";
            this.UploadLoadFileOptions.Size = new System.Drawing.Size(223, 26);
            this.UploadLoadFileOptions.Text = "LocalHostr";
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(164, 26);
            this.uploadToolStripMenuItem.Text = "Upload";
            // 
            // InternetExplorerLinks
            // 
            this.InternetExplorerLinks.Name = "InternetExplorerLinks";
            this.InternetExplorerLinks.Size = new System.Drawing.Size(32, 19);
            // 
            // InternetExplorerLinksList
            // 
            this.InternetExplorerLinksList.Name = "InternetExplorerLinksList";
            this.InternetExplorerLinksList.Size = new System.Drawing.Size(32, 19);
            // 
            // saveScreenshot
            // 
            this.saveScreenshot.Filter = "PNG File|*.png";
            // 
            // screenshotThumnailToolStripMenuItem
            // 
            this.screenshotThumnailToolStripMenuItem.Name = "screenshotThumnailToolStripMenuItem";
            this.screenshotThumnailToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.screenshotThumnailToolStripMenuItem.Text = "Screenshot Thumnail";
            this.screenshotThumnailToolStripMenuItem.Click += new System.EventHandler(this.screenshotThumnailToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 298);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "LocalMini";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem UploadLoadFileOptions;
        private System.Windows.Forms.ToolStripMenuItem ExitLocalMini;
        private System.Windows.Forms.ToolStripMenuItem InternetExplorerLinks;
        private System.Windows.Forms.OpenFileDialog UploadFileSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Timer ClipBoardTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem PreferencesBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer BalloonTimer;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFile;
        private System.Windows.Forms.ToolStripMenuItem skluptjeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skultjeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendLinkViaEmail;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InternetExplorerLinksList;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendLinkViaEmailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localhostrToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentaUploadsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minihrefToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem internetExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clipboardLinksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minihrefThisLinkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem uploadScreenshotToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveScreenshot;
        private System.Windows.Forms.ToolStripMenuItem screenshotThumnailToolStripMenuItem;
    }
}

