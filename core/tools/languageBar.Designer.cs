namespace Sarkui
{ 
    partial class LanguageBar

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
        private void InitializeComponent()
        {
            this.language = new Sarkui.Main.CustomComboBox();
            this.languagelabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // language
            // 
            this.language.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.language.BackColor = System.Drawing.SystemColors.Window;
            this.language.Cursor = System.Windows.Forms.Cursors.Default;
            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language.ForeColor = System.Drawing.Color.Black;
            this.language.FormattingEnabled = true;
            this.language.HighlightColor = System.Drawing.Color.Pink;
            this.language.Location = new System.Drawing.Point(64, 5);
            this.language.Name = "language";
            this.language.Size = new System.Drawing.Size(245, 21);
            this.language.TabIndex = 0;
            this.language.SelectedIndexChanged += new System.EventHandler(this.language_SelectedIndexChanged);
            // 
            // languagelabel
            // 
            this.languagelabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.languagelabel.AutoSize = true;
            this.languagelabel.Location = new System.Drawing.Point(3, 0);
            this.languagelabel.Name = "languagelabel";
            this.languagelabel.Size = new System.Drawing.Size(55, 31);
            this.languagelabel.TabIndex = 1;
            this.languagelabel.Text = "Language";
            this.languagelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.languagelabel.Click += new System.EventHandler(this.languagelabel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.languagelabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.language, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(312, 31);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // LanguageBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LanguageBar";
            this.Size = new System.Drawing.Size(312, 31);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.Label languagelabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public Main.CustomComboBox language;
    }

}
