using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sarkui.core.gui;




namespace Sarkui
{


    
    
    public class MyLabel : Label
        {


       






        public static Label Set(string Text = "", Font Font = null, Color ForeColor = new Color(), Color BackColor = new Color())
            {
                Label l = new Label();
                l.Text = Text;
                l.Font = (Font == null) ? new Font("Calibri", 11) : Font;

                 l.AutoSize = true;
        //   l.Height = CalcLabelHeight(l);


            if (Properties.Settings.Default.Setting == true)
            {

                l.BackColor = Color.FromArgb(40, 42, 54);
                l.ForeColor = Color.FromArgb(189, 147, 249);
            }
            else
            {
                l.ForeColor = (ForeColor == new Color()) ? Color.Black : ForeColor;
                l.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
            }
            return l;
            }
        }
        public class MyButton : Button
        {
            public static Button Set(string Text = "", int Width = 70, int Height = 28, Font Font = null, Color ForeColor = new Color(), Color BackColor = new Color())
            {
                Button b = new Button();
                b.Text = Text;
                b.Width = Width;
                b.Height = Height;
                b.Font = (Font == null) ? new Font("Calibri", 12) : Font;
              //  b.ForeColor = (ForeColor == new Color()) ? Color.Black : ForeColor;
             //   b.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
                b.UseVisualStyleBackColor = (b.BackColor == SystemColors.Control);
                

            if (Properties.Settings.Default.Setting == true)
            {

                b.BackColor = Color.FromArgb(68 ,71, 90);
                b.ForeColor = Color.FromArgb(189, 147, 249);
            
            }
            return b;
        }
     }
  
        public partial class MyMessageBox : Form
        {


        public static int CalcLabelHeight(Label l)
        {
            Label lbl = new Label();
            Size sz = new Size(lbl.ClientSize.Width, Int32.MaxValue);
            sz = TextRenderer.MeasureText(lbl.Text, lbl.Font, sz, TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
            int height = sz.Height;
            if (height < lbl.Font.Height) height = lbl.Font.Height;
            return height;// + lbl.Padding.Vertical;
        }

        public MyMessageBox()
            {



                this.panText = new FlowLayoutPanel();
                this.panButtons = new FlowLayoutPanel();
                this.SuspendLayout();
                // 
                // panText
                // 
                this.panText.Parent = this;
                this.panText.AutoScroll = true;
                this.panText.AutoSize = true;
                this.panText.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.panText.Location = new Point(90, 90);
                this.panText.Margin = new Padding(0);
                this.panText.MaximumSize = new Size(500, 300);
            //    this.panText.MinimumSize = new Size(08, 50);
           //     this.panText.Size = new Size(108, 50);
                // 
                // panButtons
                // 
                this.panButtons.AutoSize = true;
                this.panButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.panButtons.FlowDirection = FlowDirection.RightToLeft;
                this.panButtons.Location = new Point(89, 89);
                this.panButtons.Margin = new Padding(0);
                this.panButtons.MaximumSize = new Size(200, 30);
                this.panButtons.MinimumSize = new Size(50, 0);
                this.panButtons.Size = new Size(108, 35);
                // 
                // MyMessageBox
                // 
           //     this.AutoScaleDimensions = new SizeF(8F, 19F);
                this.AutoScaleMode = AutoScaleMode.Font;
         //       this.ClientSize = new Size(206, 133);
               this.Controls.Add(this.panButtons);
                this.Controls.Add(this.panText);
                this.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.Margin = new Padding(4);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.MinimumSize = new Size(168, 132);
                this.Name = "MyMessageBox";
                this.ShowIcon = false;
                this.ShowInTaskbar = false;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.ResumeLayout(false);
                this.PerformLayout();
                this.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
                if (Properties.Settings.Default.Setting == true)
            {

                this.BackColor = Color.FromArgb(40, 42, 54);
            }
        }
        public static string Show(Label Label, string Title = "", List<Button> Buttons = null, PictureBox Image = null)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(Label);
                return Show(Labels, Title, Buttons, Image);
            }
            public static string Show(string Label, string Title = "", List<Button> Buttons = null, PictureBox Image = null)
            {
                List<Label> Labels = new List<Label>();
                Labels.Add(MyLabel.Set(Label));
                return Show(Labels, Title, Buttons, Image);
            }
            public static string Show(List<Label> Labels = null, string Title = "", List<Button> Buttons = null, PictureBox Image = null)
            {
                if (Labels == null) Labels = new List<Label>();
                if (Labels.Count == 0) Labels.Add(MyLabel.Set(""));
                if (Buttons == null) Buttons = new List<Button>();
                if (Buttons.Count == 0) Buttons.Add(MyButton.Set("OK"));
                List<Button> buttons = new List<Button>(Buttons);
                buttons.Reverse();

                int ImageWidth = 0;
                int ImageHeight = 0;
                int LabelWidth = 0;
                int LabelHeight = 10;
                int ButtonWidth = 0;
                int ButtonHeight = 0;
                int TotalWidth = 0;
                int TotalHeight = 0;

                MyMessageBox mb = new MyMessageBox();

                mb.Text = Title;

                //Image
                if (Image != null)
                {
                    mb.Controls.Add(Image);
                    Image.MaximumSize = new Size(50, 300);
                    ImageWidth = Image.Width + Image.Margin.Horizontal;
                    ImageHeight = Image.Height + Image.Margin.Vertical;
                }

                //Labels
                List<int> il = new List<int>();
                mb.panText.Location = new Point(14+ ImageWidth, 15);
                foreach (Label l in Labels)
                {
                    mb.panText.Controls.Add(l);
                    l.Location = new Point(10, 70);
              //      l.MaximumSize = new Size(10, 80);
                    il.Add(l.Width);
           //     l.Height = CalcLabelHeight(l);
               
            }
            //    int mw = Labels.Max(x => x.Width);
                il.ToString();
           //     Labels.ForEach(l => l.MinimumSize = new Size(Labels.Max(x => x.Width+150), 20));
                
                mb.panText.Height = Labels.Sum(l => l.Height);
                mb.panText.MinimumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), ImageHeight);
          //      mb.panText.MaximumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), 600);
                LabelWidth = mb.panText.Width;
            LabelHeight = mb.panText.Height;

                //Buttons
                foreach (Button b in buttons)
                {
                    mb.panButtons.Controls.Add(b);
                    b.Location = new Point(0, 0);
         //           b.TabIndex = Buttons.FindIndex(i => i.Text == b.Text);
                    b.Click += new EventHandler(mb.Button_Click);
                }
                ButtonWidth = mb.panButtons.Width;
                ButtonHeight = mb.panButtons.Height;

                //Set Widths
                if (ButtonWidth > ImageWidth + LabelWidth)
                {
            //        Labels.ForEach(l => l.MinimumSize = new Size(ButtonWidth - ImageWidth - mb.ScrollBarWidth(Labels), 1));
           //         mb.panText.Height = Labels.Sum(l => l.Height);
          //          mb.panText.MinimumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), ImageHeight);
            //        mb.panText.MaximumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), 300);
            //        LabelWidth = mb.panText.Width;
             //       LabelHeight = mb.panText.Height;
                }
                TotalWidth = ImageWidth + LabelWidth;

                //Set Height
                TotalHeight = LabelHeight + ButtonHeight;

                mb.panButtons.Location = new Point(TotalWidth/2 -25, TotalHeight +2);

                mb.Size = new Size(TotalWidth + 35, TotalHeight + 37);
                mb.ShowDialog();
                return mb.Result;
            }

            private FlowLayoutPanel panText;
            private FlowLayoutPanel panButtons;
            private int ScrollBarWidth(List<Label> Labels)
            {
                return (Labels.Sum(l => l.Height) > 300) ? 23 : 6;
            }

            private void Button_Click(object sender, EventArgs e)
            {
                Result = ((Button)sender).Text;
                Close();
            }

            private string Result = "";
        }
    }
