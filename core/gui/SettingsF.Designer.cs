namespace Sarkui
{
     partial class SettingsF
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsF));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FfmpegLocation = new Sarkui.FileBar();
            this.lblffmpeg = new System.Windows.Forms.Label();
            this.useFfmpeg = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MkvpropLocation = new Sarkui.FileBar();
            this.lblmkvprop = new System.Windows.Forms.Label();
            this.useMkvpropedit = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.MkvmergeLocation = new Sarkui.FileBar();
            this.lblmkvmerge = new System.Windows.Forms.Label();
            this.useMkvmerge = new System.Windows.Forms.CheckBox();
            this.defaultOutputDir = new Sarkui.FileBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.MkvextLocation = new Sarkui.FileBar();
            this.lblmkvext = new System.Windows.Forms.Label();
            this.useMkvext = new System.Windows.Forms.CheckBox();
            this.useMp4box = new System.Windows.Forms.CheckBox();
            this.lblmp4box = new System.Windows.Forms.Label();
            this.Mp4boxLocation = new Sarkui.FileBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flowback = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.SoxLocation = new Sarkui.FileBar();
            this.lblsox = new System.Windows.Forms.Label();
            this.useSox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Controls.Add(this.FfmpegLocation);
            this.groupBox1.Controls.Add(this.lblffmpeg);
            this.groupBox1.Controls.Add(this.useFfmpeg);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 57);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // FfmpegLocation
            // 
            this.FfmpegLocation.BackColor = System.Drawing.SystemColors.Control;
            this.FfmpegLocation.Enabled = false;
            this.FfmpegLocation.Filename = "";
            this.FfmpegLocation.Filter = "Ffmpeg|ffmpeg.exe";
            this.FfmpegLocation.Location = new System.Drawing.Point(85, 23);
            this.FfmpegLocation.Name = "FfmpegLocation";
            this.FfmpegLocation.Size = new System.Drawing.Size(426, 26);
            this.FfmpegLocation.TabIndex = 0;
            // 
            // lblffmpeg
            // 
            this.lblffmpeg.AutoSize = true;
            this.lblffmpeg.Enabled = false;
            this.lblffmpeg.Location = new System.Drawing.Point(11, 31);
            this.lblffmpeg.Name = "lblffmpeg";
            this.lblffmpeg.Size = new System.Drawing.Size(48, 13);
            this.lblffmpeg.TabIndex = 1;
            this.lblffmpeg.Text = "Location";
            // 
            // useFfmpeg
            // 
            this.useFfmpeg.AutoSize = true;
            this.useFfmpeg.Location = new System.Drawing.Point(6, 0);
            this.useFfmpeg.Name = "useFfmpeg";
            this.useFfmpeg.Size = new System.Drawing.Size(94, 17);
            this.useFfmpeg.TabIndex = 0;
            this.useFfmpeg.Text = "Enable ffmpeg";
            this.useFfmpeg.UseVisualStyleBackColor = true;
            this.useFfmpeg.CheckedChanged += new System.EventHandler(this.useFfmpeg_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 393);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(537, 35);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(459, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(378, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox2.Controls.Add(this.MkvpropLocation);
            this.groupBox2.Controls.Add(this.lblmkvprop);
            this.groupBox2.Controls.Add(this.useMkvpropedit);
            this.groupBox2.Location = new System.Drawing.Point(8, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 57);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // MkvpropLocation
            // 
            this.MkvpropLocation.BackColor = System.Drawing.SystemColors.Control;
            this.MkvpropLocation.Enabled = false;
            this.MkvpropLocation.Filename = "";
            this.MkvpropLocation.Filter = "Mkvpropedit|mkvpropedit.exe";
            this.MkvpropLocation.Location = new System.Drawing.Point(85, 25);
            this.MkvpropLocation.Name = "MkvpropLocation";
            this.MkvpropLocation.Size = new System.Drawing.Size(426, 26);
            this.MkvpropLocation.TabIndex = 3;
            // 
            // lblmkvprop
            // 
            this.lblmkvprop.AutoSize = true;
            this.lblmkvprop.Enabled = false;
            this.lblmkvprop.Location = new System.Drawing.Point(11, 31);
            this.lblmkvprop.Name = "lblmkvprop";
            this.lblmkvprop.Size = new System.Drawing.Size(48, 13);
            this.lblmkvprop.TabIndex = 1;
            this.lblmkvprop.Text = "Location";
            // 
            // useMkvpropedit
            // 
            this.useMkvpropedit.AutoSize = true;
            this.useMkvpropedit.Location = new System.Drawing.Point(6, 0);
            this.useMkvpropedit.Name = "useMkvpropedit";
            this.useMkvpropedit.Size = new System.Drawing.Size(120, 17);
            this.useMkvpropedit.TabIndex = 3;
            this.useMkvpropedit.Text = "Enable mkvpropedit";
            this.useMkvpropedit.UseVisualStyleBackColor = true;
            this.useMkvpropedit.CheckedChanged += new System.EventHandler(this.useMkvpropedit_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox3.Controls.Add(this.MkvmergeLocation);
            this.groupBox3.Controls.Add(this.lblmkvmerge);
            this.groupBox3.Controls.Add(this.useMkvmerge);
            this.groupBox3.Location = new System.Drawing.Point(8, 135);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(517, 57);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // MkvmergeLocation
            // 
            this.MkvmergeLocation.BackColor = System.Drawing.SystemColors.Control;
            this.MkvmergeLocation.Enabled = false;
            this.MkvmergeLocation.Filename = "";
            this.MkvmergeLocation.Filter = "Mkvmerge|mkvmerge.exe";
            this.MkvmergeLocation.Location = new System.Drawing.Point(85, 23);
            this.MkvmergeLocation.Name = "MkvmergeLocation";
            this.MkvmergeLocation.Size = new System.Drawing.Size(426, 26);
            this.MkvmergeLocation.TabIndex = 4;
            this.MkvmergeLocation.Load += new System.EventHandler(this.MkvmergeLocation_Load);
            // 
            // lblmkvmerge
            // 
            this.lblmkvmerge.AutoSize = true;
            this.lblmkvmerge.Enabled = false;
            this.lblmkvmerge.Location = new System.Drawing.Point(11, 31);
            this.lblmkvmerge.Name = "lblmkvmerge";
            this.lblmkvmerge.Size = new System.Drawing.Size(48, 13);
            this.lblmkvmerge.TabIndex = 1;
            this.lblmkvmerge.Text = "Location";
            // 
            // useMkvmerge
            // 
            this.useMkvmerge.AutoSize = true;
            this.useMkvmerge.Location = new System.Drawing.Point(6, 0);
            this.useMkvmerge.Name = "useMkvmerge";
            this.useMkvmerge.Size = new System.Drawing.Size(111, 17);
            this.useMkvmerge.TabIndex = 3;
            this.useMkvmerge.Text = "Enable mkvmerge";
            this.useMkvmerge.UseVisualStyleBackColor = true;
            this.useMkvmerge.CheckedChanged += new System.EventHandler(this.useMkvmerge_CheckedChanged);
            // 
            // defaultOutputDir
            // 
            this.defaultOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.defaultOutputDir.Filename = "";
            this.defaultOutputDir.FolderMode = true;
            this.defaultOutputDir.Location = new System.Drawing.Point(8, 27);
            this.defaultOutputDir.Margin = new System.Windows.Forms.Padding(4);
            this.defaultOutputDir.Name = "defaultOutputDir";
            this.defaultOutputDir.Size = new System.Drawing.Size(329, 23);
            this.defaultOutputDir.TabIndex = 40;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox5.Controls.Add(this.MkvextLocation);
            this.groupBox5.Controls.Add(this.lblmkvext);
            this.groupBox5.Controls.Add(this.useMkvext);
            this.groupBox5.Location = new System.Drawing.Point(8, 261);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(517, 57);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            // 
            // MkvextLocation
            // 
            this.MkvextLocation.BackColor = System.Drawing.SystemColors.Control;
            this.MkvextLocation.Enabled = false;
            this.MkvextLocation.Filename = "";
            this.MkvextLocation.Filter = "Mkvextract|mkvextract.exe";
            this.MkvextLocation.Location = new System.Drawing.Point(85, 23);
            this.MkvextLocation.Name = "MkvextLocation";
            this.MkvextLocation.Size = new System.Drawing.Size(426, 26);
            this.MkvextLocation.TabIndex = 4;
            // 
            // lblmkvext
            // 
            this.lblmkvext.AutoSize = true;
            this.lblmkvext.Enabled = false;
            this.lblmkvext.Location = new System.Drawing.Point(11, 31);
            this.lblmkvext.Name = "lblmkvext";
            this.lblmkvext.Size = new System.Drawing.Size(48, 13);
            this.lblmkvext.TabIndex = 1;
            this.lblmkvext.Text = "Location";
            // 
            // useMkvext
            // 
            this.useMkvext.AutoSize = true;
            this.useMkvext.Location = new System.Drawing.Point(6, 0);
            this.useMkvext.Name = "useMkvext";
            this.useMkvext.Size = new System.Drawing.Size(114, 17);
            this.useMkvext.TabIndex = 3;
            this.useMkvext.Text = "Enable mkvextract";
            this.useMkvext.UseVisualStyleBackColor = true;
            this.useMkvext.CheckedChanged += new System.EventHandler(this.useMkvext_CheckedChanged);
            // 
            // useMp4box
            // 
            this.useMp4box.AutoSize = true;
            this.useMp4box.Location = new System.Drawing.Point(6, 0);
            this.useMp4box.Name = "useMp4box";
            this.useMp4box.Size = new System.Drawing.Size(99, 17);
            this.useMp4box.TabIndex = 0;
            this.useMp4box.Text = "Enable mp4box";
            this.useMp4box.UseVisualStyleBackColor = true;
            this.useMp4box.CheckedChanged += new System.EventHandler(this.useMp4box_CheckedChanged);
            // 
            // lblmp4box
            // 
            this.lblmp4box.AutoSize = true;
            this.lblmp4box.Enabled = false;
            this.lblmp4box.Location = new System.Drawing.Point(11, 31);
            this.lblmp4box.Name = "lblmp4box";
            this.lblmp4box.Size = new System.Drawing.Size(48, 13);
            this.lblmp4box.TabIndex = 1;
            this.lblmp4box.Text = "Location";
            // 
            // Mp4boxLocation
            // 
            this.Mp4boxLocation.BackColor = System.Drawing.SystemColors.Control;
            this.Mp4boxLocation.Enabled = false;
            this.Mp4boxLocation.Filename = "";
            this.Mp4boxLocation.Filter = "mp4box|mp4box.exe";
            this.Mp4boxLocation.Location = new System.Drawing.Point(85, 23);
            this.Mp4boxLocation.Name = "Mp4boxLocation";
            this.Mp4boxLocation.Size = new System.Drawing.Size(426, 26);
            this.Mp4boxLocation.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox4.Controls.Add(this.Mp4boxLocation);
            this.groupBox4.Controls.Add(this.lblmp4box);
            this.groupBox4.Controls.Add(this.useMp4box);
            this.groupBox4.Location = new System.Drawing.Point(8, 198);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(517, 57);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // flowback
            // 
            this.flowback.Location = new System.Drawing.Point(0, 1);
            this.flowback.Name = "flowback";
            this.flowback.Size = new System.Drawing.Size(537, 389);
            this.flowback.TabIndex = 34;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox6.Controls.Add(this.SoxLocation);
            this.groupBox6.Controls.Add(this.lblsox);
            this.groupBox6.Controls.Add(this.useSox);
            this.groupBox6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groupBox6.Location = new System.Drawing.Point(8, 327);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(517, 57);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "groupBox6";
            // 
            // SoxLocation
            // 
            this.SoxLocation.BackColor = System.Drawing.SystemColors.Control;
            this.SoxLocation.Enabled = false;
            this.SoxLocation.Filename = "";
            this.SoxLocation.Filter = "Sox|sox.exe";
            this.SoxLocation.Location = new System.Drawing.Point(85, 23);
            this.SoxLocation.Name = "SoxLocation";
            this.SoxLocation.Size = new System.Drawing.Size(426, 26);
            this.SoxLocation.TabIndex = 4;
            // 
            // lblsox
            // 
            this.lblsox.AutoSize = true;
            this.lblsox.Enabled = false;
            this.lblsox.Location = new System.Drawing.Point(11, 31);
            this.lblsox.Name = "lblsox";
            this.lblsox.Size = new System.Drawing.Size(48, 13);
            this.lblsox.TabIndex = 1;
            this.lblsox.Text = "Location";
            // 
            // useSox
            // 
            this.useSox.AutoSize = true;
            this.useSox.Location = new System.Drawing.Point(6, 0);
            this.useSox.Name = "useSox";
            this.useSox.Size = new System.Drawing.Size(78, 17);
            this.useSox.TabIndex = 3;
            this.useSox.Text = "Enable sox";
            this.useSox.UseVisualStyleBackColor = true;
            this.useSox.CheckedChanged += new System.EventHandler(this.useSox_CheckedChanged);
            // 
            // SettingsF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(537, 428);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowback);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsF";
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private FileBar defaultOutputDir;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblffmpeg;
        private System.Windows.Forms.CheckBox useFfmpeg;
        public System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblmkvprop;
        private System.Windows.Forms.CheckBox useMkvpropedit;
        public System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblmkvmerge;
        private System.Windows.Forms.CheckBox useMkvmerge;
        public FileBar FfmpegLocation;
        //    public FileBar fileBar1;
        public FileBar MkvpropLocation;
        public FileBar MkvmergeLocation;
        public System.Windows.Forms.Button saveButton;
        public System.Windows.Forms.Button cancelButton;
        public System.Windows.Forms.GroupBox groupBox5;
        public FileBar MkvextLocation;
        private System.Windows.Forms.Label lblmkvext;
        private System.Windows.Forms.CheckBox useMkvext;
        private System.Windows.Forms.CheckBox useMp4box;
        private System.Windows.Forms.Label lblmp4box;
        public FileBar Mp4boxLocation;
        public System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.FlowLayoutPanel flowback;
        public System.Windows.Forms.GroupBox groupBox6;
        public FileBar SoxLocation;
        private System.Windows.Forms.Label lblsox;
        private System.Windows.Forms.CheckBox useSox;
    }
}