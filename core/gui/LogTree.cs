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
using System.IO;
using System.Windows.Forms;

using Sarkui.core.util;

namespace Sarkui.core.gui
{
    public partial class LogTree //: UserControl
    {
        private LogItem _oLog;

        public LogTree()
        {
        //    InitializeComponent();

            _oLog = new LogItem("Log", ImageType.NoImage);

            ImageList i = new ImageList();
            i.Images.Add(System.Drawing.SystemIcons.Error);
            i.Images.Add(System.Drawing.SystemIcons.Warning);
            i.Images.Add(System.Drawing.SystemIcons.Information);
        }

        public LogItem Log
        {
            get { return _oLog; }
        }

      

      


        

        private void EditLog_Click(object sender, EventArgs e)
        {
            Show(_oLog, true);
        }

    

        private void Show(LogItem l, bool subnodes)
         {
            if (l == null)
                return;

            TextViewer t = new TextViewer()
            {
                Contents = l.ToString(subnodes),
                Wrap = false
           };
            t.ShowDialog();
        }

      
       

 

        private void ExpandOrCollapseAll(LogItem i, bool expand)
        {
            if (expand)
                i.Expand();
            else
                i.Collapse();

            foreach (LogItem i2 in i.SubEvents)
                ExpandOrCollapseAll(i2, expand);
        }

        private void ExpandAll(LogItem i) { ExpandOrCollapseAll(i, true); }
        private void CollapseAll(LogItem i) { ExpandOrCollapseAll(i, false); }

    
   
    }
}