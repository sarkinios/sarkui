namespace Sarkui
{
    partial class suinfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(suinfo));
            this.sfrm = new System.Windows.Forms.ListBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // sfrm
            // 
            this.sfrm.FormattingEnabled = true;
            this.sfrm.Location = new System.Drawing.Point(218, 30);
            this.sfrm.Name = "sfrm";
            this.sfrm.Size = new System.Drawing.Size(120, 95);
            this.sfrm.TabIndex = 2;
            this.sfrm.Visible = false;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(-1, -1);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(318, 205);
            this.richTextBox2.TabIndex = 3;
            this.richTextBox2.Text = "";
            // 
            // suinfo
            // 
            this.ClientSize = new System.Drawing.Size(281, 201);
            this.Controls.Add(this.sfrm);
            this.Controls.Add(this.richTextBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "suinfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MediaInfo";
            this.Load += new System.EventHandler(this.suinfo_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox sfrm;
        public System.Windows.Forms.RichTextBox richTextBox2;
    }
}



