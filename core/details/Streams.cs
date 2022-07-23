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
using System.Text;

using Sarkui.core.util;

namespace Sarkui.core.details
{
    public class MuxStream
    {
        private string _language;
        private string _name;
        public string path;
        public int delay;
        public bool bDefaultTrack;
        public bool bForceTrack;
        //     public TrackInfo MuxOnlyInfo;
        public string _numbering;
        public bool bEdittr;

        public MuxStream(string path, string language, string name, int delay, bool bDefaultTrack, bool bForceTrack, string numbering, bool bEdittr)//, TrackInfo MuxOnlyInfo)
        {
            this._language = language;
            this._name = name;
            this.path = path;
            this.delay = delay;
            this.bDefaultTrack = bDefaultTrack;
            this.bForceTrack = bForceTrack;
            //          this.MuxOnlyInfo = MuxOnlyInfo;
            this._numbering = numbering;
            this.bEdittr = bEdittr;
        }

        public MuxStream() : this(null, null, null, 0, false, false,null, false) { }

/*
        public LanguageBar lang
        {
            get { return lang; }
            set { this._language = value.lang; }
        }

*/
        
                public string language
                {
                    get
                    {
                        return _language;
                    }
                    set
                    {
                        _language = value;
                    }
                }
        
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string numbering
        {
            get
            {
                return _numbering;

            }
            set
            {
                _numbering = value;
            }
        }

    /*    public string bEdittr
        {
            get
            {
                return bEdittr;

            }
            set
            {
                bEdittr = value;
            }
        }
    */


    }

 //   public class BitrateCalculationStream
//    {
//        public BitrateCalculationStream(string filename)
//        {
//            this.Filename = filename;
//            if (Filename != null) fillInfo();
//        }

 //       public string Filename;
//        public OutputType Type;
//
 //       public virtual void fillInfo()
 //       {
 //           Size = FileSize.Of2(Filename);
 //       }

 //   }

 //   public class AudioBitrateCalculationStream : BitrateCalculationStream
 //   {
 //       public AudioBitrateCalculationStream() : this(null) { }

//        public AudioBitrateCalculationStream(string filename)
 //           : base(filename) { }
//

//        public AudioType AType;

 //       public override void fillInfo()
 //       {
 //           base.fillInfo();
 //           Type = AType = VideoUtil.guessAudioType(Filename);
 //       }
 //   }
}

