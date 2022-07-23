using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
//using Sarkui.core.gui;
//using Sarkui.core.details;
//using Sarkui.core.util;

namespace Sarkui
{
    public partial class SettingsF : Form
    {
      
        #region variables
        /*      private SarkSettings internalSettings = new SarkSettings();
              //    private XmlDocument ContextHelp = new XmlDocument();
              //    private AutoEncodeDefaultsSettings autoEncodeDefaults;
              //   private SourceDetectorSettings sdSettings;
              #endregion
        */


        #region variables
        private SarkSettings internalSettings = new SarkSettings();
        private SarkSettings settings;

        //edw na tsekarw
        public SarkSettings GetSettings()
        {
            return settings;
        }

        //edw na tsekarw
        internal void SetSettings(SarkSettings value)
        {
            settings = value;
        }



        #endregion

        public SettingsF()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //Sarkui.app


           
           
        }


        #region properties

        public SarkSettings Settings
        {
            get
            {
                SarkSettings settings = internalSettings;
                //          settings.AcceptableFPSError = acceptableFPSError.Value;

                //          settings.SourceDetectorSettings = sdSettings;
                settings.ffmpegPath = FfmpegLocation.Filename;
                settings.mkvpropPath = MkvpropLocation.Filename;
                settings.mkvmergePath = MkvmergeLocation.Filename;
                settings.mkvextPath = MkvextLocation.Filename;
                settings.mp4boxPath = Mp4boxLocation.Filename;
                settings.soxPath = SoxLocation.Filename;
  
                if (useFfmpeg.Checked != internalSettings.UseFfmpeg)
                    //                       UpdateCacher.CheckPackage("neroaacenc", useNeroAacEnc.Checked, false);
                    settings.UseFfmpeg = useFfmpeg.Checked;

                if (useMkvpropedit.Checked != internalSettings.UseMkvpropedit)
                    settings.UseMkvpropedit = useMkvpropedit.Checked;

                if (useMkvmerge.Checked != internalSettings.UseMkvmerge)
                    settings.UseMkvmerge = useMkvmerge.Checked;

                if (useMkvext.Checked != internalSettings.UseMkvext)
                    settings.UseMkvext = useMkvext.Checked;


                if (useMp4box.Checked != internalSettings.UseMp4box)
                    settings.UseMp4box = useMp4box.Checked;

                if (useSox.Checked != internalSettings.UseSox)
                    settings.UseSox = useSox.Checked;


                /*
                                       if (useFDKAac.Checked != internalSettings.UseFDKAac)
                                           UpdateCacher.CheckPackage("fdkaac", useFDKAac.Checked, false);
                                       settings.UseFDKAac = useFDKAac.Checked;

                                       if (useQAAC.Checked != internalSettings.UseQAAC)
                                           UpdateCacher.CheckPackage("qaac", useQAAC.Checked, false);
                                       settings.UseQAAC = useQAAC.Checked;

                                       settings.Input8Bit = chkInput8Bit.Checked;
                                       settings.UseExternalMuxerX264 = chx264ExternalMuxer.Checked;
                                       settings.AlwaysUsePortableAviSynth = cbUseIncludedAviSynth.Checked;
                                       settings.ChapterCreatorMinimumLength = (int)minimumTitleLength.Value;
                                       settings.Usex64Tools = chk64Bit.Checked;
                                       settings.ShowDebugInformation = chkDebugInformation.Checked;
                                       settings.EnableDirectShowSource = chkDirectShowSource.Checked;

                                       EnumProxy o = cbStandbySettings.SelectedItem as EnumProxy;
                                      settings.StandbySetting = (SarkSettings.StandbySettings)o.RealValue;

                       */
                return settings;
            }
            set
            {
                internalSettings = value;
                SarkSettings settings = value;
                /*                       acceptableFPSError.Value = settings.AcceptableFPSError;
                       */      //          useAutoUpdateCheckbox.Checked = settings.AutoUpdate;
                FfmpegLocation.Filename = settings.ffmpegPath;
                MkvpropLocation.Filename = settings.mkvpropPath;
                MkvmergeLocation.Filename = settings.mkvmergePath;
                MkvextLocation.Filename = settings.mkvextPath;
                Mp4boxLocation.Filename = settings.mp4boxPath;
                SoxLocation.Filename = settings.soxPath;
           
                useFfmpeg.Checked = settings.UseFfmpeg;
                useMkvpropedit.Checked = settings.UseMkvpropedit;
                useMkvmerge.Checked = settings.UseMkvmerge;
                useMkvext.Checked = settings.UseMkvext;
                useMp4box.Checked = settings.UseMp4box;
                useSox.Checked = settings.UseSox;
                /*               useFDKAac.Checked = settings.UseFDKAac;
                                useQAAC.Checked = settings.UseQAAC;
                                chx264ExternalMuxer.Checked = settings.UseExternalMuxerX264;
                                cbUseIncludedAviSynth.Checked = settings.AlwaysUsePortableAviSynth;
                                minimumTitleLength.Value = settings.ChapterCreatorMinimumLength;
                                chk64Bit.Checked = settings.Usex64Tools;
                                chkDebugInformation.Checked = settings.ShowDebugInformation;
                                chkDirectShowSource.Checked = settings.EnableDirectShowSource;
                                chkInput8Bit.Checked = settings.Input8Bit;
              */
                //         cbStandbySettings.SelectedItem = EnumProxy.Create(Main.Instance.Settings.StandbySetting);
            }
        }


        #endregion
        #endregion

        /////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////
       

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void fileBar1_Load(object sender, EventArgs e)
        {

        }

        private void useFfmpeg_CheckedChanged(object sender, EventArgs e)
        {
            FfmpegLocation.Enabled = lblffmpeg.Enabled = useFfmpeg.Checked;
            //      if (useFfmpeg.Checked && !internalSettings.UseFfmpeg)
            //          MessageBox.Show("Restart SarkUI in order to get access to Ffmpeg.\r\n", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (useFfmpeg.Checked == false)
            {
                FfmpegLocation.filename.Text = "";

            }


        }


        private void useMkvpropedit_CheckedChanged(object sender, EventArgs e)
        {
            MkvpropLocation.Enabled = lblmkvprop.Enabled = useMkvpropedit.Checked;
            //      if (useMkvpropedit.Checked && !internalSettings.UseFfmpeg)
            //         MessageBox.Show("Restart SarkUI in order to get access to Ffmpeg.\r\n", "Restart required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (useMkvpropedit.Checked == false)
            {
                MkvpropLocation.filename.Text = "";

            }


        }

        private void useMkvmerge_CheckedChanged(object sender, EventArgs e)
        {
            MkvmergeLocation.Enabled = lblmkvmerge.Enabled = useMkvmerge.Checked;
            if (useMkvmerge.Checked == false)
            {
                MkvmergeLocation.filename.Text = "";

            }
        }
        private void useMkvext_CheckedChanged(object sender, EventArgs e)
        {
            MkvextLocation.Enabled = lblmkvext.Enabled = useMkvext.Checked;
            if (useMkvext.Checked == false)
            {
                MkvextLocation.filename.Text = "";

            }
        }
        private void useMp4box_CheckedChanged(object sender, EventArgs e)
        {
            Mp4boxLocation.Enabled = lblmp4box.Enabled = useMp4box.Checked;
            if (useMp4box.Checked == false)
            {
                Mp4boxLocation.filename.Text = "";

            }
        }
        private void useSox_CheckedChanged(object sender, EventArgs e)
        {
            SoxLocation.Enabled = lblsox.Enabled = useSox.Checked;
            if (useSox.Checked == false)
            {
                SoxLocation.filename.Text = "";

            }
        }
        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }

      

        private void MkvmergeLocation_Load(object sender, EventArgs e)
        {

        }





        private void flowlayback_Paint(object sender, PaintEventArgs e)
        {

        }

        public Boolean nameOfControlVisible
        {
            get { return this.flowLayoutPanel1.Visible; }
            set { this.flowLayoutPanel1.Visible = value; }
        }

        
    }








}
