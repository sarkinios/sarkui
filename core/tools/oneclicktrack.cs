using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sarkui.core.util;
using Sarkui.core.details;
using Sarkui.core.gui;

namespace Sarkui
{
    public partial class oneclicktrack : UserControl
    {

        public int ControlId { get; set; }

        public oneclicktrack()
        {
            InitializeComponent();

        }

        private protected bool _val;
        
        public bool visibar
        {
            get { return _val; }
            set
            {
                this.languageBar1.Visible = value;

            }
        }
        private protected int _lanvalue;
        public int setlang
        {
            get { return _lanvalue; }
            set
            {
                languageBar1.SetDefault = value;

            }
        }

        public MuxStream Stream
        {
            get
            {
                string metaName;
                metaName = input.Text;
                bool bDefault = false;
                bDefault = deftrvidlbl.Checked;
                string numbering;
                numbering = vidnumb.Text;
                string lang;
                lang = languageBar1.Language;
                bool bEdittr = false;
                bEdittr = edittrvideo.Checked;

                return new MuxStream(null, lang , metaName , 0, bDefault, false, numbering, bEdittr);
            }
            set
            {
                
                languageBar1.Language = value.language;
                input.Text = value.name;
                //   audioDelay.Value = value.delay;
                deftrvidlbl.Checked = value.bDefaultTrack;
                //   chkForceStream.Checked = value.bForceTrack;
                //   _trackInfo = value.MuxOnlyInfo;
                vidnumb.Text = value.numbering;
                edittrvideo.Checked = value.bEdittr;

            }

        }

       



public void edittrvideo_CheckedChanged(object sender, EventArgs e)
        {
            languageBar1.Enabled = trackvidlbl.Enabled  = deftrvidlbl.Enabled = edittrvideo.Checked;

           

        }

        public void trackvidlbl_CheckedChanged(object sender, EventArgs e)
        {
            input.Enabled = numvidlbl.Enabled = trackvidlbl.Checked;

        }

        public void numvidlbl_CheckedChanged(object sender, EventArgs e)
        {
            startlbl.Enabled = vidnumb.Enabled = numvidlbl.Checked;
        }

        public void deftrvidlbl_CheckedChanged(object sender, EventArgs e)
        {

        }

      

        private void vidnumb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                  (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void vidnumb_TextChanged(object sender, EventArgs e)
        {

        }

    }
   
    

}
    

    
















    
   
