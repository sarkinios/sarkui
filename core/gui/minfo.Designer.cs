namespace Sarkui
{
    partial class minfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(minfo));
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.frm = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(-1, 1);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(318, 205);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            this.richTextBox2.TextChanged += new System.EventHandler(this.richTextBox2_TextChanged);
            // 
            // frm
            // 
            this.frm.FormattingEnabled = true;
            this.frm.Location = new System.Drawing.Point(218, 30);
            this.frm.Name = "frm";
            this.frm.Size = new System.Drawing.Size(120, 95);
            this.frm.TabIndex = 1;
            this.frm.Visible = false;
            // 
            // minfo
            // 
            this.ClientSize = new System.Drawing.Size(281, 201);
            this.Controls.Add(this.frm);
            this.Controls.Add(this.richTextBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "minfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MediaInfo";
            this.Load += new System.EventHandler(this.minfo_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ListBox frm;
    }
}



