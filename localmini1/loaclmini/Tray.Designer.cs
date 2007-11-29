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
            this.recentaUploadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.uploadScreenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minihrefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minihrefThisLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.clipboardLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.internetExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firefoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClipBoardTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.BalloonTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uploadRegistryCheck = new System.Windows.Forms.Timer(this.components);
            this.Hotspot = new System.Windows.Forms.Timer(this.components);
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.MainMenu;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "LocalMini";
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseDoubleClick);
            // 
            // MainMenu
            // 
            this.MainMenu.AllowDrop = true;
            this.MainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.toolStripSeparator3,
            this.sendLinkViaEmailToolStripMenuItem,
            this.localhostrToolStripMenuItem,
            this.minihrefToolStripMenuItem});
            this.MainMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.MainMenu.Name = "contextMenuStrip1";
            this.MainMenu.Size = new System.Drawing.Size(227, 162);
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
            this.recentaUploadsToolStripMenuItem,
            this.toolStripSeparator5,
            this.uploadScreenshotToolStripMenuItem,
            this.uploadFilesToolStripMenuItem});
            this.localhostrToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.localhostrToolStripMenuItem.Name = "localhostrToolStripMenuItem";
            this.localhostrToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.localhostrToolStripMenuItem.Text = "Localhostr";
            // 
            // recentaUploadsToolStripMenuItem
            // 
            this.recentaUploadsToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.recentaUploadsToolStripMenuItem.Name = "recentaUploadsToolStripMenuItem";
            this.recentaUploadsToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.recentaUploadsToolStripMenuItem.Text = "Recent Uploads";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(176, 6);
            // 
            // uploadScreenshotToolStripMenuItem
            // 
            this.uploadScreenshotToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.uploadScreenshotToolStripMenuItem.Name = "uploadScreenshotToolStripMenuItem";
            this.uploadScreenshotToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.uploadScreenshotToolStripMenuItem.Text = "Upload Screenshot";
            this.uploadScreenshotToolStripMenuItem.Click += new System.EventHandler(this.uploadScreenshotToolStripMenuItem_Click);
            // 
            // uploadFilesToolStripMenuItem
            // 
            this.uploadFilesToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources._991fd964de7bdbf088d3;
            this.uploadFilesToolStripMenuItem.Name = "uploadFilesToolStripMenuItem";
            this.uploadFilesToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.uploadFilesToolStripMenuItem.Text = "Upload File";
            this.uploadFilesToolStripMenuItem.Click += new System.EventHandler(this.uploadFilesToolStripMenuItem_Click);
            // 
            // minihrefToolStripMenuItem
            // 
            this.minihrefToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minihrefThisLinkToolStripMenuItem,
            this.toolStripSeparator4,
            this.clipboardLinksToolStripMenuItem,
            this.internetExplorerToolStripMenuItem,
            this.firefoxToolStripMenuItem});
            this.minihrefToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
            this.minihrefToolStripMenuItem.Name = "minihrefToolStripMenuItem";
            this.minihrefToolStripMenuItem.Size = new System.Drawing.Size(226, 26);
            this.minihrefToolStripMenuItem.Text = "Minihref";
            // 
            // minihrefThisLinkToolStripMenuItem
            // 
            this.minihrefThisLinkToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.favorites_32x32;
            this.minihrefThisLinkToolStripMenuItem.Name = "minihrefThisLinkToolStripMenuItem";
            this.minihrefThisLinkToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.minihrefThisLinkToolStripMenuItem.Text = "Recent Minihrefs";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(169, 6);
            // 
            // clipboardLinksToolStripMenuItem
            // 
            this.clipboardLinksToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.clipboard_32x32;
            this.clipboardLinksToolStripMenuItem.Name = "clipboardLinksToolStripMenuItem";
            this.clipboardLinksToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.clipboardLinksToolStripMenuItem.Text = "Clipboard Links";
            // 
            // internetExplorerToolStripMenuItem
            // 
            this.internetExplorerToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.activex_cache_32x32;
            this.internetExplorerToolStripMenuItem.Name = "internetExplorerToolStripMenuItem";
            this.internetExplorerToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.internetExplorerToolStripMenuItem.Text = "Internet Explorer";
            // 
            // firefoxToolStripMenuItem
            // 
            this.firefoxToolStripMenuItem.Image = global::WindowsFormsApplication1.Properties.Resources.firefox;
            this.firefoxToolStripMenuItem.Name = "firefoxToolStripMenuItem";
            this.firefoxToolStripMenuItem.Size = new System.Drawing.Size(172, 26);
            this.firefoxToolStripMenuItem.Text = "Firefox";
            // 
            // ClipBoardTimer
            // 
            this.ClipBoardTimer.Enabled = true;
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
            // uploadRegistryCheck
            // 
            this.uploadRegistryCheck.Tick += new System.EventHandler(this.uploadRegistryCheck_Tick);
            // 
            // Hotspot
            // 
            this.Hotspot.Enabled = true;
            this.Hotspot.Tick += new System.EventHandler(this.Hotspot_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 298);
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
        private System.Windows.Forms.Timer ClipBoardTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer BalloonTimer;
        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.ToolStripMenuItem firefoxToolStripMenuItem;
        private System.Windows.Forms.Timer uploadRegistryCheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Timer Hotspot;
    }
}

