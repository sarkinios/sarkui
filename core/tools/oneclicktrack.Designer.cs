namespace Sarkui
{
    partial class oneclicktrack
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.deftrvidlbl = new System.Windows.Forms.CheckBox();
            this.trackvidlbl = new System.Windows.Forms.CheckBox();
            this.vidnumb = new System.Windows.Forms.TextBox();
            this.startlbl = new System.Windows.Forms.Label();
            this.numvidlbl = new System.Windows.Forms.CheckBox();
            this.input = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.languageBar1 = new Sarkui.LanguageBar();
            this.edittrvideo = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deftrvidlbl
            // 
            this.deftrvidlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.deftrvidlbl.AutoSize = true;
            this.deftrvidlbl.Enabled = false;
            this.deftrvidlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.deftrvidlbl.Location = new System.Drawing.Point(378, 55);
            this.deftrvidlbl.Name = "deftrvidlbl";
            this.deftrvidlbl.Size = new System.Drawing.Size(91, 17);
            this.deftrvidlbl.TabIndex = 22;
            this.deftrvidlbl.Text = "Default Track";
            this.deftrvidlbl.UseVisualStyleBackColor = true;
            this.deftrvidlbl.CheckedChanged += new System.EventHandler(this.deftrvidlbl_CheckedChanged);
            // 
            // trackvidlbl
            // 
            this.trackvidlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.trackvidlbl.AutoSize = true;
            this.trackvidlbl.Enabled = false;
            this.trackvidlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.trackvidlbl.Location = new System.Drawing.Point(8, 56);
            this.trackvidlbl.Name = "trackvidlbl";
            this.trackvidlbl.Size = new System.Drawing.Size(85, 17);
            this.trackvidlbl.TabIndex = 21;
            this.trackvidlbl.Text = "Track Name";
            this.trackvidlbl.UseVisualStyleBackColor = true;
            this.trackvidlbl.CheckedChanged += new System.EventHandler(this.trackvidlbl_CheckedChanged);
            // 
            // vidnumb
            // 
            this.vidnumb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.vidnumb.BackColor = System.Drawing.SystemColors.Window;
            this.vidnumb.Enabled = false;
            this.vidnumb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.vidnumb.ForeColor = System.Drawing.Color.Black;
            this.vidnumb.Location = new System.Drawing.Point(117, 89);
            this.vidnumb.Name = "vidnumb";
            this.vidnumb.Size = new System.Drawing.Size(40, 20);
            this.vidnumb.TabIndex = 16;
            this.vidnumb.Text = "1";
            this.vidnumb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.vidnumb.TextChanged += new System.EventHandler(this.vidnumb_TextChanged);
            this.vidnumb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vidnumb_KeyPress);
            // 
            // startlbl
            // 
            this.startlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.startlbl.AutoSize = true;
            this.startlbl.Enabled = false;
            this.startlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.startlbl.Location = new System.Drawing.Point(84, 94);
            this.startlbl.Name = "startlbl";
            this.startlbl.Size = new System.Drawing.Size(32, 13);
            this.startlbl.TabIndex = 15;
            this.startlbl.Text = "Start:";
            // 
            // numvidlbl
            // 
            this.numvidlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.numvidlbl.AutoSize = true;
            this.numvidlbl.Enabled = false;
            this.numvidlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.numvidlbl.Location = new System.Drawing.Point(8, 92);
            this.numvidlbl.Name = "numvidlbl";
            this.numvidlbl.Size = new System.Drawing.Size(80, 17);
            this.numvidlbl.TabIndex = 14;
            this.numvidlbl.Text = "Numbering:";
            this.numvidlbl.UseVisualStyleBackColor = true;
            this.numvidlbl.CheckedChanged += new System.EventHandler(this.numvidlbl_CheckedChanged);
            // 
            // input
            // 
            this.input.BackColor = System.Drawing.SystemColors.Window;
            this.input.Enabled = false;
            this.input.ForeColor = System.Drawing.Color.Black;
            this.input.Location = new System.Drawing.Point(99, 53);
            this.input.Multiline = true;
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(265, 22);
            this.input.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.languageBar1);
            this.panel1.Controls.Add(this.edittrvideo);
            this.panel1.Controls.Add(this.input);
            this.panel1.Controls.Add(this.deftrvidlbl);
            this.panel1.Controls.Add(this.trackvidlbl);
            this.panel1.Controls.Add(this.numvidlbl);
            this.panel1.Controls.Add(this.startlbl);
            this.panel1.Controls.Add(this.vidnumb);
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 113);
            this.panel1.TabIndex = 23;
            // 
            // languageBar1
            // 
            this.languageBar1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.languageBar1.AutoScroll = true;
            this.languageBar1.Enabled = false;
            this.languageBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.languageBar1.Location = new System.Drawing.Point(275, 3);
            this.languageBar1.Name = "languageBar1";
            this.languageBar1.SetDefault = 0;
            this.languageBar1.Setlbl = false;
            this.languageBar1.Size = new System.Drawing.Size(185, 28);
            this.languageBar1.TabIndex = 28;
            // 
            // edittrvideo
            // 
            this.edittrvideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.edittrvideo.AutoSize = true;
            this.edittrvideo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.edittrvideo.Location = new System.Drawing.Point(183, 10);
            this.edittrvideo.Name = "edittrvideo";
            this.edittrvideo.Size = new System.Drawing.Size(75, 17);
            this.edittrvideo.TabIndex = 27;
            this.edittrvideo.Text = "Edit Track";
            this.edittrvideo.UseVisualStyleBackColor = true;
            this.edittrvideo.CheckedChanged += new System.EventHandler(this.edittrvideo_CheckedChanged);
            // 
            // oneclicktrack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panel1);
            this.Name = "oneclicktrack";
            this.Size = new System.Drawing.Size(475, 113);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.CheckBox deftrvidlbl;
        public System.Windows.Forms.CheckBox trackvidlbl;
        public System.Windows.Forms.TextBox vidnumb;
        public System.Windows.Forms.Label startlbl;
        public System.Windows.Forms.CheckBox numvidlbl;
        public System.Windows.Forms.TextBox input;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox edittrvideo;
        public LanguageBar languageBar1;

    }
}
