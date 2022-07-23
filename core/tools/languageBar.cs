

// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using Sarkui.core.gui;
//using Sarkui.core.plugins.interfaces;
using Sarkui.core.util;

namespace Sarkui
{
    public partial class LanguageBar : UserControl
    {


        public LanguageBar()
        {
             

             InitializeComponent();
             language.Items.AddRange(new List<string>(LanguageSelectionContainer.Languages.Keys).ToArray());
            //    cbEncodingMode.Items.AddRange(EnumProxy.CreateArray(OneClickSettings.SupportedModes));

         
        }

   

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Language
        {
            get { return language.Text; }
            set { language.SelectedItem = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    //    public bool UseFirstTrackOnly
    //    {
    //        get { return cbFirstTrackOnly.Checked; }
    //        set { cbFirstTrackOnly.Checked = value; }
   //     }


        private void language_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.Focus();

            if (language.Text.Equals("[default]"))
                language.SelectedItem = "";
            //            cbFirstTrackOnly.Text = "use only the first audio track";
            //        else
            //            cbFirstTrackOnly.Text = "use only the first " + language.Text + " audio track";


           



        }

         private protected int _lanvalue ;
  
        public int SetDefault
        {
            get { return _lanvalue; }
            set
            {
                language.Enabled = true;
   //            language.Items.Add("[default]");
                language.SelectedIndex = value;
            }
        }

        private void languagelabel_Click(object sender, EventArgs e)
        {

        }

        private protected bool _lanlbl;

        public bool Setlbl
        {
            get { return _lanlbl; }
            set
            {
                languagelabel.Visible = value;



            }
        }

    }


}