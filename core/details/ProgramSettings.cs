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
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;




namespace Sarkui.core.details
{
    public class ProgramSettings
    {
        private bool _enabled, _required;
        private string _path;
        private DateTime _lastused;
        private string _name;
        private string _displayname;
        private List<string> _files;
        private List<string> _doNotDeleteFilesOnUpdate;
        private List<string> _doNotDeleteFoldersOnUpdate;


        public ProgramSettings(string name)
        {
            _files = new List<string>();
            _doNotDeleteFilesOnUpdate = new List<string>();
            _doNotDeleteFoldersOnUpdate = new List<string>();
            _enabled = _required = false;
            _path = String.Empty;
            _lastused = new DateTime();
            _name = _displayname = name;
            Main.Instance.ProgramSettings.Add(this);
        }

        public void UpdateInformation(string name, string displayname, string path)
        {
            _name = name;
            if (!String.IsNullOrEmpty(displayname))
                _displayname = displayname;
            if (String.IsNullOrEmpty(_displayname))
                _displayname = name;
            _path = path;
            _files.Add(path);
        }

        public bool Enabled
        {
            get
            {
                if (_required)
                    return true;
                return _enabled;
            }
            set { _enabled = value; }
        }

        public DateTime LastUsed
        {
            get { return _lastused; }
            set
            {
                _lastused = value;
            }
        }

        [XmlIgnore()]
        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        [XmlIgnore()]
        public string Path
        {
            get { return _path; }
        }

        [XmlIgnore()]
        public List<string> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        [XmlIgnore()]
        public List<string> DoNotDeleteFilesOnUpdate
        {
            get { return _doNotDeleteFilesOnUpdate; }
            set { _doNotDeleteFilesOnUpdate = value; }
        }

        [XmlIgnore()]
        public List<string> DoNotDeleteFoldersOnUpdate
        {
            get { return _doNotDeleteFoldersOnUpdate; }
            set { _doNotDeleteFoldersOnUpdate = value; }
        }

        [XmlIgnore()]
        public string Name
        {
            get { return _name; }
        }

        [XmlIgnore()]
        public string DisplayName
        {
            get { return _displayname; }
            set { _displayname = value; }
        }



  /*      public bool PackageToBeShown()
        {
            if (//(_name.Equals("dgindexim") && !MainForm.Instance.Settings.UseDGIndexIM) ||
             //   (_name.Equals("dgindexnv") && !MainForm.Instance.Settings.UseDGIndexNV) ||
             //   (_name.Equals("fdkaac") && !MainForm.Instance.Settings.UseFDKAac) ||
                (_name.Equals("ffmpeg") && !Main.Instance.Settings.UseFfmpeg) ||
                (_name.Equals("mkvpropedit") && !Main.Instance.Settings.UseMkvpropedit) ||
                (_name.Equals("mkvmerge") && !Main.Instance.Settings.UseMkvmerge))
             //   (_name.Equals("qaac") && !MainForm.Instance.Settings.UseQAAC))
            {
                _enabled = false;
                return false;
            }
            return true;
        }
        */


        private bool FilesAvailable()
        {
            foreach (String file in _files)
                if (!File.Exists(file))
                    return false;
            return true;
        }
    }
}