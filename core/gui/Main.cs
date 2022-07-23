using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using Sarkui.core.details;
using Sarkui.core.gui;
//using Sarkui.core.plugins.interfaces;
using Sarkui.core.util;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.FileIO;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace Sarkui
{
    public partial class Main : Form
    {
        List<Control> panels;
        List<Control> buttons;
        List<Control> textboxes;
        List<Control> listboxes;
        List<Control> tabsc;
        List<Control> difcol;
        List<Control> inputs;
        List<Control> cboxes;

        // This instance is to be used by the serializers that can't be passed a MainForm as a parameter
        public static Main Instance;

        #region variable declaration
        public List<oneclicktrack> metavideotracks;
        public List<oneclicktrack> metaaudiotracks;
        public List<oneclicktrack> metasubtracks;


        private List<string> filesToDeleteOnClosing = new List<string>();
        private List<Form> allForms = new List<Form>();
        private List<Form> formsToReopen = new List<Form>();
        // private ITaskbarList3 taskbarItem;
        //  private Icon taskbarIcon;
        private List<ProgramSettings> _programsettings;
        private SarkSettings settings = new SarkSettings();
        private string path; // path the program was started from
                             //     public bool IsOverlayIconActive { get { return taskbarIcon != null; } }
        private CodecManager codecs;

        public List<ProgramSettings> ProgramSettings { get { return _programsettings; } set { _programsettings = value; } }

        private LogItem _oneClickLog;
        private LogItem _autoEncodeLog;
        private LogItem _aVSScriptCreatorLog;
        private LogItem _fileIndexerLog;
        private LogItem _eac3toLog;
        private LogItem _avisynthWrapperLog;
        private LogItem _mediaInfoWrapperLog;
        private LogItem _oTempLog = new LogItem("Log", ImageType.NoImage);
        public LogItem OneClickLog { get { return _oneClickLog; } set { _oneClickLog = value; } }
        public LogItem AutoEncodeLog { get { return _autoEncodeLog; } set { _autoEncodeLog = value; } }
        public LogItem AVSScriptCreatorLog { get { return _aVSScriptCreatorLog; } set { _aVSScriptCreatorLog = value; } }
        public LogItem FileIndexerLog { get { return _fileIndexerLog; } set { _fileIndexerLog = value; } }
        public LogItem Eac3toLog { get { return _eac3toLog; } set { _eac3toLog = value; } }
        public LogItem AviSynthWrapperLog { get { return _avisynthWrapperLog; } set { _avisynthWrapperLog = value; } }
        public LogItem MediaInfoWrapperLog { get { return _mediaInfoWrapperLog; } set { _mediaInfoWrapperLog = value; } }


        #endregion


        public void DeleteOnClosing(string file)
        {
            filesToDeleteOnClosing.Add(file);
        }


        public Main()
        {

            this.MaximizeBox = false;

            string strSarkTempPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\sarktemp";
            FileUtil.ensureDirectoryExists(strSarkTempPath);
            string sarktools = Path.GetDirectoryName(Application.ExecutablePath) + @"\sarktools";
            FileUtil.ensureDirectoryExists(sarktools);


            // Log File Handling
            //    string strSarkLogPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\logs";
            //         FileUtil.ensureDirectoryExists(strSarkLogPath);
            //             strLogFile = strSarkLogPath + @"\logfile-" + DateTime.Now.ToString("yy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".log";
            //             FileUtil.WriteToFile(strLogFile, "Preliminary log file only. During closing of SarkUI the well formed log file will be written.\r\n\r\n", false);
            Instance = this;
            constructSarkInfo();
            InitializeComponent();
            InitializeOpenFileDialog();
            //   logTree1.SetLog(_oTempLog);
            System.Reflection.Assembly myAssembly = this.GetType().Assembly;
            string name = this.GetType().Namespace + ".";



        }




        #region job management
        #region I/O verification
        /// <summary>
        /// Test whether a filename is suitable for writing to
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Error message if problem, null if ok</returns>
        public static string verifyOutputFile(string filename)
        {
            try
            {
                filename = Path.GetFullPath(filename);  // this will throw ArgumentException if invalid
                if (File.Exists(filename))
                {
                    FileStream fs = File.OpenWrite(filename);  // this will throw if we'll have problems writing
                    fs.Close();
                }
                else
                {
                    FileStream fs = File.Create(filename);  // this will throw if we'll have problems writing
                    fs.Close();
                    File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }

        /// <summary>
        /// Test whether a filename is suitable for reading from
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Error message if problem, null if ok</returns>
        public static string verifyInputFile(string filename)
        {
            try
            {
                filename = Path.GetFullPath(filename);  // this will throw ArgumentException if invalid
                FileStream fs = File.OpenRead(filename);  // this will throw if we'll have problems reading
                fs.Close();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
        #endregion
        #endregion

        #region settings load save settings
        /// <summary>
        /// saves the global GUI settings to settings.xml
        /// </summary>
        public void saveSettings()
        {
            XmlSerializer ser = null;
            string fileName = this.path + @"\lib\settings.xml";
            using (Stream s = File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    ser = new XmlSerializer(typeof(SarkSettings));
                    ser.Serialize(s, this.settings);
                }
                catch (Exception ex)
                {
                    LogItem _oLog = Main.Instance.Log.Info("Error");
                    _oLog.LogValue("saveSettings", ex, ImageType.Error);
                }
            }
        }
        /// <summary>
        /// loads the global settings
        /// </summary>
        public void loadSettings()
        {
            this._programsettings = new List<ProgramSettings>();
            string fileName = Path.Combine(path, @"lib\settings.xml");
            if (File.Exists(fileName))
            {
                XmlSerializer ser = null;
                using (Stream s = File.OpenRead(fileName))
                {
                    ser = new XmlSerializer(typeof(SarkSettings));
                    try
                    {
                        this.settings = (SarkSettings)ser.Deserialize(s);
                    }
                    catch
                    {
                        MessageBox.Show("SarkUI settings could not be loaded. Default values will be applied now.", "Error loading Sarkui settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            Main.Instance.Settings.InitializeProgramSettings();
        }

        #endregion


        #region GUI updates

        #region helper methods





        LogTree logTree1 = new LogTree();


        public LogItem Log
        {

            get
            {
                if (logTree1 == null)
                    return _oTempLog;
                return logTree1.Log;
            }
        }

        /// <summary>
        /// saves the whole content of the log into a logfile
        /// </summary>




        /// <summary>
        /// returns the profile manager to whomever might require it
        /// </summary>


        #endregion


        #region Sarkinfo

        #region start and end

        /// <summary>
        /// default constructor
        /// initializes all the GUI components, initializes the internal objects and makes a default selection for all the GUI dropdowns
        /// In addition, all the jobs and profiles are being loaded from the harddisk
        /// </summary>
        public void constructSarkInfo()
        {
            //    this.muxProvider = new MuxProvider(this);
            this.codecs = new CodecManager();
            this.path = System.Windows.Forms.Application.StartupPath;
            //   this.addPackages();
            //    this.profileManager = new ProfileManager(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "allprofiles"));
            //    this.profileManager.LoadProfiles();
            //    this.mediaFileFactory = new MediaFileFactory(this);
            this.loadSettings();
            //    Main.Instance.settings.DPIRescaleAll();
            //   this.dialogManager = new DialogManager(this);
        }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Mains(string[] args)
        {
            // prevent another instance of Sarkui from the same location
            int iCount = 0;
            foreach (Process oProc in Process.GetProcessesByName(Application.ProductName))
            {
                try
                {
                    if (Application.ExecutablePath.Equals(oProc.MainModule.FileName))
                        iCount++;
                }
                catch { }
            }
            if (iCount > 1)
            {
                MessageBox.Show("There is already another instance of the application running.", "Sarkui Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check if the program can write to the program dir
            if (!FileUtil.IsDirWriteable(Path.GetDirectoryName(Application.ExecutablePath)))
            {
                // parse if the program has already been started elevated
                Boolean bRunElevated = false;
                foreach (string strParam in args)
                {
                    if (strParam.Equals("-elevate"))
                    {
                        bRunElevated = true;
                        break;
                    }
                }

                // if needed run as elevated process
                if (!bRunElevated)
                {
                    try
                    {
                        Process p = new Process();
                        p.StartInfo.FileName = Application.ExecutablePath;
                        p.StartInfo.Arguments = "-elevate";
                        p.StartInfo.Verb = "runas";
                        p.Start();
                        return;
                    }
                    catch
                    {
                    }
                }

                MessageBox.Show("Sarkui cannot be started as it cannot write to the application directory.\rPlease grant the required permissions or move the application to an unprotected directory.", "Sarkui Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

#if !DEBUG
            // catch uncatched errors
            //           Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //           AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif

            // delete PDB file if outdated
            try
            {
                string strDebugFile = Path.ChangeExtension(Application.ExecutablePath, ".pdb");
                if (File.Exists(strDebugFile))
                {
                    DateTime creationAPP = File.GetLastWriteTime(Application.ExecutablePath);
                    DateTime creationPDB = File.GetLastWriteTime(strDebugFile);
                    double difference = (creationAPP < creationPDB) ? (creationPDB - creationAPP).TotalSeconds : (creationAPP - creationPDB).TotalSeconds;
                    if (difference > 360)
                        File.Delete(strDebugFile);
                }
            }
            catch { }

            //       Application.EnableVisualStyles();



            Main mainForm = new Main();

            // start Sarkui form if not blocked
            bool bStart = true;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--dont-start")
                    bStart = false;
            }
            if (bStart)
                Application.Run(mainForm);
        }


        #endregion


        #endregion



        #region properties
        /// <summary>
        /// gets the path from where Sarkui was launched
        /// </summary>
        public string SarkuiPath
        {
            get { return this.path; }
        }
        #endregion

        #endregion



        public SarkSettings Settings
        {
            get { return settings; }
        }

        protected SettingsF SettingsF;


        // MAIN LOAD

        #region Main LOAD 
        void Main_Load(object sender, EventArgs e)
        {

            /*
                        void Main(SettingsF SettingsF)
                        {
                            this.SettingsF = SettingsF;
                        }
            */

            if (Properties.Settings.Default.radioSetting == 4)
            {
                radioButtonList1.SetSelected(0, true);
                chmkv.Checked = false;
            }
            else
            {
                radioButtonList1.SetSelected(Properties.Settings.Default.radioSetting, true);

            }

            if (Properties.Settings.Default.radioSetting == 0)
            {
                //        checkmkv.OnColour = Color.FromArgb(255, 85, 85);
                //         checkmkv.Checked = true;
                chmkv.Checked = true;
                chmkv.Select();
                chmp4.Checked = chts.Checked = chavi.Checked = chaud.Checked = false;

            }
            else if (Properties.Settings.Default.radioSetting == 1)
            {
                //      checkmp4.OnColour = Color.FromArgb(255, 85, 85);
                //        checkmp4.Checked = true;
                chmp4.Checked = true;
                chmp4.Select();
                chmkv.Checked = chts.Checked = chavi.Checked = chaud.Checked = false;

            }
            else if (Properties.Settings.Default.radioSetting == 2)
            {
                //        checkts.OnColour = Color.FromArgb(255, 85, 85);
                //         checkts.Checked = true;
                chts.Checked = true;
                chmkv.Checked = chmp4.Checked = chavi.Checked = chaud.Checked = false;

            }
            else if (Properties.Settings.Default.radioSetting == 3)
            {
                //      checkavi.OnColour = Color.FromArgb(255, 85, 85);
                //         checkavi.Checked = true;
                chavi.Checked = true;
                chavi.Select();
                chmkv.Checked = chmp4.Checked = chts.Checked = chaud.Checked = false;

            }
            else if (Properties.Settings.Default.radioSetting == 4)
            {
                chaud.Checked = true;
                chaud.Select();
                chmkv.Checked = chmp4.Checked = chts.Checked = chavi.Checked = false;

            }




            Initialize_Add();

            this.tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            this.tabControl1.Click += tabControl1_Click;
            this.tabControl2.Click += tabControl1_Click;

            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl2.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.tabControl2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl2_DrawItem);






            metavideotracks = new List<oneclicktrack>();  //neag
            metaaudiotracks = new List<oneclicktrack>();
            metasubtracks = new List<oneclicktrack>();
            //    metadatatracks.Add(oneclicktrack1);          //neag

            comboBoxvideotrack.Items.Add("Video Track 01");
            comboBoxvideotrack.SelectedIndex = 0;
            comboBoxaudiotrack.Items.Add("Audio Track 01");
            comboBoxaudiotrack.SelectedIndex = 0;
            comboBoxsubtrack.Items.Add("Sub Track 01");
            comboBoxsubtrack.SelectedIndex = 0;
            oneclicktrack a = new oneclicktrack();
            a.Name = "0";
            oneclicktrack1.Controls.Add(a);
            metavideotracks.Add(a);
            a.visibar = true;
            a.setlang = 1;
            a.deftrvidlbl.Checked = true;
            a.BringToFront();





            inputs.Add(a.input);
            listboxes.Add(a.languageBar1.language);
            inputs.Add(a.vidnumb);



            oneclicktrack b = new oneclicktrack();
            b.Name = "0";
            oneclicktrack2.Controls.Add(b);
            metaaudiotracks.Add(b);
            b.BringToFront();
            b.deftrvidlbl.Checked = true;
            b.visibar = true;
            b.setlang = 0;


            inputs.Add(b.input);
            listboxes.Add(b.languageBar1.language);
            inputs.Add(b.vidnumb);

            oneclicktrack c = new oneclicktrack();
            c.Name = "0";
            oneclicktrack3.Controls.Add(c);
            metasubtracks.Add(c);
            c.visibar = true;
            c.setlang = 0;
            c.deftrvidlbl.Checked = true;
            c.BringToFront();

            //      languageBar1.Visible = true;

            inputs.Add(c.input);
            listboxes.Add(c.languageBar1.language);
            inputs.Add(c.vidnumb);



            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 1000;
            toolTip1.InitialDelay = 1200;
            toolTip1.ReshowDelay = 800;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.grpBoxTitle, "Change the Title Name in Media Info (metadata)");
            toolTip1.SetToolTip(grpBoxVideo, "Change the Video Name in Media Info (metadata)");
            toolTip1.SetToolTip(grpBoxAudio, "Change the Audio Name in Media Info (metadata)");




            //      radioButtonList1.SetSelected(0, true);
            this.radioButtonList1.Enabled = false;
            listBox1.Items.Clear();
            radioButtonList2.SetSelected(0, true);


            tabControl2.SizeMode = TabSizeMode.Fixed;
            tabControl2.DrawMode = TabDrawMode.OwnerDrawFixed;








            //languageBar2.language.SelectedIndex = 112;
            //languageBar3.language.SelectedIndex = 84;
            languageBar2.language.SelectedIndex = 0;
            languageBar3.language.SelectedIndex = 0;


            bunifuiOSSwitch1.Value = Properties.Settings.Default.Setting;

            audiobox.SelectedIndex = 0;

            //           audiobox.DropDown += new EventHandler(audiobox_DropDown);
            //         audiobox.DropDownClosed += new EventHandler(audiobox_DropDownClosed);

            languageBar3.Setlbl = false;












            //instatiate Bunifu Radio Button using Bunifu.UI.WinForms;
            //     Bunifu.UI.WinForms.BunifuRadioButton bunifuRadio = new Bunifu.UI.WinForms.BunifuRadioButton();
            //set the location for the radio button
            //   chmkv.Location = new System.Drawing.Point(40, 10);
            //  chmkv.Margin = new System.Windows.Forms.Padding(40, 10, 20, 3);

            //instatiate a Bunifu Label to bind to the radio button
            //       lbl1 bunifuLabel = new BunifuLabel();
            //set the text for the label
            //      bunifuLabel.Text = "Option one";
            //bind the label to Bunifu Radio
            //    chmkv.BindingControl = lbl1;
            //    bunifuRadio.AllowBindingControlLocation = true;
            //add the two controls to the form
            //       this.Controls.AddRange(new Control[] { chmkv, lbl1 });










            //  bunifuHSlider1.MouseClick -= bunifuHSlider1_MouseClick;

            trackBar1.Value = 128;
            textBox1.Text = trackBar1.Value.ToString();



            audioext.SelectedIndex = 0;



        }

        #endregion

























        //TABS


        #region OPTIONS TAB




        List<Control> ControlList = new List<Control>();
        private void GetAllControls(SettingsF form, Control container)
        {
            foreach (Control c in form.Controls)
            {
                GetAllControls(form, c);
                if (c is GroupBox)
                {
                    ControlList.Add(c);
                    MessageBox.Show(c.Name.ToString());
                }
            }
        }




        private void mnuOptions_Click(object sender, EventArgs e)
        {






            //Display Another Form On Button Click
            // KENTRIZW TO PARATHIRO TWN SETTINGS SE SXESH ME TO ARXIKO PARATHIRO

            #region center_window_app
            using (SettingsF sform = new SettingsF())
            {
                if (bunifuiOSSwitch1.Value == true)
                {

                    // listboxes.Add(sform.lblmkvmerge);
                    buttons.Add(sform.MkvmergeLocation);
                    buttons.Add(sform.FfmpegLocation);
                    buttons.Add(sform.Mp4boxLocation);
                    buttons.Add(sform.MkvpropLocation);
                    buttons.Add(sform.MkvextLocation);
                    buttons.Add(sform.MkvmergeLocation);
                    buttons.Add(sform.SoxLocation);
                    inputs.Add(sform.FfmpegLocation.filename);
                    inputs.Add(sform.Mp4boxLocation.filename);
                    inputs.Add(sform.MkvpropLocation.filename);
                    inputs.Add(sform.MkvextLocation.filename);
                    inputs.Add(sform.SoxLocation.filename);
                    inputs.Add(sform.MkvmergeLocation.filename);
                    panels.Add(sform.flowback);
                    panels.Add(sform.flowLayoutPanel1);
                    textboxes.Add(sform.groupBox1);
                    textboxes.Add(sform.groupBox2);
                    textboxes.Add(sform.groupBox3);
                    textboxes.Add(sform.groupBox4);
                    textboxes.Add(sform.groupBox5);
                    textboxes.Add(sform.groupBox6);
                    buttons.Add(sform.cancelButton);
                    buttons.Add(sform.saveButton);
                    //    textboxes.Add(SettingsF.FfmpegLocation);
                    //   textboxes.Add(SettingsF.flowback);
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);

                }
                sform.Settings = this.settings;
                sform.StartPosition = FormStartPosition.CenterScreen;

                if (sform.ShowDialog() == DialogResult.OK)
                {
                    SarkSettings.StandbySettings oldStandbyValue = this.settings.StandbySetting;

                    this.settings = sform.Settings;
                    this.saveSettings();


                }
            }
            #endregion
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }



        private void mnuInfo_Click(object sender, EventArgs e)
        {
            using (Info sform = new Info())
            {


                if (bunifuiOSSwitch1.Value == true)
                {

                    // listboxes.Add(sform.lblmkvmerge);
                    buttons.Add(sform.label1);

                    textboxes.Add(sform.groupBox1);
                    textboxes.Add(sform.panel1);
                    var img = resizeImage(Sarkui.Properties.Resources.grave_Image2, new Size(156, 83));
              //      sform.pictureBox1.Image = ((System.Drawing.Image)(img));
              //      sform.pictureBox1.Size = new System.Drawing.Size(156, 83);


                    //    textboxes.Add(SettingsF.FfmpegLocation);
                    //   textboxes.Add(SettingsF.flowback);
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);
                    //      SettingsF.MkvmergeLocation.BackColor = Color.Black;

                }
                sform.StartPosition = FormStartPosition.CenterScreen;
                sform.ShowDialog();


                /*       if (sform.ShowDialog() == DialogResult.OK)
                       {
                           SarkSettings.StandbySettings oldStandbyValue = this.settings.StandbySetting;

                           this.settings = sform.Settings;
                           this.saveSettings();
                       }
                */
            }
        }

        #endregion




        // RADIOBUTTONS

        #region RADIOBUTTONS

        private void checkmkv_CheckedChanged(object sender, EventArgs e)
        {
            //     checkmkv.OnColour = Color.FromArgb(255, 85, 85);
            listBox1.Items.Clear();
            hide.Visible = true;
            hide1.Visible = hide2.Visible = false;
            oneclickconverter.Enabled = grpBoxTitle.Enabled = grpBoxAudio.Enabled = grpBoxVideo.Enabled = groupBox1.Enabled = grpBoxSub.Enabled = groupBox3.Enabled = audiomuxpanel.Enabled = true;
            //     groupBox4.Enabled = false;
            languageBar3.Enabled = expaudiobut.Enabled = true;
            audiobox.Enabled = trackBar1.Enabled = convaudio.Enabled = audioext.Enabled = false;
            oneclickconverter.Enabled = radioButtonList2.Enabled = label10.Enabled = Overwrite2.Enabled = label9.Enabled = Overwrite.Enabled = sortsub.Enabled = sortaud.Enabled = true;
            radioButtonList1.SetSelected(0, true);
            Properties.Settings.Default.radioSetting = 0;
            Properties.Settings.Default.Save();
        }

        public void checkmp4_CheckedChanged(object sender, EventArgs e)
        {
            //      checkmp4.OnColour = Color.FromArgb(255, 85, 85);
            listBox1.Items.Clear();
            hide.Visible = hide1.Visible = hide2.Visible = true;
            grpBoxTitle.Enabled = grpBoxAudio.Enabled = grpBoxVideo.Enabled = groupBox1.Enabled = grpBoxTitle.Enabled = grpBoxSub.Enabled = audiomuxpanel.Enabled = true;
            vremtrack.Enabled = vaddtrack.Enabled = true;
            oneclickconverter.Enabled = radioButtonList2.Enabled = expaudiobut.Enabled = sortsub.Enabled = sortaud.Enabled = true;
            groupBox3.Enabled = true;
            audiobox.Enabled = trackBar1.Enabled = convaudio.Enabled = label10.Enabled = Overwrite2.Enabled =label9.Enabled = Overwrite.Enabled = false;

            radioButtonList1.SetSelected(1, true);
            Properties.Settings.Default.radioSetting = 1;
            Properties.Settings.Default.Save();

        }
        private void checkts_CheckedChanged(object sender, EventArgs e)
        {
            //         checkts.OnColour = Color.FromArgb(255, 85, 85);
            listBox1.Items.Clear();
            hide.Visible = hide1.Visible = hide2.Visible = true;

            grpBoxTitle.Enabled = grpBoxAudio.Enabled = grpBoxVideo.Enabled = groupBox1.Enabled = grpBoxSub.Enabled = groupBox3.Enabled = audiomuxpanel.Enabled = false;
            audiobox.Enabled = languageBar3.Enabled = expaudiobut.Enabled = trackBar1.Enabled = convaudio.Enabled = false;
            oneclickconverter.Enabled = radioButtonList2.Enabled = sortsub.Enabled = sortaud.Enabled = true;
            radioButtonList1.SetSelected(2, true);
            Properties.Settings.Default.radioSetting = 2;
            Properties.Settings.Default.Save();
        }

        private void checkavi_CheckedChanged(object sender, EventArgs e)
        {
            //        checkavi.OnColour = Color.FromArgb(255, 85, 85);
            listBox1.Items.Clear();
            hide.Visible = hide1.Visible = hide2.Visible = true;

            grpBoxTitle.Enabled = grpBoxAudio.Enabled = grpBoxVideo.Enabled = groupBox1.Enabled = grpBoxSub.Enabled = groupBox3.Enabled = audiomuxpanel.Enabled = false;
            audiobox.Enabled = languageBar3.Enabled = expaudiobut.Enabled = trackBar1.Enabled = convaudio.Enabled = false;
            radioButtonList1.SetSelected(3, true);
            oneclickconverter.Enabled = radioButtonList2.Enabled = sortsub.Enabled = sortaud.Enabled = true;
            Properties.Settings.Default.radioSetting = 3;
            Properties.Settings.Default.Save();
        }


        private void checkaud_CheckedChanged(object sender, EventArgs e)
        {
            hide.Visible = false;
             hide1.Visible = hide2.Visible = true;

            audiobox.Enabled = languageBar3.Enabled = trackBar1.Enabled = convaudio.Enabled = audioext.Enabled = true;
            //      checkaud.OnColour = Color.FromArgb(255, 85, 85);
            listBox1.Items.Clear();
            grpBoxTitle.Enabled = grpBoxAudio.Enabled = grpBoxVideo.Enabled = groupBox1.Enabled = grpBoxSub.Enabled = groupBox3.Enabled = oneclickconverter.Enabled = groupBox3.Enabled = radioButtonList2.Enabled = expaudiobut.Enabled = false;
            radioButtonList1.SetSelected(0, true);
            radioButtonList2.Enabled = audiomuxpanel.Enabled = sortsub.Enabled = sortaud.Enabled = false;
            label10.Enabled = true;
            Properties.Settings.Default.radioSetting = 4;
            Properties.Settings.Default.Save();
        }

        #endregion


        // CLEAR FILES BUTTON

        #region clear_files


        private void clearfiles_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex != -1)
            {

                for (int x = listBox1.SelectedIndices.Count - 1; x >= 0; x--)
                {
                    int idx = listBox1.SelectedIndices[x];
                    listBox1.Items.RemoveAt(idx);

                }
            }
            else
            {
                listBox1.Items.Clear();
            }

        }
        #endregion


        // BROWSE BUTTON AND OPEN FILE DIALOG

        #region Browse_button


        // LEO STO PARATHIRO BROWSE TI ONOMA NA EXEI KAI NA KANEI MULTISELECT

        private void InitializeOpenFileDialog()
        {
            // Set the file dialog to filter for graphics files.
            this.openFileDialog1.Title = "Video Files";
            this.openFileDialog1.Multiselect = true;
        }



        private void cmdBrowse_Click(object sender, EventArgs e)
        {

            if (checkmp4.Checked)
            {
                this.openFileDialog1.Filter =
              "Video (*.mp4)|*.mp4";
            }
            else if (checkavi.Checked)
            {
                this.openFileDialog1.Filter =
              "Video (*.avi)|*.avi";
            }
            else if (checkts.Checked)
            {
                this.openFileDialog1.Filter =
              "Video (*.ts)|*.ts";
            }
            else if (checkmkv.Checked)
            {
                this.openFileDialog1.Filter =
              "Video (*.mkv)|*.mkv";
            }
            else
            {
                this.openFileDialog1.Filter =
                    "Audio (*.ac3, *.aac, *.m4a, *.wav, *.mp3)|*.ac3;*.aac;*.M4A;*.wav;*.mp3;)";
                //   "Audio (*.aac|*.aac";
            }

            this.openFileDialog1.ShowDialog();
            //string filename = openFileDialog1.FileName;
            foreach (string FileName in openFileDialog1.FileNames)
            {
                if (!listBox1.Items.Contains(FileName))
                {
                    listBox1.Items.Add(FileName);
                }

            }

        }
        #endregion


        // LIST BOX AND DRAG N DROP

        #region ListBox

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }


        }



        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            int i = 0;
            int j = 0;
            int m = 0;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string file in files)
            {
                m++;
            }


            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);

                // MessageBox.Show(ext);
                if (!listBox1.Items.Contains(file))
                {

                    if (checkmp4.Checked && (ext == ".mp4"))
                    {
                        listBox1.Items.Add(file);

                    }
                    else if (checkavi.Checked && (ext == ".avi"))
                    {
                        listBox1.Items.Add(file);

                    }
                    else if (checkts.Checked && (ext == ".ts"))
                    {
                        listBox1.Items.Add(file);

                    }
                    else if (checkmkv.Checked && (ext == ".mkv"))
                    {
                        listBox1.Items.Add(file);
                    }
                    else if (chaud.Checked && (ext == ".aac" | ext == ".ac3" | ext == ".m4a" | ext == ".wav" | ext == ".mp3"))
                    {
                        listBox1.Items.Add(file);

                    }
                    else if (ext == ".srt")
                    {
                        //  subbutton.Enabled = languageBar2.Enabled = DefaultSub.Enabled = ForcedSub.Enabled = Overwrite.Enabled = true;

                        m--;
                    }
                    else
                    {
                        j++;
                    }

                    listBox1.Sorted = true;
                }


            }
            if (j != 0)
            {
                MyMessageBox.Show("Change Container / found " + j + @" wrong extensions out of " + m + " files");
                i++;
            }

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // e.SuppressKeyPress = true;
            listBox1.Focus();
            //   this.listBox1.Select();
            //you might need to select one value to allow arrow keys
            //   listBox1.SelectedIndex = 0;
            if (e.KeyCode == Keys.A && e.Control)
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SetSelected(i, true);
                }
            }

        }



        #endregion


        string AddDoubleQuotes(string value)
        {
            return "\"" + value + "\"";
        }




        // ENABLE/DISABLE BUTTON WHEN TEXT LOADED IN FILEBAR   --- den to xw xreiastei akomi

        #region disable_buttons_from_filebar

        private void coverLocation_Load(object sender, EventArgs e)
        {
            //          FileBar = new FileBar();
            //         coverLocation.FileSelected += FileBar_FileSelected;
        }
        private void FileBar_FileSelected(object sender, EventArgs e)
        {

            /*           if (listBox1.Items.Count > 0 & !checkts.Checked & !checkavi.Checked )
                       {
                           addcover.Enabled = true;
                           removecover.Enabled = true;
                       }
            */
            // add other process here
        }

        public FileBar FileBar { get; set; }

        #endregion


        //App EXIT

        #region Application Exit


        string tempo = Path.GetDirectoryName(Application.ExecutablePath) + @"\sarktemp";

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //  File.Delete(tempo + @"\cover.jpg");
            //    Directory.Delete(tempo, true);
            FileUtil.ensureDirectoryExists(tempo);

            Directory.Delete(tempo, true);

            System.Windows.Forms.Application.Exit();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.Delete(tempo + @"\cover.jpg");
            //    Directory.Delete(tempo, true);
            System.Windows.Forms.Application.Exit();
        }

        #endregion



        // COVER GROUP //

        #region addcover_button

        private void addcover_Click(object sender, EventArgs e)
        {
            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvpropPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " ")
            {
                string covername = coverLocation.Filename;
                //  if ((covername != "") && (listBox1.Items.Count > 0))
                if (covername != "")
                {
                    if (listBox1.Items.Count > 0)
                    {
                        System.IO.File.Copy(covername, @"sarktemp\cover.jpg", true);

                        //     string covernamenoex = System.IO.Path.GetFileNameWithoutExtension(covername);
                        //     string covextension = Sytem.IO.Path.GetExtension(covername);

                        foreach (string FileName in listBox1.Items)
                        {
                            /*                        pBar1.Visible = true;
                                                    // Set Minimum to 1 to represent the first file being copied.
                                                    pBar1.Minimum = 1;
                                                    // Set Maximum to the total number of files to copy.
                                                    pBar1.Maximum = FileName.Length;
                                                    // Set the initial value of the ProgressBar.
                                                    pBar1.Value = 1;
                                                    // Set the Step property to a value of 1 to represent each file being copied.
                                                    pBar1.Step = 1;

                          */
                            string anyCommand = AddDoubleQuotes(FileName);
                            //           string nostringpath = AddDoubleQuotes(Main.Instance.Settings.ffmpegPath);
                            Process mkvprop = new Process();
                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);


                            if (checkmkv.Checked)
                            {
                                string cover = AddDoubleQuotes(Path.GetFullPath(@"sarktemp\cover.jpg"));

                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvpropPath);
                                mkvprop.StartInfo.Arguments = anyCommand + @" --attachment-name cover --attachment-mime-type image/jpeg --add-attachment " + cover;
                            }
                            else if (checkmp4.Checked)
                            {
                                string cover = Path.GetFullPath(@"sarktemp\cover.jpg");

                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mp4boxPath);
                                //  mkvprop.StartInfo.Arguments = anyCommand + @" --artwork " + cover + " --overWrite ";
                                mkvprop.StartInfo.Arguments = @" -itags ""cover=" + cover + @" "" " + anyCommand;
                            }


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop.Start();



                            /*                       for (int x = 1; x <= FileName.Length; x++)
                                                   {
                                                       // Copy the file and increment the ProgressBar if successful.
                                                       if (mkvprop.Start() == true)
                                                       {
                                                           // Perform the increment on the ProgressBar.
                                                           pBar1.PerformStep();
                                                       }
                                                   }

                                                   pBar1.Value = 1;

                       */
                        }
                    }
                    else
                    {
                        MyMessageBox.Show("No Files to Cover!");

                    }


                }
                else
                {
                    MyMessageBox.Show(" Empty cover dir ");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }


        }
        #endregion

        #region EXPORT COVER
        private void expcover_Click(object sender, EventArgs e)
        {
            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvextPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " ")
            {


                if (listBox1.Items.Count > 0)
                {

                    int i = 1;
                    foreach (string FileName in listBox1.Items)
                    {
                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName) + @"\covers";
                        FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);

                        if (checkmkv.Checked)
                        {
                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);
                            mkvprop.StartInfo.Arguments = anyCommand + @"  attachments  1:cover_" + i + @".jpg";
                        }
                        else if (checkmp4.Checked)
                        {
                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mp4boxPath);
                            mkvprop.StartInfo.Arguments = @" -dump-cover " + anyCommand + @" -out " + mkvprop.StartInfo.WorkingDirectory + @"\cover_" + i + @".jpg"" ";
                        }
                        i++;
                        mkvprop.StartInfo.CreateNoWindow = true;
                        mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        mkvprop.Start();
                    }


                }
                else
                {
                    MyMessageBox.Show("No Files to Export Cover");

                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first!");
            }
        }

        #endregion

        #region removecover_button
        private void removecover_Click(object sender, EventArgs e)
        {
            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvpropPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " ")
            {

                if (listBox1.Items.Count > 0)
                {
                    foreach (string FileName in listBox1.Items)
                    {


                        /*                     pBar1.Visible = true;
                                             // Set Minimum to 1 to represent the first file being copied.
                                             pBar1.Minimum = 1;
                                             // Set Maximum to the total number of files to copy.
                                             pBar1.Maximum = FileName.Length;
                                             // Set the initial value of the ProgressBar.
                                             pBar1.Value = 1;
                                             // Set the Step property to a value of 1 to represent each file being copied.
                                             pBar1.Step = 1;
                        */

                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                        if (checkmkv.Checked)
                        {

                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvpropPath);
                            mkvprop.StartInfo.Arguments = anyCommand + @" --delete-attachment mime-type:image/jpeg --delete-attachment mime-type:image/png";
                        }
                        else if (checkmp4.Checked)
                        {
                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mp4boxPath);
                            mkvprop.StartInfo.Arguments = @" -itags cover= " + anyCommand;

                        }

                        mkvprop.StartInfo.CreateNoWindow = true;
                        mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        mkvprop.Start();


                        /*                    for (int x = 1; x <= FileName.Length; x++)
                                            {
                                                // Copy the file and increment the ProgressBar if successful.
                                                if (mkvprop.Start() == true)
                                                {
                                                    // Perform the increment on the ProgressBar.
                                                    pBar1.PerformStep();
                                                }
                                            }

                                            pBar1.Value = 1;
                    */
                    }
                }
                else
                {
                    MyMessageBox.Show("No Files to remove Cover!");

                }
            }
            else
            {
                MyMessageBox.Show("Please Add exe paths in Options first!");
            }
        }
        #endregion


        // Literal for REGEX

        #region literal regex

        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, new CodeGeneratorOptions { IndentString = "\t" });
                    var literal = writer.ToString();
                    literal = literal.Replace(string.Format("\" +{0}\t\"", Environment.NewLine), "");
                    return literal;
                }
            }
        }
        #endregion


        #region info button
        private void info_Click(object sender, EventArgs e)
        {
            MyMessageBox.Show("[ Info for Title , Video and Audio Names ]\n\n• Check numbering and add {num} to the title e.g. 'My Title {num}'.\n• Use {filename} to use the file name as title.\n• Use Start number for first Video.\n• Leave empty to Remove Title.");
        }
        #endregion


        // ADD TITLE META //

        #region Add and Remove Title


        private void numtitlestart_KeyPress(object sender, KeyPressEventArgs e)
        {
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
        }

        private void nummetatitle_CheckedChanged(object sender, EventArgs e)
        {
            numtitlestart.Enabled = start1.Enabled = nummetatitle.Checked;
        }


        #endregion


        // MAKE TITLES TITLE+VIDEO+AUDIO BUTTON

        #region ENTER BUTTONS


        //ADD TITLE,VIDEO,AUDIO ENTER BUTTONS
        public int w;
        public string[] sss;
        public string ss;
        public string[] zz;
        public string[] lan;
        //      public string[] lan2;
        public bool[] deftrack1;
        public bool[] deftrack2;
        public bool[] deftrack3;
        public bool[] editr;
        public int index1;
        public int index2;
        public int index3;

        public void addNamesMeta(object sender, EventArgs e)
        {
            MuxStream[] vStreams = getStreams(metavideotracks);
            MuxStream[] aStreams = getStreams(metaaudiotracks);
            MuxStream[] sStreams = getStreams(metasubtracks);

            if (listBox1.Items.Count > 0 & Main.Instance.Settings.mkvpropPath != " " & Main.Instance.Settings.mp4boxPath != " ")
            {
                if (sender == addtitlemeta)
                {
                    ss = titlemeta.Text.ToString();
                    sss = new string[] { ss };
                    w = int.Parse(numtitlestart.Text);
                    zz = new string[] { w.ToString() };
                    lan = new string[] { "0" };
                }
                else if (sender == addtrackvid)
                {
                    sss = vStreams.Select(I => I.name).ToArray();
                    zz = vStreams.Select(I => (I.numbering)).ToArray();
                    lan = vStreams.Select(I => I.language).ToArray();
                    deftrack1 = vStreams.Select(I => I.bDefaultTrack).ToArray();
                    editr = vStreams.Select(I => I.bEdittr).ToArray();
                    index1 = Array.IndexOf(deftrack1, true);

                    foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
                    {
                        foreach (string x in lan)
                        {

                            if (x.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                            {
                                int co = Array.IndexOf(lan, x);
                                lan[co] = strLanguage.Value.ToString();
                            }
                        }

                    }

                    //         string toDisplay = string.Join(Environment.NewLine, lan);
                    //          MessageBox.Show(toDisplay);

                    //             deftrack = vStreams.Select(I => I.bDefaultTrack).ToArray();
                    //             index1 = Array.FindIndex(deftrack, I => I == true);

                    /*                 foreach (bool def in deftrack)
                                     {
                                         if (def == true)
                                         {
                                             int lex = def.
                                         }
                                     }
                 */

                }
                else if (sender == addtrackaud)
                {

                    sss = aStreams.Select(I => (I.name)).ToArray();
                    zz = aStreams.Select(I => (I.numbering)).ToArray();
                    lan = aStreams.Select(I => I.language).ToArray();
                    deftrack2 = aStreams.Select(I => I.bDefaultTrack).ToArray();
                    index2 = Array.IndexOf(deftrack2, true);


                    foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
                    {
                        foreach (string x in lan)
                        {

                            if (x.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                            {
                                int co = Array.IndexOf(lan, x);
                                lan[co] = strLanguage.Value.ToString();
                            }
                        }

                    }


                }
                else if (sender == addtracksub)
                {
                    sss = sStreams.Select(I => (I.name)).ToArray();
                    zz = sStreams.Select(I => (I.numbering)).ToArray();
                    lan = sStreams.Select(I => I.language).ToArray();
                    deftrack3 = sStreams.Select(I => I.bDefaultTrack).ToArray();
                    index3 = Array.IndexOf(deftrack3, true);


                    foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                    {
                        foreach (string x in lan)
                        {

                            if (x.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                            {
                                int co = Array.IndexOf(lan, x);
                                lan[co] = strLanguage.Value.ToString();
                            }
                        }

                    }

                }

                int i = 1;
                int j = 0;
                int r = 2;
                int metratracks = 0;
                foreach (string nametrack in sss)
                {

                    string z2 = zz[j];
                    string z3 = lan[j];

                    int lamda = int.Parse(z2);
                    ss = nametrack;


                    foreach (string FileName in listBox1.Items)
                    {

                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                        string metaname;

                        int count = 1;
                        Process subtrack = new Process();
                        subtrack.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                        subtrack.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);
                        subtrack.StartInfo.Arguments = @" -i " + anyCommand;// + @" >" + System.IO.Path.GetDirectoryName(FileName) + @"\subinfo.txt";
                        subtrack.StartInfo.CreateNoWindow = true;
                        subtrack.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        subtrack.StartInfo.RedirectStandardOutput = true;
                        subtrack.StartInfo.UseShellExecute = false;
                        subtrack.Start();
                        while (!subtrack.StandardOutput.EndOfStream)
                        {
                            string line = subtrack.StandardOutput.ReadLine();
                            if (line.Contains(@"Track"))
                            {
                                count++;
                            }
                        }
                        /*        //    int count = File.ReadLines(System.IO.Path.GetDirectoryName(FileName) + @"\subinfo.txt")
                                   int count = File.ReadLines()
                                    .Count(line => line.Contains(@"Track"));
                                    count++;
                        */

                        //       MessageBox.Show(count.ToString());
                        if (metratracks > 0)
                        {
                            count = count + metratracks;   //edw na to dw gia parapanw apo 3 tracks subtitles
                        }




                        if (ss.Contains("{num}") || ss.Contains("{filename}"))
                        {
                            metaname = ss.Replace("{num}", lamda.ToString()).Replace("{filename}", System.IO.Path.GetFileNameWithoutExtension(FileName));
                            lamda++;
                        }
                        else
                        {
                            metaname = ss;
                        }

                        if (checkmkv.Checked)
                        {
                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvpropPath);

                            if (sender == addtitlemeta)
                            {
                                mkvprop.StartInfo.Arguments = anyCommand + @" --tags all: --set title=" + ToLiteral(metaname);
                            }
                            else if (sender == addtrackvid)
                            {

                                if (j == index1)
                                {
                                    mkvprop.StartInfo.Arguments = anyCommand + @" --edit track:v" + i + " --set name=" + ToLiteral(metaname) + " --set language=" + z3 + @" --set flag-default=1";

                                }
                                else
                                {
                                    mkvprop.StartInfo.Arguments = anyCommand + @" --edit track:v" + i + " --set name=" + ToLiteral(metaname) + " --set language=" + z3 + @" --set flag-default=0";

                                }

                            }
                            else if (sender == addtrackaud)
                            {
                                if (j == index2)
                                {
                                    mkvprop.StartInfo.Arguments = anyCommand + @" --edit track:a" + i + " --set name=" + ToLiteral(metaname) + @" --set flag-default=1" + " --set language=" + z3;
                                }
                                else
                                {
                                    mkvprop.StartInfo.Arguments = anyCommand + @" --edit track:a" + i + " --set name=" + ToLiteral(metaname) + @" --set flag-default=0" + " --set language=" + z3;
                                }

                            }
                            else if (sender == addtracksub)
                            {
                                if (j == index3)
                                {
                                    mkvprop.StartInfo.Arguments = anyCommand + @" --edit track:s" + i + " --set name=" + ToLiteral(metaname) + @" --set flag-default=1" + " --set language=" + z3;
                                }
                                else
                                {
                                    mkvprop.StartInfo.Arguments = anyCommand + @" --edit track:s" + i + " --set name=" + ToLiteral(metaname) + @" --set flag-default=0" + " --set language=" + z3;
                                }
                            }
                        }
                        else if (checkmp4.Checked)
                        {

                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mp4boxPath);

                            if (sender == addtitlemeta)
                            {
                                mkvprop.StartInfo.Arguments = @" -itags name=" + ToLiteral(metaname) + "  " + anyCommand;

                            }
                            else if (sender == addtrackvid)
                            {
                                mkvprop.StartInfo.Arguments = " -name 1=" + ToLiteral(metaname) + " -lang 1=" + z3 + " " + anyCommand;

                            }
                            else if (sender == addtrackaud)
                            {
                                mkvprop.StartInfo.Arguments = " -name " + r + "=" + ToLiteral(metaname) + " -lang " + r + "=" + z3 + " " + anyCommand;

                            }
                            else if (sender == addtracksub)
                            {

                                //  .TakeWhile(line => !line.Contains("CustomerCh"));

                                mkvprop.StartInfo.Arguments = " -name " + count + "=" + ToLiteral(metaname) + " -lang " + count + "=" + z3 + " " + anyCommand;
                            }

                        }
                        mkvprop.StartInfo.CreateNoWindow = true;
                        mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        if (checkmp4.Checked)
                        {


                            var confirmResult = MessageBox.Show("Are you sure to proceed ?? \nMake Sure # Audio Tracks MATCH file tracks else mp4 will be unstable!!!", "", MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                mkvprop.Start();
                            }
                            else
                            {
                                mkvprop.Close();
                            }


                        }
                        else
                        {
                            mkvprop.Start();

                        }

                    }
                    metratracks++;
                    i++;
                    j++;
                    r++;
                }

            }
            else
            {
                MyMessageBox.Show("No files to process and/or No dirs in Options!");
            }
        }

        // REMOVE TITLE BUTTON //AXREIASTO

        /*            private void rmvtitlemeta_Click(object sender, EventArgs e)
                    {
                        if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvpropPath) != " ")
                        {
                            if (listBox1.Items.Count > 0)
                            {
                                foreach (string FileName in listBox1.Items)
                                {
                                    string anyCommand = AddDoubleQuotes(FileName);
                                    Process mkvprop = new Process();
                                    mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                    if (checkmkv.Checked)
                                    {
                                        mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvpropPath);
                                        mkvprop.StartInfo.Arguments = anyCommand + @"  --tags all: -d title";
                                    }
                                    else if (checkmp4.Checked)
                                    {
                                        mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvpropPath);
                                        mkvprop.StartInfo.Arguments = @" -itags name="" "" " + anyCommand;
                                    }
                                    mkvprop.StartInfo.CreateNoWindow = true;
                                    mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    mkvprop.Start();
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Files to remove Title!");

                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Add exe paths in Settings first");
                        }
                    }
        */



        public void addtitlemeta_Click(object sender, EventArgs e)
        {
            addNamesMeta(sender, e);

        }

        private void addtrackvid_Click(object sender, EventArgs e)
        {
            addNamesMeta(sender, e);
        }

        private void addtrackaud_Click(object sender, EventArgs e)
        {
            addNamesMeta(sender, e);

        }

        private void addtracksub_Click(object sender, EventArgs e)
        {
            addNamesMeta(sender, e);
        }



        #endregion


        // ADD ITEMS IN COMBOBOXES VIDEO, AUDIO //

        #region tracks video and audio comboboxes
        public int x = 0;
        public int y = 0;
        public int z = 0;
        private void vaddtrack_Click(object sender, EventArgs e)
        {
            x++;
            comboBoxvideotrack.Items.Add("Video Track " + (x + 1).ToString("D2"));
            SubtitleAddTrack(sender, e);
            comboBoxvideotrack.SelectedIndex = x;
        }

        private void vremtrack_Click(object sender, EventArgs e)
        {
            if (x > 0)
            {
                SubtitleAddTrack(sender, e);
                comboBoxvideotrack.Items.RemoveAt(x);
                comboBoxvideotrack.SelectedIndex = x - 1;
                x--;
            }

        }

        private void aaddtrack_Click(object sender, EventArgs e)
        {

            y++;
            comboBoxaudiotrack.Items.Add("Audio Track " + (y + 1).ToString("D2"));
            SubtitleAddTrack(sender, e);
            comboBoxaudiotrack.SelectedIndex = y;



        }

        private void aremtrack_Click(object sender, EventArgs e)
        {
            if (y > 0)
            {
                SubtitleAddTrack(sender, e);
                comboBoxaudiotrack.Items.RemoveAt(y);
                comboBoxaudiotrack.SelectedIndex = y - 1;
                y--;
            }
        }


        private void saddtrack_Click(object sender, EventArgs e)
        {

            z++;
            comboBoxsubtrack.Items.Add("Sub Track " + (z + 1).ToString("D2"));
            SubtitleAddTrack(sender, e);
            comboBoxsubtrack.SelectedIndex = z;



        }

        private void sremtrack_Click(object sender, EventArgs e)
        {
            if (z > 0)
            {
                SubtitleAddTrack(sender, e);
                comboBoxsubtrack.Items.RemoveAt(z);
                comboBoxsubtrack.SelectedIndex = z - 1;
                z--;
            }
        }





        #endregion


        // METADATA TRACKS BUTTONS CHECKED, CHANGED // merika den ta xreiazomai na ta dw

        #region video and audio track metadata
        /*      private void edittrvideo_CheckedChanged(object sender, EventArgs e)
              {
                  trackvidlbl.Enabled = deftrvidlbl.Enabled = addtrackvid.Enabled = edittrvideo.Checked;
              }

              private void edittraudio_CheckedChanged(object sender, EventArgs e)
              {
                  trackaudlbl.Enabled = deftraudlbl.Enabled = addtrackaud.Enabled = edittraudio.Checked;
                  //  languageBar1.SetDefault();

              }

              private void trackvidlbl_CheckedChanged(object sender, EventArgs e)
              {
                  trackvideonamebox.Enabled = numvidlbl.Enabled = vidnumb.Enabled = trackvidlbl.Checked;
              }

              private void trackaudlbl_CheckedChanged(object sender, EventArgs e)
              {
                  trackaudionamebox.Enabled = numaudlbl.Enabled = audnumb.Enabled = trackaudlbl.Checked;
              }

              */
        private void comboBoxvideotrack_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int currentMyComboBoxIndex = comboBoxvideotrack.SelectedIndex;
            SubtitleAddTrack(sender, e);
            this.ActiveControl = null;
        }

        private void comboBoxaudiotrack_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SubtitleAddTrack(sender, e);

        }

        private void comboBoxsubtrack_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SubtitleAddTrack(sender, e);
        }



        #endregion


        //      int indexx = 0;

        public MuxStream[] getStreams(List<oneclicktrack> controls)
        {
            List<MuxStream> streams = new List<MuxStream>();
            foreach (oneclicktrack t in controls)
            {
                if (t.Stream != null)
                    streams.Add(t.Stream);
            }
            return streams.ToArray();
            //          indexx++;
        }



        #region function to add oneclicktracks controls

        public void SubtitleAddTrack(Object sender, EventArgs e)
        {

            int curVideoIndex = comboBoxvideotrack.SelectedIndex;
            string oneclickname = curVideoIndex.ToString();
            int curAudioIndex = comboBoxaudiotrack.SelectedIndex;
            string oneclickname2 = curAudioIndex.ToString();
            int curSubIndex = comboBoxsubtrack.SelectedIndex;
            string oneclickname3 = curSubIndex.ToString();


            if (sender == vaddtrack)
            {

                oneclicktrack a = new oneclicktrack();
                metavideotracks.Add(a);//neagrammi
                a.Name = x.ToString();
                oneclicktrack1.Controls.Add(a);
                a.visibar = true;
                inputs.Add(a.input);
                listboxes.Add(a.languageBar1.language);
                inputs.Add(a.vidnumb);
                if (bunifuiOSSwitch1.Value == false)
                {
                    ApplyTheme(zcolor(240, 240, 240), zcolor(255, 192, 192), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), Color.White, Color.White, Color.Black, Color.Black, Color.Pink, SystemColors.Control);
                    label2.Visible = true;
                    label5.Visible = true;
                }

                if (bunifuiOSSwitch1.Value == true)
                {
                    //  ApplyTheme(zcolor(128, 128, 255), zcolor(51, 51, 51), zcolor(45, 45, 48), zcolor(104, 104, 104), zcolor(51, 51, 51), zcolor(104, 104, 104), Color.Black, Color.White, zcolor(104, 104, 104));
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);

                    label2.Visible = false;
                    label5.Visible = false;
                }

                a.BringToFront();
            }
            else if (sender == comboBoxvideotrack)
            {

                oneclicktrack bx = grpBoxVideo.Controls.Find(oneclickname, true).FirstOrDefault() as oneclicktrack;
                if (bx != null)
                {
                    bx.BringToFront();
                }

            }
            else if (sender == vremtrack)
            {
                int tempindex = comboBoxvideotrack.SelectedIndex;


                oneclicktrack bx1 = grpBoxVideo.Controls.Find((x - 1).ToString(), true).FirstOrDefault() as oneclicktrack;

                bx1.BringToFront();

                oneclicktrack bx2 = grpBoxVideo.Controls.Find(x.ToString(), true).FirstOrDefault() as oneclicktrack;

                Controls.Remove(bx2);
                metavideotracks.RemoveAt(metavideotracks.Count - 1);

            }
            else if (sender == aaddtrack)
            {
                oneclicktrack b = new oneclicktrack();
                b.Name = y.ToString();
                metaaudiotracks.Add(b);//neagrammi
                oneclicktrack2.Controls.Add(b);
                b.setlang = 1;
                b.visibar = true;

                if (checkmp4.Checked)
                {
                    b.deftrvidlbl.Visible = false;
                }

                inputs.Add(b.input);
                listboxes.Add(b.languageBar1.language);
                inputs.Add(b.vidnumb);
                if (bunifuiOSSwitch1.Value == false)
                {
                    ApplyTheme(zcolor(240, 240, 240), zcolor(255, 192, 192), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), Color.White, Color.White, Color.Black, Color.Black, Color.Pink, SystemColors.Control);
                    label2.Visible = true;
                    label5.Visible = true;
                }

                if (bunifuiOSSwitch1.Value == true)
                {
                    //  ApplyTheme(zcolor(128, 128, 255), zcolor(51, 51, 51), zcolor(45, 45, 48), zcolor(104, 104, 104), zcolor(51, 51, 51), zcolor(104, 104, 104), Color.Black, Color.White, zcolor(104, 104, 104));
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);

                    label2.Visible = false;
                    label5.Visible = false;
                }
                b.BringToFront();


            }
            else if (sender == comboBoxaudiotrack)
            {

                oneclicktrack bxa = grpBoxAudio.Controls.Find(oneclickname2, true).FirstOrDefault() as oneclicktrack;
                if (bxa != null)
                {
                    bxa.BringToFront();
                }

            }
            else if (sender == aremtrack)
            {
                int tempindex = comboBoxaudiotrack.SelectedIndex;


                oneclicktrack bx = grpBoxAudio.Controls.Find((y - 1).ToString(), true).FirstOrDefault() as oneclicktrack;

                bx.BringToFront();

                oneclicktrack bx2 = grpBoxAudio.Controls.Find(y.ToString(), true).FirstOrDefault() as oneclicktrack;

                Controls.Remove(bx2);
                metaaudiotracks.RemoveAt(metaaudiotracks.Count - 1);
            }
            else if (sender == saddtrack)
            {
                oneclicktrack c = new oneclicktrack();
                c.Name = z.ToString();
                metasubtracks.Add(c);//neagrammi
                oneclicktrack3.Controls.Add(c);
                c.setlang = 1;
                c.visibar = true;

                if (checkmp4.Checked)
                {
                    c.deftrvidlbl.Visible = false;
                }
                inputs.Add(c.input);
                listboxes.Add(c.languageBar1.language);
                inputs.Add(c.vidnumb);
                if (bunifuiOSSwitch1.Value == false)
                {
                    ApplyTheme(zcolor(240, 240, 240), zcolor(255, 192, 192), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), Color.White, Color.White, Color.Black, Color.Black, Color.Black, SystemColors.Control);
                    label2.Visible = true;
                    label5.Visible = true;
                }

                if (bunifuiOSSwitch1.Value == true)
                {
                    //  ApplyTheme(zcolor(128, 128, 255), zcolor(51, 51, 51), zcolor(45, 45, 48), zcolor(104, 104, 104), zcolor(51, 51, 51), zcolor(104, 104, 104), Color.Black, Color.White, zcolor(104, 104, 104));
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);

                    label2.Visible = false;
                    label5.Visible = false;
                }
                c.BringToFront();
            }
            else if (sender == comboBoxsubtrack)
            {
                oneclicktrack bza = grpBoxSub.Controls.Find(oneclickname3, true).FirstOrDefault() as oneclicktrack;
                if (bza != null)
                {
                    bza.BringToFront();
                }
            }
            else if (sender == sremtrack)
            {
                int tempindex = comboBoxsubtrack.SelectedIndex;


                oneclicktrack bz = grpBoxSub.Controls.Find((z - 1).ToString(), true).FirstOrDefault() as oneclicktrack;

                bz.BringToFront();

                oneclicktrack bz2 = grpBoxSub.Controls.Find(z.ToString(), true).FirstOrDefault() as oneclicktrack;

                Controls.Remove(bz2);
                metasubtracks.RemoveAt(metasubtracks.Count - 1);
            }
        }
        #endregion



        private void flowLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

            ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel7.ClientRectangle,
                    Color.Black, 1, ButtonBorderStyle.Dotted, // left
                    Color.Black, 1, ButtonBorderStyle.Dotted, // top
                    Color.Black, 1, ButtonBorderStyle.Dotted, // right
                    Color.Black, 1, ButtonBorderStyle.Dotted);// bottom         
        }

        private void containerpanel_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, containerpanel.ClientRectangle,
                        Color.Black, 1, ButtonBorderStyle.Dotted, // left
                        Color.Black, 1, ButtonBorderStyle.Dotted, // top
                        Color.Black, 1, ButtonBorderStyle.Dotted, // right
                        Color.Black, 1, ButtonBorderStyle.Dotted);// bottom         
        }










        //  CONVERT BUTTON

        #region OneClickButton

        private void oneclickconverter_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;


            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {
                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;





                    foreach (string FileName in listBox1.Items)
                    {

                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                        mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);

                        if (radioButtonList2.SelectedIndex == 0)
                        {
                            //   mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV";
                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            int countfiles = mkvprop.StartInfo.WorkingDirectory.Length;
                            FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);
                            if (checkavi.Checked | checkts.Checked)
                            {
                                //   mkvprop.StartInfo.Arguments = @" -y -ss 0:01 -fflags +genpts -i " + anyCommand + @" -c:v copy -c:a copy " + AddDoubleQuotes(newname + @".mkv");
                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);
                                mkvprop.StartInfo.Arguments = @" -o  " + AddDoubleQuotes(newname + @".mkv") + @" " + anyCommand;

                            }
                            else
                            {
                                mkvprop.StartInfo.Arguments = @" -i " + anyCommand + @" -map 0 -vcodec copy -acodec copy -metadata:s vendor_id= " + AddDoubleQuotes(newname + @".mkv");
                                //  mkvprop.StartInfo.Arguments = @" -i " + anyCommand + @" -map 0 -vcodec copy -acodec copy -map_metadata 0 " + AddDoubleQuotes(newname + @".mkv");

                                // -map 0 - map 1 - c copy - c:v: 1 png - disposition:v: 1 attached_pic out.mp4

                            }
                        }
                        else if (radioButtonList2.SelectedIndex == 1)
                        {
                            //  mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName) + @"\newMP4";
                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);
                            if (checkavi.Checked | checkts.Checked)
                            {
                                mkvprop.StartInfo.Arguments = @" -fflags +genpts -i " + anyCommand + @" -c:v copy -c:a copy " + AddDoubleQuotes(newname + @".mp4");

                            }
                            else
                            {
                                mkvprop.StartInfo.Arguments = @" -i " + anyCommand + @" -map 0 -map -0:s -c copy " + AddDoubleQuotes(newname + @".mp4");

                            }
                        }

                        mkvprop.StartInfo.CreateNoWindow = true;
                        mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        mkvprop.Start();
                        //string output = mkvprop.StandardOutput.ReadToEnd();
                        mkvprop.WaitForExit();
                        pBar1.PerformStep();
                        pBar1.Refresh();
                        int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                        pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                            new Font("Arial", (float)10.25, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                        cnt++;
                        Cursor.Current = Cursors.Default;

                    }
                }
                else
                {
                    MyMessageBox.Show("No Files to Convert!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options  first");
            }
        }










































        private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
        {

            Graphics g = e.Graphics;
            Brush _TextBrush;

            // Get the item from the collection.
            TabPage _TabPage = tabControl2.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _TabBounds = tabControl2.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _TextBrush = new SolidBrush(Color.Black);
                g.FillRectangle(Brushes.White, e.Bounds);
            }
            else
            {
                _TextBrush = new System.Drawing.SolidBrush(e.ForeColor);
                // e.DrawBackground();
            }

            // Use our own font. Because we CAN.
            Font _TabFont = new Font(e.Font.FontFamily, (float)12, FontStyle.Bold, GraphicsUnit.Pixel);
            //  Font fnt = new Font(e.Font.FontFamily, (float)7.5, FontStyle.Bold);

            // Draw string. Center the text.
            StringFormat _StringFlags = new StringFormat();
            _StringFlags.Alignment = StringAlignment.Center;
            _StringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(tabControl2.TabPages[e.Index].Text, _TabFont, _TextBrush,
            _TabBounds, new StringFormat(_StringFlags));











            TabPage CurrentTab = tabControl2.TabPages[e.Index];
            Rectangle ItemRect = tabControl2.GetTabRect(e.Index);
            ItemRect.Height = 17;
            ItemRect.Inflate(0, 2);
            SolidBrush FillBrush = new SolidBrush(BackColor);
            SolidBrush TextBrush = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;





            SolidBrush fillbrush = new SolidBrush(BackColor);



            //draw rectangle behind the tabs
            Rectangle lasttabrect = tabControl2.GetTabRect(tabControl2.TabPages.Count - 1);
            Rectangle background = new Rectangle();
            background.Location = new Point(lasttabrect.Right, 0);

            //pad the rectangle to cover the 1 pixel line between the top of the tabpage and the start of the tabs
            background.Size = new Size(tabControl2.Right - background.Left, lasttabrect.Height + 3);
            e.Graphics.FillRectangle(fillbrush, background);


            //If we are currently painting the Selected TabItem we'll
            //change the brush colors and inflate the rectangle.
            if (System.Convert.ToBoolean(e.State & DrawItemState.Selected))
            {
                FillBrush.Color = BackColor;
                TextBrush.Color = ForeColor;
                ItemRect.Inflate(2, 2);
            }

            //Set up rotation for left and right aligned tabs
            if (tabControl2.Alignment == TabAlignment.Left || tabControl2.Alignment == TabAlignment.Right)
            {
                float RotateAngle = 90;
                if (tabControl2.Alignment == TabAlignment.Left)
                    RotateAngle = 270;
                PointF cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                ItemRect = new Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
            }

            //Next we'll paint the TabItem with our Fill Brush
            e.Graphics.FillRectangle(FillBrush, ItemRect);

            //Now draw the text.
            e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, (RectangleF)ItemRect, sf);

            //Reset any Graphics rotation
            e.Graphics.ResetTransform();




            //Finally, we should Dispose of our brushes.
            FillBrush.Dispose();
            TextBrush.Dispose();


        }





        #endregion


/*
        public static Encoding GetEncoding(string sFileName)
        {
            using (var reader = new StreamReader(sFileName, Encoding.Default, true))
            {
                string sContent = "";
                if (reader.Peek() >= 0) // you need this!
                    sContent = reader.ReadToEnd();
                Encoding MyEncoding = reader.CurrentEncoding;
                if (MyEncoding == Encoding.Default) // Ansi detected (this happens if BOM is missing)
                { // Look, if there are typical UTF8 chars in this file...
                    string sUmlaute = "ÄÖÜäöüß";
                    bool bUTF8CharDetected = false;
                    for (int z = 0; z < sUmlaute.Length; z++)
                    {
                        string sUTF8Letter = sUmlaute.Substring(z, 1);
                        string sUTF8LetterInAnsi = Encoding.Default.GetString(Encoding.UTF8.GetBytes(sUTF8Letter));
                        if (sContent.Contains(sUTF8LetterInAnsi))
                        {
                            bUTF8CharDetected = true;
                            break;
                        }
                    }
                    if (bUTF8CharDetected) MyEncoding = Encoding.UTF8;
                }
                return MyEncoding;
            }
        }

        */


        #region convertSubtitles


        public string slan;

        public void subbutton_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;




            string def;
            if (DefaultSub.Checked == true)
            {

                if(checkmp4.Checked)
                {
                    def = "default";
                }
                else
                {
                    def = "yes";
                }
                
            }
            else
            {

                if (checkmp4.Checked)
                {
                    def = "";
                }
                else
                {
                    def = "no";
                }
            }

            string forced;

            if (ForcedSub.Checked == true)
            {
                forced = "yes";
            }
            else
            {
                forced = "no";
            }



            slan = languageBar2.language.SelectedItem.ToString();



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
            {


                if (slan.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan = strLanguage.Value.ToString();
                }


            }

            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {
                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;


                    string langu = languageBar2.language.SelectedItem.ToString();

                    int au = 0;

                    foreach (string FileName in listBox1.Items)
                    {

                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                        //          mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);


                        //   mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV";

                        mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                        FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);

                        string dirr = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV\";
                        FileUtil.ensureDirectoryExists(dirr);

                        string info = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".srt";
                        string info2 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".ssa";
                        string info3 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".idx";
                        string info4 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".vtt";
                        string info5 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".ass";

                        if (File.Exists(info))
                        {
                            aext = @".srt";
                        }
                        else if (File.Exists(info2))
                        {
                            aext = @".ssa";

                        }
                        else if (File.Exists(info3))
                        {
                            aext = @".idx";
                        }
                        else if (File.Exists(info4))
                        {
                            aext = @".vtt";
                        }
                        else if (File.Exists(info5))
                        {
                            aext = @".ass";
                        }

                        if ((checkmkv.Checked && (File.Exists(info) || File.Exists(info2) || File.Exists(info3) || File.Exists(info4) || File.Exists(info5)) ) ^ (checkmp4.Checked && File.Exists(info)))
                        {

                            //        MediaInfo sMI = new MediaInfo();
                            //       String sToDisplay = "";
                            //       sMI.Open(info);
                            //        sToDisplay = sMI.Get(StreamKind.Text, 0, "CodecID");




                            //    var en = GetEncoding(info);
                            //    MyMessageBox.Show(en.ToString());
                            //       {
                            //             while (sr.Peek() >= 0)
                            //            {
                            //                var kaka = (char)sr.Read();
                            //                 MyMessageBox.Show(kaka.ToString());
                            //             }

                            //Test for the encoding after reading, or at least
                            //after the first read.
                            //               lolas = sr.CurrentEncoding.ToString();

                            //          }
                            //  File.WriteAllText(info, rtext, Encoding.UTF8);
                            //          MyMessageBox.Show(lolas);


                            //       UTF8Encoding unicode = new UTF8Encoding();

                            //      string rt = File.ReadAllText(info);
                            /*               Byte[] encodedBytes = Encoding.UTF8.GetBytes(rt);
                                           var fs = new FileStream(info, FileMode.Create);
                                           Byte[] bom = Encoding.UTF8.GetPreamble();
                                           fs.Write(bom, 0, bom.Length);
                                           fs.Write(encodedBytes, 0, encodedBytes.Length);
                                           fs.Close();
                                       */
                            if (checkmp4.Checked)
                            {
                                string utfvar =" ";
 

                           
                                string filencoding;
                                using (StreamReader sr = new StreamReader(info, Encoding.Default, true))
                                {
                                    sr.Read();
                                    filencoding = sr.CurrentEncoding.ToString();

                                }
                                if (filencoding.Contains("UTF8"))
                                {
                                    utfvar = " ";
                                }
                                else
                                {
                                    utfvar = " -sub_charenc windows-1253 ";
                                }
                            
                           
                        
                           
                            String ToDisplay = "0";
                            String ToDisplay1 = "";
                            string capt = "";
                            MediaInfo MI = new MediaInfo();
                            MI.Open(FileName);
                            ToDisplay = MI.Get(StreamKind.Text, 0, "StreamCount");
                            ToDisplay1 = MI.Get(StreamKind.Text, 0, "Format");

                            if (ToDisplay == "")
                            {
                                ToDisplay = "0";
                            }


                            



                            if (ToDisplay1.Contains("VobSub"))
                            {
                                capt = "-c:s:0 dvdsub";
                            }
                            else if (ToDisplay1.Contains("text") || ToDisplay1.Contains(""))
                            {
                                capt = "-c:s:0 subrip";
                            }
                            else
                            {
                                capt = "";
                            }
                            
                           // MyMessageBox.Show(ToDisplay1);

                            
                         //       MyMessageBox.Show(ToDisplay);
                         //       MyMessageBox.Show(capt);
                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);
                                mkvprop.StartInfo.Arguments = @" -y -i " + anyCommand + utfvar + @" -i " + AddDoubleQuotes(newname + @".srt") + @" -map 0:v:0 -map 0:a -map 0:s?  -map 1 -metadata:s:s:" + ToDisplay + @" language=" + slan + @" -vcodec copy -acodec copy " + capt + @" -c:s:1 subrip -disposition:s:" + ToDisplay + @" " + def + @" " + AddDoubleQuotes(dirr + newname + @".mkv");
                            }
                            else
                            {
                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);

                                mkvprop.StartInfo.Arguments = @" -o " + AddDoubleQuotes(dirr + newname + @".mkv") + @" " + anyCommand + @" --forced-track 0:" + forced + @" --default-track 0:" + def + @" --track-name 0:" + langu + @" --language 0:" + slan + @" " + AddDoubleQuotes(newname + aext);

                            }



                            string dir = System.IO.Path.GetDirectoryName(FileName);


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop.Start();
                            //string output = mkvprop.StandardOutput.ReadToEnd();
                            mkvprop.WaitForExit();

                            if (Overwrite.Checked == true)
                            {
                                System.IO.File.Copy(Path.Combine(dir + @"\newMKV\", newname + @".mkv"), Path.Combine(dir, newname + @".mkv"), true);
                                Directory.Delete(dir + @"\newMKV\", true);
                                File.Delete(dir + newname + @".srt");


                                //             DirectoryInfo di = new DirectoryInfo(dir);
                                //              FileInfo[] files = di.GetFiles("*.srt")
                                //                                   .Where(p => p.Extension == ".srt").ToArray();
                                //              foreach (FileInfo file in files)
                                //                  try
                                //                  {
                                //                      file.Attributes = FileAttributes.Normal;
                                //                      File.Delete(file.FullName);
                                //                  }
                                //                  catch { }
                            }

                            pBar1.PerformStep();
                            pBar1.Refresh();
                            int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                            pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                                new Font("Arial", (float)10.25, FontStyle.Bold),
                                Brushes.Black,
                                new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                            cnt++;
                            Cursor.Current = Cursors.Default;
                        }
                        else
                        {



                            
                                au++;

                            

                        }

                    



                    }
                    if (au > 0)
                    {

                        MyMessageBox.Show(au + " files out of " + listBox1.Items.Count + @" don't have audio or Audio files have different name");
                    }

                }
                else
                {
                    MyMessageBox.Show("No Video to Mux subs!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }
        }






        #endregion



























        int i = 0;

        private void radioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (radioButtonList2.SelectedIndex == 1 && i < 1)
            {
                i++;
                //   MessageBox.Show("CAUTION!! If subtitles are Image Type (like pgs) they won't get muxed in mp4");
                MyMessageBox.Show("CAUTION!! Subtitles won't get muxed in mp4");
            }
        }


        #region DarkMode

        public void Initialize_Add()
        {
            panels = new List<Control>();
            buttons = new List<Control>();
            textboxes = new List<Control>();
            listboxes = new List<Control>();
            tabsc = new List<Control>();
            difcol = new List<Control>();
            inputs = new List<Control>();
            cboxes = new List<Control>();








            //panels.Add(containerpanel);
            panels.Add(flowLayoutPanel1);
            panels.Add(flowLayoutPanel3);
            panels.Add(flowLayoutPanel4);
            panels.Add(flowLayoutPanel5);
            panels.Add(flowLayoutPanel6);



            buttons.Add(cmdBrowse);
            buttons.Add(clearfiles);
            buttons.Add(addcover);
            buttons.Add(expcover);
            buttons.Add(removecover);
            buttons.Add(addtitlemeta);
            buttons.Add(addtrackaud);
            buttons.Add(addtracksub);
            buttons.Add(addtrackvid);
            buttons.Add(oneclickconverter);
            buttons.Add(coverLocation);
            buttons.Add(expsub);
            buttons.Add(subbutton);
            buttons.Add(expaudiobut);
            buttons.Add(convaudio);
            buttons.Add(remsubs);
            buttons.Add(remaudio);
            buttons.Add(muxaudio);
            buttons.Add(sortaud);
            buttons.Add(sortsub);
            textboxes.Add(audinfo);
            textboxes.Add(subinfo);

            textboxes.Add(lbl1);
            textboxes.Add(lbl2);
            textboxes.Add(lbl3);
            textboxes.Add(lbl4);
            textboxes.Add(lbl5);
            textboxes.Add(mainMenu1);
            textboxes.Add(label01);
            textboxes.Add(label02);
            textboxes.Add(checkmkv);
            textboxes.Add(checkmp4);
            textboxes.Add(checkavi);
            textboxes.Add(checkts);
            textboxes.Add(Input);
            textboxes.Add(start1);
            textboxes.Add(lblcover);
            cboxes.Add(sl1);
            cboxes.Add(sl2);
            textboxes.Add(sl3);
            textboxes.Add(sl4);
            textboxes.Add(sl5);
            textboxes.Add(sl6);
            textboxes.Add(sl7);
            textboxes.Add(sl8);
            textboxes.Add(sl9);
            textboxes.Add(trackBar1);
            textboxes.Add(lblaudioconv);
            textboxes.Add(videotab);
            textboxes.Add(audiotab);
            textboxes.Add(subtitletab);
            textboxes.Add(radioButtonList1);
            textboxes.Add(radioButtonList2);
            textboxes.Add(sortlbl);


            inputs.Add(titlemeta);
            inputs.Add(numtitlestart);
            inputs.Add(coverLocation.filename);



            listboxes.Add(comboBoxvideotrack);
            listboxes.Add(comboBoxaudiotrack);
            listboxes.Add(comboBoxsubtrack);
            listboxes.Add(languageBar2.language);
            listboxes.Add(languageBar3.language);
            listboxes.Add(audioext);
            listboxes.Add(label2);
            listboxes.Add(label5);
            listboxes.Add(listBox1);
            listboxes.Add(audiobox);
            difcol.Add(checkall);
            difcol.Add(checkall2);


            difcol.Add(groupBox1);
            difcol.Add(editsource);
            difcol.Add(tabControl2);
            difcol.Add(grpBoxTitle);
            difcol.Add(grpBoxAudio);
            difcol.Add(grpBoxVideo);
            difcol.Add(grpBoxSub);
            difcol.Add(Convert);
            difcol.Add(groupBox2);
            difcol.Add(groupBox3);
            difcol.Add(groupBox4);
            difcol.Add(groupBox5);
            difcol.Add(Audio);



            difcol.Add(DefaultSub);
            difcol.Add(ForcedSub);

            tabsc.Add(pBar1);
        }

        public void ApplyTheme(Color inp, Color tab, Color back, Color pan, Color btn, Color tbox, Color lbox, Color combox, Color TextColor, Color difc, Color cb, Color discol)
        {
            this.BackColor = back;
            //      comboBox1.BackColor = combox;
            //      comboBox1.ForeColor = TextColor;
            this.ForeColor = TextColor;

            foreach (Control item in inputs)
            {
                item.BackColor = inp;
                item.ForeColor = difc;
            }

            foreach (Control item in ControlList)
            {
                item.BackColor = inp;
                item.ForeColor = difc;
            }

            foreach (Control item in tabsc)
            {
                item.BackColor = tab;
            }

            foreach (Control item in panels)
            {
                item.BackColor = pan;
            }

            foreach (Control item in buttons)
            {

                item.BackColor = btn;
                item.ForeColor = TextColor;
            }

            foreach (Control item in textboxes)
            {
                item.BackColor = tbox;
                item.ForeColor = TextColor;
            }

            foreach (Control item in listboxes)
            {


                item.BackColor = lbox;
                item.ForeColor = TextColor;

            }

            foreach (Control item in difcol)
            {
                item.BackColor = pan;
                item.ForeColor = difc;

            }

            foreach (Control item in cboxes)
            {

                item.ForeColor = cb;


            }




        }


        public void bunifuiOSSwitch1_OnValueChange_1(object sender, EventArgs e)
        {

            if (sfrm==null)
            {
                
            }
            else
            {
                sfrm.Close();
            }
            if (frm==null)
            {

            }
            else
            {
                frm.Close();

            }




            if (bunifuiOSSwitch1.Value == true)
            {
                Properties.Settings.Default.Setting = true;

                bool currentVal = Properties.Settings.Default.Setting;
                //    MessageBox.Show("The value of SavedSetting1 is '" + currentVal + "'");
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Setting = false;

                bool currentVal = Properties.Settings.Default.Setting;
                //   MessageBox.Show("The value of SavedSetting1 is '" + currentVal + "'");
                Properties.Settings.Default.Save();
            }




            if (bunifuiOSSwitch1.Value == false)
            {
                //        ApplyTheme(Color.White, zcolor(255, 192, 192), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), zcolor(240, 240, 240), Color.White, Color.White, Color.Black, Color.Black, Color.Black, SystemColors.Control) ;
                ApplyTheme(SystemColors.ControlLightLight, zcolor(255, 192, 192), SystemColors.Control, SystemColors.Control, SystemColors.Control, SystemColors.Control, Color.White, SystemColors.Control, SystemColors.ControlText, SystemColors.ControlText, Color.Black, SystemColors.Control);

                label2.Visible = true;
                label5.Visible = true;
            }

            if (bunifuiOSSwitch1.Value == true)
            {
                //  ApplyTheme(zcolor(128, 128, 255), zcolor(51, 51, 51), zcolor(45, 45, 48), zcolor(104, 104, 104), zcolor(51, 51, 51), zcolor(104, 104, 104), Color.Black, Color.White, zcolor(104, 104, 104));
                ApplyTheme(zcolor(98, 114, 164), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, zcolor(189, 147, 249), zcolor(98, 114, 164));

                label2.Visible = false;
                label5.Visible = false;
            }






        }


        Color zcolor(int r, int g, int b)
        {
            return Color.FromArgb(r, g, b);
        }








        public class CustomComboBox : ComboBox
        {
            private const int WM_PAINT = 0xF;
            private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
            new public System.Windows.Forms.DrawMode DrawMode { get; set; }
            private const int WM_SIZE = 0x0005;
            public Color HighlightColor { get; set; }
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == WM_PAINT)
                {
                    using (var g = Graphics.FromHwnd(Handle))
                    {
                        if (Properties.Settings.Default.Setting == true)
                        {
                            using (var p = new Pen(this.BorderColor, 3))
                            {
                                g.DrawRectangle(p, 1, 1, Width - buttonWidth - 2, Height - 2);
                            }

                            g.DrawImageUnscaled(Properties.Resources.dropdown, new Point(Width - buttonWidth - 1));
                        }
                    }
                }



            }
            void CustomComboBox_DrawItem(object sender, DrawItemEventArgs e)
            {
                if (e.Index < 0)
                    return;

                ComboBox combo = sender as ComboBox;
                // combo.BackColor = Color.Red;

                //      combo.ForeColor = Color.Blue;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(HighlightColor),
                                             e.Bounds);
                    e.Graphics.DrawString(((ComboBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Red), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                                             e.Bounds);

                    e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                                          new SolidBrush(combo.ForeColor),
                                          new Point(e.Bounds.X, e.Bounds.Y));
                }
                e.DrawFocusRectangle();




            }

            public CustomComboBox()
            {


                base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                this.HighlightColor = Color.Pink;
                this.DrawItem += new DrawItemEventHandler(CustomComboBox_DrawItem);
                //    this.DropDownStyle = ComboBoxStyle.DropDownList;
                BorderColor = Color.DimGray;

            }
            [Browsable(true)]
            [Category("Appearance")]
            [DefaultValue(typeof(Color), "DimGray")]

            public Color BorderColor { get; set; }



        }







        public partial class NewLabel : Label
        {
            public NewLabel()
            {
                this.ForeColor = SystemColors.ControlText; //SystemColors.InactiveCaption;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                TextRenderer.DrawText(e.Graphics, this.Text.ToString(), this.Font, ClientRectangle, ForeColor);
            }

        }












        public partial class ColorCheckBox : CheckBox
        {


            public ColorCheckBox()
            {
                Appearance = System.Windows.Forms.Appearance.Button;
                FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                TextAlign = ContentAlignment.MiddleLeft;
                FlatAppearance.BorderSize = 0;
                //       AutoSize = false;
                //      Height = 16;
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                //base.OnPaint(pevent);

                pevent.Graphics.Clear(BackColor);

                using (SolidBrush brush = new SolidBrush(ForeColor))
                    pevent.Graphics.DrawString(Text, Font, brush, 2, 4);

                Point pt = new Point(45, 4);
                Rectangle rect = new Rectangle(pt, new Size(12, 12));


                pevent.Graphics.FillRectangle(Brushes.White, rect);

                if (Checked)
                {
                    using (SolidBrush brush = new SolidBrush(BackColor))
                    using (Font wing = new Font("Wingdings", 12f))
                        pevent.Graphics.DrawString("ü", wing, brush, 44, 2);
                }
                pevent.Graphics.DrawRectangle(Pens.DarkSlateBlue, rect);

                Rectangle fRect = ClientRectangle;

                if (Focused)
                {
                    fRect.Inflate(-1, -1);
                    using (Pen pen = new Pen(Brushes.Gray) { DashStyle = DashStyle.Dot })
                        pevent.Graphics.DrawRectangle(pen, fRect);
                }

            }

        }

        //   foreach (ComboBox cbo in (this.Controls.Cast<Control>().Where(c => c is ComboBox).Select(c => (ComboBox) c)))
        //      {
        //          cbo.SelectionLength = 0;
        //      }



        /*          void audiobox_DropDown(object sender, EventArgs e)
                  {
                      // Optionally, revert the color back to the default
                      // when the combobox is dropped-down
                      //
                      // (Note that we're using the ACTUAL default color here,
                      //  rather than hard-coding black)
                      if (bunifuiOSSwitch1.Value == true)
                      {
                          audiobox.ForeColor = Color.White;
                      }
                      else
                      {
                          audiobox.ForeColor = ComboBox.DefaultForeColor;

                      }
                  }
                      void audiobox_DropDownClosed(object sender, EventArgs e)
                  {
                      // Change the color of the selected text in the combobox
                      // to your custom color
                      if (bunifuiOSSwitch1.Value == true)
                      {
                          audiobox.ForeColor = Color.Black;
                      }
                      else
                      {
                          audiobox.ForeColor = Color.Black;

                      }
                      audiobox.SelectionLength = 0;

                  }

          */


        #endregion




        #region Color Background TabsPanels



        private void tabControl1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            TabPage CurrentTab = tabControl1.TabPages[e.Index];
            Rectangle ItemRect = tabControl1.GetTabRect(e.Index);
            ItemRect.Height = 17;
            ItemRect.Inflate(0, 2);
            SolidBrush FillBrush = new SolidBrush(BackColor);
            SolidBrush TextBrush = new SolidBrush(ForeColor);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;





            SolidBrush fillbrush = new SolidBrush(BackColor);



            //draw rectangle behind the tabs
            Rectangle lasttabrect = tabControl1.GetTabRect(tabControl1.TabPages.Count - 1);
            Rectangle background = new Rectangle();
            background.Location = new Point(lasttabrect.Right, 0);

            //pad the rectangle to cover the 1 pixel line between the top of the tabpage and the start of the tabs
            background.Size = new Size(tabControl1.Right - background.Left, lasttabrect.Height + 3);
            e.Graphics.FillRectangle(fillbrush, background);


            //If we are currently painting the Selected TabItem we'll
            //change the brush colors and inflate the rectangle.
            if (System.Convert.ToBoolean(e.State & DrawItemState.Selected))
            {
                FillBrush.Color = BackColor;
                TextBrush.Color = ForeColor;
                ItemRect.Inflate(2, 2);
            }

            //Set up rotation for left and right aligned tabs
            if (tabControl1.Alignment == TabAlignment.Left || tabControl1.Alignment == TabAlignment.Right)
            {
                float RotateAngle = 90;
                if (tabControl1.Alignment == TabAlignment.Left)
                    RotateAngle = 270;
                PointF cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                ItemRect = new Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
            }

            //Next we'll paint the TabItem with our Fill Brush
            e.Graphics.FillRectangle(FillBrush, ItemRect);

            //Now draw the text.
            e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, (RectangleF)ItemRect, sf);

            //Reset any Graphics rotation
            e.Graphics.ResetTransform();




            //Finally, we should Dispose of our brushes.
            FillBrush.Dispose();
            TextBrush.Dispose();
        }



        #endregion






        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            pBar1.Minimum = 0;
            pBar1.Value = 0;
            //   MessageBox.Show("You are in the TabControl.SelectedIndexChanged event.");
        }



        private void tabControl1_Click(object sender, EventArgs e)
        {
            (sender as TabControl).SelectedTab.Focus();
        }


        private void tabControl2_Click(object sender, EventArgs e)
        {
            (sender as TabControl).SelectedTab.Focus();
        }

        private void audiobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox4.Focus();

            //    this.ActiveControl = null;
            //   this.Select(0, 0);

        }

        private void comboBoxvideotrack_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox4.Focus();
            this.ActiveControl = null;
        }

        private void languageBar3_Load(object sender, EventArgs e)
        {
            groupBox4.Focus();

        }


































        #region ColorRadiobutton


        public class ColouredRadioButton : RadioButton
        {
            // Fields
            private Color m_OnColour;
            private Color m_OffColour;
            private Rectangle m_glint;
            private Rectangle m_circle;
            private PathGradientBrush m_flareBrush;
            private Pen m_outline;

            // Properties
            public Color OnColour
            {
                get
                {
                    return m_OnColour;
                }
                set
                {
                    if ((value == Color.White) || (value == Color.Transparent))
                        m_OnColour = Color.Empty;
                    else
                        m_OnColour = value;
                }
            }
            public Color OffColour
            {
                get
                {
                    return m_OffColour;
                }
                set
                {
                    if ((value == Color.White) || (value == Color.Transparent))
                        m_OffColour = Color.Empty;
                    else
                        m_OffColour = value;
                }
            }

            // Constructor
            public ColouredRadioButton()
            {
                // Init
                m_circle = new Rectangle(43, 4, 7, 7 /*Magic Numbers*/);
                m_glint = new Rectangle(43, 4, 4, 4  /*Magic Numbers*/);
                m_outline = new Pen(new SolidBrush(Color.Black), 1F /*Magic Numbers*/);

                // Generate Glint
                GraphicsPath Path = new GraphicsPath();
                Path.AddEllipse(m_glint);
                m_flareBrush = new PathGradientBrush(Path);
                m_flareBrush.CenterColor = Color.White;
                m_flareBrush.SurroundColors = new Color[] { Color.Transparent };
                m_flareBrush.FocusScales = new PointF(0.5F, 0.5F/*Magic Numbers*/);

                // Allows for Overlaying
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
            }

            // Methods
            protected override void OnPaint(PaintEventArgs e)
            {
                // Init
                base.OnPaint(e);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Overlay Graphic
                if (this.Checked)
                {
                    OnColour = Color.Red;
                    if (OnColour != Color.Empty)
                    {
                        g.FillEllipse(new SolidBrush(OnColour), m_circle);
                        g.FillEllipse(m_flareBrush, m_glint);
                        g.DrawEllipse(m_outline, m_circle);
                    }
                }
                else
                {
                    if (OffColour != Color.Empty)
                    {
                        g.FillEllipse(new SolidBrush(OffColour), m_circle);
                        g.FillEllipse(m_flareBrush, m_glint);
                        g.DrawEllipse(m_outline, m_circle);
                    }
                }
            }
        }
























        #endregion







        #region bunifu radio map to radio buttons



        private void chmkv_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked)
            {
                checkmkv.Checked = true;

            }
        }



        private void chmp4_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked)
            {
                checkmp4.Checked = true;


            }
        }



        private void chts_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked)
            {
                checkts.Checked = true;

            }
        }

        private void chavi_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked)
            {
                checkavi.Checked = true;

            }
        }


        private void chaud_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            if (e.Checked)
            {
                checkaud.Checked = true;

            }
        }

        private void lbl1_Click(object sender, EventArgs e)
        {

            chmkv.Checked = true;
            chmp4.Checked = chts.Checked = chavi.Checked = chaud.Checked = false;

        }

        private void lbl2_Click(object sender, EventArgs e)
        {
            chmp4.Checked = true;
            chmkv.Checked = chts.Checked = chavi.Checked = chaud.Checked = false;
        }

        private void lbl3_Click(object sender, EventArgs e)
        {
            chts.Checked = true;
            chmkv.Checked = chmp4.Checked = chavi.Checked = chaud.Checked = false;
        }

        private void lbl4_Click(object sender, EventArgs e)
        {
            chavi.Checked = true;
            chmkv.Checked = chts.Checked = chmp4.Checked = chaud.Checked = false;
        }

        private void lbl5_Click(object sender, EventArgs e)
        {
            chaud.Checked = true;
            chmkv.Checked = chts.Checked = chavi.Checked = chmp4.Checked = false;
        }




        #endregion









        #region Convert Audio


        public string slan2;



        private void convaudio_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (string files in listBox1.Items)
            {
                //       MyMessageBox.Show(Path.GetExtension(files).ToString());
                if (Path.GetExtension(files).ToString() == @"." + audioext.SelectedItem.ToString())
                {
                    i++;
                }

            }



            //   MyMessageBox.Show(i.ToString());


            if (i < 1)
            {


                string br = trackBar1.Value.ToString() + @"k";



                Cursor.Current = Cursors.WaitCursor;

                string ext = @"." + audioext.SelectedItem.ToString();
                string extension2 = "";

                slan2 = languageBar3.language.SelectedItem.ToString();



                foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                {


                    if (slan2.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                    {
                        // int co = Array.IndexOf(slan, x);
                        slan2 = strLanguage.Value.ToString();
                    }


                }


                foreach (KeyValuePair<string, string> straudio in AudioExtContainer.audioextensions)
                {


                    if (ext.ToLowerInvariant().Equals(straudio.Key.ToLowerInvariant()))
                    {
                        // int co = Array.IndexOf(slan, x);
                        extension2 = straudio.Value.ToString();
                    }




                }








                if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.soxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
                {
                    if (listBox1.Items.Count > 0)
                    {




                        //     pBar1.ForeColor = Color.Red;
                        pBar1.BackColor = zcolor(192, 192, 255);
                        // Display the ProgressBar control.
                        pBar1.Visible = true;
                        // Set Minimum to 1 to represent the first file being copied.
                        pBar1.Style = ProgressBarStyle.Continuous;
                        //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                        pBar1.Minimum = 1;
                        // Set Maximum to the total number of files to copy.
                        pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                        // Set the initial value of the ProgressBar.
                        pBar1.Value = 1;
                        // Set the Step property to a value of 1 to represent each file being copied.
                        pBar1.Step = 1;
                        int cnt = 1;


                        string langu = languageBar3.language.SelectedItem.ToString();


                        foreach (string FileName in listBox1.Items)
                        {

                            string anyCommand = AddDoubleQuotes(FileName);
                            /*                    ext = Path.GetExtension(FileName);


                                                if (audiobox.SelectedIndex == 0)
                                                {
                                                    foreach (KeyValuePair<string, string> straudio in AudioExtContainer.audioextensions)
                                                    {

                                                        if (ext.ToLowerInvariant().Equals(straudio.Key.ToLowerInvariant()))
                                                        {
                                                            // int co = Array.IndexOf(slan, x);
                                                            extension2 = straudio.Value.ToString();
                                                        }
                                                    }
                                                }
                        */

                            Process mkvprop = new Process();
                            string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);

                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);
                            string exe = AddDoubleQuotes(mkvprop.StartInfo.FileName);
                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);

                            if (audiobox.SelectedIndex == 0)
                            {
                                mkvprop.StartInfo.Arguments = @" -i " + anyCommand + @" -y temp.wav";

                            }
                            else
                            {
                                mkvprop.StartInfo.Arguments = @" -i " + anyCommand + @" -f sox -y temp.sox";

                            }




                            string dir = System.IO.Path.GetDirectoryName(FileName);


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop.Start();
                            mkvprop.WaitForExit();


                            Process mkvprop2 = new Process();
                            mkvprop2.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.settings.soxPath);
                            mkvprop2.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);



                            if (audiobox.SelectedIndex == 1)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox tempo 1.042708333 ";
                            }
                            else if (audiobox.SelectedIndex == 2)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox tempo 0.959040959 ";

                            }
                            else if (audiobox.SelectedIndex == 3)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox tempo 1.041666666 ";

                            }
                            else if (audiobox.SelectedIndex == 4)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox tempo 0.96 ";

                            }
                            else if (audiobox.SelectedIndex == 5)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox speed 1.042708333 ";

                            }
                            else if (audiobox.SelectedIndex == 6)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox speed 0.959040959 ";

                            }
                            else if (audiobox.SelectedIndex == 7)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox speed 1.041666666 ";

                            }
                            else if (audiobox.SelectedIndex == 8)
                            {
                                mkvprop2.StartInfo.Arguments = @" temp.sox temp2.sox speed 0.96 ";

                            }



                            mkvprop2.StartInfo.CreateNoWindow = true;
                            mkvprop2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop2.Start();
                            mkvprop2.WaitForExit();


                            Process mkvprop3 = new Process();
                            mkvprop3.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.settings.ffmpegPath);
                            mkvprop3.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            string newdir = System.IO.Path.GetDirectoryName(FileName) + @"\[Converted Audio]";
                            FileUtil.ensureDirectoryExists(newdir);



                            if (audioext.SelectedItem.ToString() == "wav")
                            {
                                if (audiobox.SelectedIndex == 0)
                                {
                                    mkvprop3.StartInfo.Arguments = @" -i temp.wav -y " + AddDoubleQuotes(newdir + @"\" + newname + @"-" + slan2 + ext);
                                }
                                else
                                {
                                    mkvprop3.StartInfo.Arguments = @" -i temp2.sox -y " + AddDoubleQuotes(newdir + @"\" + newname + @"-" + slan2 + ext);

                                }

                            }
                            else
                            {
                                if (audiobox.SelectedIndex == 0)
                                {

                                    mkvprop3.StartInfo.Arguments = @" -i temp.wav -acodec " + extension2 + @" -ab " + br + @" -y " + AddDoubleQuotes(newdir + @"\" + newname + @"-" + slan2 + ext);


                                }
                                else
                                {
                                    mkvprop3.StartInfo.Arguments = @" -f sox -i temp2.sox -acodec " + extension2 + @" -ab " + br + @" -y " + AddDoubleQuotes(newdir + @"\" + newname + @"-" + slan2 + ext);
                                }
                            }






                            mkvprop3.StartInfo.CreateNoWindow = true;
                            mkvprop3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop3.Start();
                            mkvprop3.WaitForExit();
                            if (audiobox.SelectedIndex == 0)
                            {
                                File.Delete(mkvprop3.StartInfo.WorkingDirectory + @"\temp.wav");

                            }
                            else
                            {
                                File.Delete(mkvprop3.StartInfo.WorkingDirectory + @"\temp.sox");
                                File.Delete(mkvprop3.StartInfo.WorkingDirectory + @"\temp2.sox");
                            }
                            pBar1.PerformStep();
                            pBar1.Refresh();
                            int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                            pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                                new Font("Arial", (float)10.25, FontStyle.Bold),
                                Brushes.Black,
                                new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                            cnt++;


                            Cursor.Current = Cursors.Default;
                        }



                    }
                    else
                    {

                        MyMessageBox.Show("No Audio to Convert!");
                    }
                }
                else
                {

                    MyMessageBox.Show("Add exe paths in Options first");
                }
            }
            else
            {
                MyMessageBox.Show("You have same INPUT audio files as OUPUT. \nThe will re-encode in same format and loose quality!  :( \nPlease remove them before Converting \n \n");
            }
        }








        internal class NoFocusTrackBar : System.Windows.Forms.TrackBar
        {

            private const int WM_SETFOCUS = 0x0007;

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_SETFOCUS)
                {
                    return;
                }

                base.WndProc(ref m);
            }
        }



















        private int getind(object sender)
        {
            trackBar1.MouseWheel += new MouseEventHandler(trackBar1_MouseWheel);
            int value = (sender as TrackBar).Value;
            double indexDbl = (value * 1.0) / trackBar1.TickFrequency;
            int index = System.Convert.ToInt32(Math.Round(indexDbl));
            return index;

        }












        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // this.ActiveControl = null;

            //  NoFocusTrackBar

            trackBar1.MouseWheel += trackBar1_MouseWheel;

            int value = (sender as TrackBar).Value;
            double indexDbl = (value * 1.0) / trackBar1.TickFrequency;
            int index = System.Convert.ToInt32(Math.Round(indexDbl));
            trackBar1.Value = trackBar1.TickFrequency * index;

            textBox1.Text = trackBar1.Value.ToString();

            //      trackBar1.MouseWheel += trackBar1_MouseWheel;
            //  trackBar1.MouseWheel += new MouseEventHandler(trackBar1_MouseWheel);


        }


        public int text()
        {
            int value = System.Convert.ToInt32(textBox1.Text);
            return value;
        }

        void trackBar1_MouseWheel(object sender, MouseEventArgs e)
        {


            if (text() >= 640)
            {
                trackBar1.Value = 640;
            }
            else if (text() <= 128)
            {
                trackBar1.Value = 128;
            }



            trackBar1.Scroll -= trackBar1_Scroll;

            ((HandledMouseEventArgs)e).Handled = false;//disable default mouse wheel
            if (e.Delta <= 0)
            {

                if (trackBar1.Value < trackBar1.Maximum)
                {

                    trackBar1.Value = text() + 64;

                    textBox1.Text = trackBar1.Value.ToString();

                }
            }
            else
            {

                if (trackBar1.Value > trackBar1.Minimum)
                {

                    trackBar1.Value = text() - 64;

                    textBox1.Text = trackBar1.Value.ToString();

                }
            }





        }




        private void audioext_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox4.Focus();

        }
















        public string slan3;
        public string slan4;

        private void expsub_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            int s = 0;

            slan3 = languageBar2.language.SelectedItem.ToString();
            slan4 = languageBar2.language.SelectedItem.ToString();



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
            {


                if (slan3.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan3 = strLanguage.Value.ToString();
                }


            }



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
            {


                if (slan4.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan4 = strLanguage.Value.ToString();
                }


            }


            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {
                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;


                    string langu = languageBar2.language.SelectedItem.ToString();

                    string info;
                    if (checkmp4.Checked)
                    {
                         info = System.IO.Path.GetDirectoryName(Main.Instance.Settings.mp4boxPath) + @"\mp4box.exe";

                    }
                    else
                    {
                         info = System.IO.Path.GetDirectoryName(Main.Instance.Settings.mkvmergePath) + @"\mkvmergeinfo.exe";
                    }


                    if (!File.Exists(info))
                    {
                        MyMessageBox.Show("Cannot find mkvmergeinfo from sarktools dir");
                    }
                    else
                    {

                        foreach (string FileName in listBox1.Items)
                        {

                            string anyCommand = AddDoubleQuotes(FileName);
                            Process mkvprop = new Process();
                            string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                            //  string sinfo = System.IO.Path.GetDirectoryName(Main.Instance.Settings.mkvmergePath) + @"\sinfo.exe";

                            mkvprop.StartInfo.FileName = info;
                            //  MyMessageBox.Show(mkvprop.StartInfo.FileName);

                            //   mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV";

                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);

                            //      string dirr = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV\";
                            //       FileUtil.ensureDirectoryExists(dirr);


                            if (checkmp4.Checked)
                            {
                                mkvprop.StartInfo.Arguments = @" -info " + anyCommand;

                            }
                            else
                            {
                                mkvprop.StartInfo.Arguments = @" --identify-verbose " + anyCommand;

                            }


                            //    MyMessageBox.Show(mkvprop.StartInfo.Arguments);

                            string dir = System.IO.Path.GetDirectoryName(FileName);


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            //string output = mkvprop.StandardOutput.ReadToEnd();
                            mkvprop.StartInfo.UseShellExecute = false;
                            mkvprop.StartInfo.RedirectStandardOutput = true;

                            mkvprop.Start();

                            List<string> Output = mkvprop.StandardOutput.ReadToEnd().Split('\n').ToList();
                            foreach (string OutPutLine in Output)
                            {

                                   MyMessageBox.Show(OutPutLine.ToString());

                            }
                            mkvprop.WaitForExit();



                            List<string> TrackList1 = new List<string>();
                            List<string> SubTrackList = new List<string>();
                            List<string> DeleteTrackList = new List<string>();


                            int TheIndexOfTrackNum = 0;


                            int gout = 0;

                            foreach (string OutPutLine in Output)
                            {
                                gout = gout + 1;
        //                        MyMessageBox.Show(OutPutLine.ToString());

                                if (checkmp4.Checked)
                                {
           //                             MyMessageBox.Show(OutPutLine.ToString());


                                    if (OutPutLine.Contains("sbtl") || OutPutLine.Contains("text:") || OutPutLine.Contains("subp:"))
                                    {
                                        string tr = Output[gout - 2];
                                        //     var ind = OutPutLine.IndexOf("sbtl");
                                        //   MyMessageBox.Show(OutPutLine[ind].ToString());
                                        int position = tr.IndexOf("#");
                                        TrackList1.Add(tr.Substring(position+1,2));
                                 //       MyMessageBox.Show(TrackList1[0].ToString());
                                        SubTrackList.Add(OutPutLine);

                                   //     MyMessageBox.Show(TrackList1.IndexOf("sbtl",0).ToString());
                             //           MyMessageBox.Show(OutPutLine);
                                    }
                                }
                                else
                                {
                                    if (OutPutLine.Contains("Track") & OutPutLine.Contains("subtitles"))
                                    {

                                        TrackList1.Add(OutPutLine.Substring(TheIndexOfTrackNum, 1));
                                        SubTrackList.Add(OutPutLine);



                                    }

                                }

                            

                            }

                            int i = 0;
                            string keeptrack1 = "";
                            List<string> keeps1 = new List<string>();
                            List<string> keepext1 = new List<string>();
                            List<string> lang = new List<string>();

                            int found = 0;

                            if (checkall.Checked)
                            {
                                if (checkmp4.Checked)
                                {
                                    foreach (string Track in TrackList1)
                                    {
                                        keeps1.Add(Track);
                                    }
                                }



                                foreach (string Track in SubTrackList)
                                {
                                    ++i;

                                    if (!checkmp4.Checked)
                                    {
                                    
                                        keeptrack1 = new string(Track.SkipWhile(c => !char.IsDigit(c))
                                                                           .TakeWhile(c => char.IsDigit(c))
                                                                           .ToArray());

                                        keeps1.Add(keeptrack1);

                                        found = Track.IndexOf("language:");
                                        string end = Track.Substring(found + 9, 3);
                                        lang.Add(end);

                                    }
                                    else
                                    {
                                    //    MyMessageBox.Show(Track);

                                        found = Track.IndexOf("Language");
                                        string end = Track.Substring(found + 10, 3);
                                        lang.Add(end);
                                    //    MyMessageBox.Show(end);
                                    }






                                    
                                   


                                    if (Track.Contains("PGS"))
                                    {
                                        keepext1.Add(".sup");
                                    }
                                    else if (Track.Contains("SSA"))
                                    {
                                        keepext1.Add(".ssa");

                                    }
                                    else if (Track.Contains("ASS"))
                                    {
                                        keepext1.Add(".ass");

                                    }
                                    else if (Track.Contains("S_TEXT/UTF8") || Track.Contains("S_TEXT / ASCII") || Track.Contains("tx3g"))
                                    {
                                        keepext1.Add(".srt");

                                    }
                                    else if (Track.Contains("VOBSUB"))
                                    {
                                        keepext1.Add(".sub");

                                    }
                                    else if (Track.Contains("WEBVTT"))
                                    {
                                        keepext1.Add(".webvtt");

                                    }
                                    else if (Track.Contains("mp4s"))
                                    {
                                        keepext1.Add("");

                                    }

                                }



                                for (int j = 1; j <= i; j++)
                                {
                                    string num;
                                    if (j == 1)
                                    {
                                        num = "";
                                    }
                                    else
                                    {
                                        num = j.ToString();
                                    }


                                    string sub = keeps1.ElementAt(j - 1);
                                    string ex1 = keepext1.ElementAt(j - 1);
                                    string lan = lang.ElementAt(j - 1);

                                    Process execute = new Process();
                         //           execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);
                                    execute.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                    FileUtil.ensureDirectoryExists(execute.StartInfo.WorkingDirectory);
                                    string subdir = System.IO.Path.GetDirectoryName(FileName) + @"\[subs]";
                                    FileUtil.ensureDirectoryExists(subdir);

                                    if(checkmp4.Checked)
                                    {
                                        execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mp4boxPath);
                                        execute.StartInfo.Arguments = @" -raw " + sub + @" " + anyCommand + @" -out " +  AddDoubleQuotes(subdir + @"\" + newname + @"-track" + sub + @"-" + lan + ex1);

                                    }
                                    else
                                    {
                                        execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);

                                        execute.StartInfo.Arguments = @" tracks " + anyCommand + @" " + sub + @":" + AddDoubleQuotes(subdir + @"\" + newname + @"-track" + sub + @"-" + lan + ex1);

                                    }
                            //            MyMessageBox.Show(execute.StartInfo.Arguments);
                                    execute.StartInfo.CreateNoWindow = true;
                                    execute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    execute.Start();
                                    execute.WaitForExit();


                                }









                            }
                            else
                            {
                                int fi = 0;

                                foreach (string Track in SubTrackList)
                                {

                                    if (Track.Contains(@":" + langu) || Track.Contains(@":" + slan3) || Track.Contains(@":" + slan4) || Track.Contains(@"Language "+@"""" + langu))
                                    {
                                        ++i;
                                        ++fi;

                                        if (!checkmp4.Checked)
                                        { 

                                        keeptrack1 = new string(Track.SkipWhile(c => !char.IsDigit(c))
                                        .TakeWhile(c => char.IsDigit(c))
                                        .ToArray()); ;


                                        keeps1.Add(keeptrack1);


                                         }
                                         else
                                        {
                                      //      MyMessageBox.Show(TrackList1[fi -1].ToString());
                                            keeps1.Add(TrackList1[fi-1]);
                                        }













                                    if (Track.Contains("PGS"))
                                        {
                                            keepext1.Add(".sup");
                                        }
                                        else if (Track.Contains("SSA"))
                                        {
                                            keepext1.Add(".ssa");

                                        }
                                        else if (Track.Contains("ASS"))
                                        {
                                            keepext1.Add(".ass");

                                        }
                                        else if (Track.Contains("S_TEXT/UTF8") || Track.Contains("S_TEXT / ASCII") || Track.Contains("tx3g"))
                                        {
                                            keepext1.Add(".srt");

                                        }
                                        else if (Track.Contains("VOBSUB"))
                                        {
                                            keepext1.Add(".sub");

                                        }
                                        else if (Track.Contains("WEBVTT"))
                                        {
                                            keepext1.Add(".webvtt");

                                        }



                                    }



                                }





                                if (i < 1)
                                {
                                    s++;
                                }
                                else
                                {
                                    for (int j = 1; j <= i; j++)
                                    {
                                        string num;
                                        if (j == 1)
                                        {
                                            num = "";
                                        }
                                        else
                                        {
                                            num = j.ToString();
                                        }


                                        string sub = keeps1.ElementAt(j - 1);
                                        string ex1 = keepext1.ElementAt(j - 1);

                                        Process execute = new Process();
                                        execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);
                                        execute.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                        FileUtil.ensureDirectoryExists(execute.StartInfo.WorkingDirectory);
                                        string subdir = System.IO.Path.GetDirectoryName(FileName) + @"\[subs]";
                                        FileUtil.ensureDirectoryExists(subdir);





                                        if (checkmp4.Checked)
                                        {
                                 //           MyMessageBox.Show(slan4);
                                            execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mp4boxPath);
                                            execute.StartInfo.Arguments = @" -srt " + sub + @" " + anyCommand + @" -out " + AddDoubleQuotes(subdir + @"\" + newname + @"-track" + sub + @"-" + slan4 + ex1);

                                        }
                                        else
                                        {
                                            execute.StartInfo.Arguments = @" tracks " + anyCommand + @" " + sub + @":" + AddDoubleQuotes(subdir + @"\" + newname + @"-" + slan4 + num + ex1);
                                        }
                                            
                                            //    MyMessageBox.Show(execute.StartInfo.Arguments);
                                        execute.StartInfo.CreateNoWindow = true;
                                        execute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                        execute.Start();
                                        execute.WaitForExit();


                                    }
                                }
                            }

                            pBar1.PerformStep();
                            pBar1.Refresh();
                            int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                            pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                                new Font("Arial", (float)10.25, FontStyle.Bold),
                                Brushes.Black,
                                new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                            cnt++;
                        }
                        if (s > 0)
                        {
                            MyMessageBox.Show("No subtitles in " + langu + " found in " + s + " files.");

                        }

                    }
                }
                else
                {
                    MyMessageBox.Show("No Video to Mux subs!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }
        }



        public string slan5;
        public string slan6;

        private void expaudiobut_Click(object sender, EventArgs e)
        {

            int s = 0;
            slan5 = languageBar3.language.SelectedItem.ToString();
            slan6 = languageBar3.language.SelectedItem.ToString();



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
            {


                if (slan5.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan5 = strLanguage.Value.ToString();
                }


            }



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
            {


                if (slan6.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan6 = strLanguage.Value.ToString();
                }


            }


            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {
                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;


                    string langu = languageBar3.language.SelectedItem.ToString();

                    string info = System.IO.Path.GetDirectoryName(Main.Instance.Settings.mkvmergePath) + @"\mkvmergeinfo.exe";
                    if (!File.Exists(info))
                    {
                        MyMessageBox.Show("You have accidentally delete sinfo from sarktools");
                    }
                    else
                    {

                        foreach (string FileName in listBox1.Items)
                        {

                            string anyCommand = AddDoubleQuotes(FileName);
                            Process mkvprop = new Process();
                            string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                            mkvprop.StartInfo.FileName = info;
                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);

                            //      string dirr = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV\";
                            //       FileUtil.ensureDirectoryExists(dirr);
                            mkvprop.StartInfo.Arguments = @" --identify-verbose " + anyCommand;
                            //    MyMessageBox.Show(mkvprop.StartInfo.Arguments);

                            string dir = System.IO.Path.GetDirectoryName(FileName);


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop.StartInfo.UseShellExecute = false;
                            mkvprop.StartInfo.RedirectStandardOutput = true;

                            mkvprop.Start();

                            List<string> Output = mkvprop.StandardOutput.ReadToEnd().Split('\n').ToList();

                            mkvprop.WaitForExit();



                            List<string> TrackList2 = new List<string>();
                            List<string> AudTrackList = new List<string>();
                            List<string> DeleteTrackList = new List<string>();
                            List<string> audiotypes = new List<string>();

                            //     string[] keeptrack2;
                            //    string[] keepext2;
                            //    keeptrack2[0] = "";
                            //     keepext2[0] = "";

                            int TheIndexOfTrackNum = 0;

                            foreach (string OutPutLine in Output)
                            {
                                if (OutPutLine.Contains("Track") && OutPutLine.Contains("audio"))
                                {
                                    TrackList2.Add(OutPutLine.Substring(TheIndexOfTrackNum, 1));
                                    AudTrackList.Add(OutPutLine);


                                }
                            }


                            int i = 0;
                            string keeptrack2 = "";
                            List<string> keeps2 = new List<string>();
                            List<string> keepext2 = new List<string>();
                            List<string> lang = new List<string>();

                            int found = 0;

                            if (checkall2.Checked)
                            {
                                foreach (string Track in AudTrackList)
                                {
                                    ++i;
                                    keeptrack2 = new string(Track.SkipWhile(c => !char.IsDigit(c))
                                    .TakeWhile(c => char.IsDigit(c))
                                    .ToArray());

                                    keeps2.Add(keeptrack2);

                                    found = Track.IndexOf("language:");
                                    string end = Track.Substring(found + 9, 3);
                                    lang.Add(end);


                                    if (Track.Contains("AC3")|| Track.Contains("AC-3"))
                                    {
                                        keepext2.Add(".ac3");
                                    }
                                    else if (Track.Contains("AAC"))
                                    {
                                        keepext2.Add(".aac");

                                    }
                                    else if (Track.Contains("MPEG"))
                                    {
                                        keepext2.Add(".mp3");

                                    }
                                    else if (Track.Contains("DTS"))
                                    {
                                        keepext2.Add(".dts");

                                    }
                                    else if (Track.Contains("A_PCM"))
                                    {
                                        keepext2.Add(".wav");

                                    }
                                    else if (Track.Contains("MP2"))
                                    {
                                        keepext2.Add(".mp3");

                                    }
                                    

                                }




                                for (int j = 1; j <= i; j++)
                                {
                                    string num;
                                    if (j == 1)
                                    {
                                        num = "";
                                    }
                                    else
                                    {
                                        num = j.ToString();
                                    }


                                    string aud = keeps2.ElementAt(j - 1);
                                    string ex2 = keepext2.ElementAt(j - 1);
                                    string lan = lang.ElementAt(j - 1);

                                    Process execute = new Process();
                               //     execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);
                                    execute.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                    FileUtil.ensureDirectoryExists(execute.StartInfo.WorkingDirectory);
                                    string newdir = System.IO.Path.GetDirectoryName(FileName) + @"\[Exported Audio]";
                                    FileUtil.ensureDirectoryExists(newdir);





                                    if (checkmp4.Checked)
                                    {
                                                                                                       
                                     var aud1 = Int32.Parse(aud)-1;

                              //       MyMessageBox.Show(aud.ToString());
                              //          MyMessageBox.Show(lan);



                                        execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);
                                      execute.StartInfo.Arguments = @" -y -i " + anyCommand + @" -vn -acodec copy -map 0:" + aud + @" " + AddDoubleQuotes(newdir + @"\" + newname + @"-track" + aud + @"-" + lan + ex2);

                          //              execute.StartInfo.Arguments = @" -i " + anyCommand + @" -c copy -map 0:a:" + aud1 + @" " + AddDoubleQuotes(newdir + @"\" + newname + @"-track" + aud + @"-" + lan + ex2);



                                    }
                                    else if (checkmkv.Checked)
                                    {
                                      execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);

                                      execute.StartInfo.Arguments = @" tracks " + anyCommand + @" " + aud + @":" + AddDoubleQuotes(newdir + @"\" + newname + @"-track" + aud + @"-" + lan + ex2);



                                    }



                                    execute.StartInfo.CreateNoWindow = true;
                                    execute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    execute.Start();
                                    execute.WaitForExit();


                                }

                            }
                            else
                            {



                                foreach (string Track in AudTrackList)
                                {
                                    if (Track.Contains(@":" + langu) || Track.Contains(@":" + slan5) || Track.Contains(@":" + slan6))
                                    {
                                        keeptrack2 = new string(Track.SkipWhile(c => !char.IsDigit(c))
                                       .TakeWhile(c => char.IsDigit(c))
                                       .ToArray());

                                        keeps2.Add(keeptrack2);



                                        if (Track.Contains("AC3") || Track.Contains("AC-3"))
                                        {
                                            keepext2.Add(".ac3");
                                        }
                                        else if (Track.Contains("AAC"))
                                        {
                                            keepext2.Add(".aac");

                                        }
                                        else if (Track.Contains("MPEG"))
                                        {
                                            keepext2.Add(".mp3");

                                        }
                                        else if (Track.Contains("DTS"))
                                        {
                                            keepext2.Add(".dts");

                                        }
                                        else if (Track.Contains("A_PCM"))
                                        {
                                            keepext2.Add(".wav");

                                        }
                                        else if (Track.Contains("MP2"))
                                        {
                                            keepext2.Add(".mp3");

                                        }


                                        ++i;


                                    }



                                }



                                if (i < 1)
                                {
                                    s++;
                                }
                                else
                                {
                                    for (int j = 1; j <= i; j++)
                                    {
                                        string num;
                                        if (j == 1)
                                        {
                                            num = "";
                                        }
                                        else
                                        {
                                            num = j.ToString();
                                        }


                                        string aud = keeps2.ElementAt(j - 1);
                                        string ex2 = keepext2.ElementAt(j - 1);

                                        Process execute = new Process();
                                        execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvextPath);
                                        execute.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                        FileUtil.ensureDirectoryExists(execute.StartInfo.WorkingDirectory);
                                        string newdir = System.IO.Path.GetDirectoryName(FileName) + @"\[Exported Audio]";
                                        FileUtil.ensureDirectoryExists(newdir);




                                        if (checkmp4.Checked)
                                        {
                                            //       MyMessageBox.Show(aud);



                                            var aud1 = Int32.Parse(aud) - 1;
                                            execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);
                                      //      execute.StartInfo.Arguments = @" -i " + anyCommand + @" -c copy -map 0:a:" + aud1 + @" " + AddDoubleQuotes(newdir + @"\" + newname + @"-track" + aud + @"-" + slan6 + num + ex2);

                                            execute.StartInfo.Arguments = @" -y -i " + anyCommand + @" -vn -acodec copy -map 0:" + aud + @" " + AddDoubleQuotes(newdir + @"\" + newname + @"-track" + aud + @"-" + lan + ex2);


                                        }
                                        else
                                        {
                                            execute.StartInfo.Arguments = @" tracks " + anyCommand + @" " + aud + @":" + AddDoubleQuotes(newdir + @"\" + newname + @"-" + slan6 + num + ex2);

                                        }



                                        execute.StartInfo.CreateNoWindow = true;
                                        execute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                        execute.Start();
                                        execute.WaitForExit();


                                    }
                                }


                            }
                            pBar1.PerformStep();
                            pBar1.Refresh();
                            int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                            pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                            new Font("Arial", (float)10.25, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                            cnt++;

                        }
                        if (s > 0)
                        {
                            MyMessageBox.Show("No Audio in " + langu + " found in " + s + " files.");

                        }

                    }
                }
                else
                {
                    MyMessageBox.Show("No Video to Export Audio!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }
        }

        private void flowLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {
            if (bunifuiOSSwitch1.Value == true)
            {
                if (groupBox3.Enabled == true)
                {
                    ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel5.ClientRectangle,
                    Color.Pink, 1, ButtonBorderStyle.Solid, // left
                    Color.Pink, 1, ButtonBorderStyle.Solid, // top
                    Color.Pink, 1, ButtonBorderStyle.Solid, // right
                    Color.Pink, 1, ButtonBorderStyle.Solid);// bottom 
                }
                else
                {
                    ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel5.ClientRectangle,
                    Color.Black, 1, ButtonBorderStyle.Solid, // left
                    Color.Black, 1, ButtonBorderStyle.Solid, // top
                    Color.Black, 1, ButtonBorderStyle.Solid, // right
                    Color.Black, 1, ButtonBorderStyle.Solid);// bottom 
                }

            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel5.ClientRectangle,
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid, // left
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid, // top
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid, // right
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid);// bottom 
            }



        }

        private void flowLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {
            if (bunifuiOSSwitch1.Value == true)
            {
                if (audiomuxpanel.Enabled == true)
                {
                    ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel6.ClientRectangle,
                    Color.Pink, 1, ButtonBorderStyle.Solid, // left
                    Color.Pink, 1, ButtonBorderStyle.Solid, // top
                    Color.Pink, 1, ButtonBorderStyle.Solid, // right
                    Color.Pink, 1, ButtonBorderStyle.Solid);// bottom 
                    label10.ForeColor = Color.Pink;
                    label9.ForeColor = Color.Pink;

                }
                else
                {
                    ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel6.ClientRectangle,
                    Color.Black, 1, ButtonBorderStyle.Solid, // left
                    Color.Black, 1, ButtonBorderStyle.Solid, // top
                    Color.Black, 1, ButtonBorderStyle.Solid, // right
                    Color.Black, 1, ButtonBorderStyle.Solid);// bottom 
                    label10.ForeColor = SystemColors.ControlDark;
                    label9.ForeColor = SystemColors.ControlDark;

                }

            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, flowLayoutPanel6.ClientRectangle,
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid, // left
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid, // top
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid, // right
                SystemColors.ControlText, 1, ButtonBorderStyle.Solid);// bottom 
                if (audiomuxpanel.Enabled == true)
                {
                    label10.ForeColor = SystemColors.ControlText;
                    label9.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    label10.ForeColor = SystemColors.ControlDark;
                    label9.ForeColor = SystemColors.ControlDark;
                }
            }

        }

        private void remsubs_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            int s = 0;

            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {


                slan3 = languageBar2.language.SelectedItem.ToString();
                slan4 = languageBar2.language.SelectedItem.ToString();



                foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
                {


                    if (slan3.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                    {
                        // int co = Array.IndexOf(slan, x);
                        slan3 = strLanguage.Value.ToString();
                    }


                }



                foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
                {


                    if (slan4.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                    {
                        // int co = Array.IndexOf(slan, x);
                        slan4 = strLanguage.Value.ToString();
                    }


                }





                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;


                    string langu = languageBar2.language.SelectedItem.ToString();
                    string info = System.IO.Path.GetDirectoryName(Main.Instance.Settings.mkvmergePath) + @"\mkvmergeinfo.exe";
                    int fc = 0;


                    foreach (string FileName in listBox1.Items)
                    {

                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                        mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                        FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);



                        if (checkall.Checked == true)
                        {

                            mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);

                            string dirr = System.IO.Path.GetDirectoryName(FileName) + @"\[Videos with Removed Subs]\";
                            FileUtil.ensureDirectoryExists(dirr);


                            mkvprop.StartInfo.Arguments = @" -o " + AddDoubleQuotes(dirr + newname + @".mkv") + @" --no-subtitles " + anyCommand;


                            string dir = System.IO.Path.GetDirectoryName(FileName);


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop.Start();
                            //string output = mkvprop.StandardOutput.ReadToEnd();
                            mkvprop.WaitForExit();



                        }
                        else
                        {
                            
                            if (checkmp4.Checked)
                            {
                                fc++;
                                if ( fc == listBox1.Items.Count)
                                {
                                    MyMessageBox.Show("mp4 files support only remove ALL subs. Please check the box 'All Subs'");
                                }
                                

                            }
                            else
                            {



                                mkvprop.StartInfo.FileName = info;


                                mkvprop.StartInfo.Arguments = @" --identify-verbose " + anyCommand;

                                string dir = System.IO.Path.GetDirectoryName(FileName);


                                mkvprop.StartInfo.CreateNoWindow = true;
                                mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                mkvprop.StartInfo.UseShellExecute = false;
                                mkvprop.StartInfo.RedirectStandardOutput = true;

                                mkvprop.Start();

                                List<string> Output = mkvprop.StandardOutput.ReadToEnd().Split('\n').ToList();

                                mkvprop.WaitForExit();



                                List<string> TrackList1 = new List<string>();
                                List<string> SubTrackList = new List<string>();
                                List<string> DeleteTrackList = new List<string>();


                                int TheIndexOfTrackNum = 0;




                                foreach (string OutPutLine in Output)
                                {
                                    if (OutPutLine.Contains("Track") & OutPutLine.Contains("subtitles"))
                                    {

                                        TrackList1.Add(OutPutLine.Substring(TheIndexOfTrackNum, 1));
                                        SubTrackList.Add(OutPutLine);



                                    }

                                }

                                int i = 0;
                               
                                string keeptrack1 = "";
                                List<string> keeps1 = new List<string>();

                                foreach (string Track in SubTrackList)
                                {
                                    if (Track.Contains(@":" + langu) || Track.Contains(@":" + slan3) || Track.Contains(@":" + slan4) || Track.Contains(@"Language " + @"""" + langu))
                                    {
                                        i++;
                                    }
                                    else
                                    {
                                        
                                            keeptrack1 = new string(Track.SkipWhile(c => !char.IsDigit(c))
                                          .TakeWhile(c => char.IsDigit(c))
                                          .ToArray());

                                            keeps1.Add(keeptrack1);
                                      







                                    }


                                }

                                if (i < 1)
                                {
                                    s++;
                                }
                                else
                                {

                                    string TrackList2Keep = "";

                                    foreach (string Track in keeps1)
                                    {
                                        TrackList2Keep = TrackList2Keep + Track + ",";
                                    }

                                




                                    Process execute = new Process();
                                    //            execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);
                                    execute.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                    FileUtil.ensureDirectoryExists(execute.StartInfo.WorkingDirectory);
                                    string subdir = System.IO.Path.GetDirectoryName(FileName) + @"\[Videos with Removed Subs]\";
                                    FileUtil.ensureDirectoryExists(subdir);
                                    execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);


                                    string Tracks;
                                    if (TrackList2Keep.Length > 0)
                                    {
                                        Tracks = TrackList2Keep.Substring(0, TrackList2Keep.Length - 1);
                                        execute.StartInfo.Arguments = @" -o " + AddDoubleQuotes(subdir + newname + @".mkv") + @" -s " + Tracks + @" " + anyCommand;

                                    }
                                    else
                                    {
                                        execute.StartInfo.Arguments = @" -o " + AddDoubleQuotes(subdir + newname + @".mkv") + @" --no-subtitles " + anyCommand;

                                    }




                                    //    MyMessageBox.Show(execute.StartInfo.Arguments);
                                    execute.StartInfo.CreateNoWindow = true;
                                    execute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    execute.Start();
                                    execute.WaitForExit();



                                }
                            }

                        }
                        pBar1.PerformStep();
                        pBar1.Refresh();
                        int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                        pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                            new Font("Arial", (float)10.25, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                        cnt++;

                        Cursor.Current = Cursors.Default;


                    }

                    if (s > 0)
                    {
                        MyMessageBox.Show("No subtitles in " + langu + " found in " + s + " files.");

                    }
                }
                else
                {
                    MyMessageBox.Show("No Video to Remove subs!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }
        }





        public suinfo sfrm;
        public bool sform2IsOpen = false; //{ get; set; }

        public Point m_sPreviousLocation = new Point(int.MinValue, int.MinValue);

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            if (sform2IsOpen == false)
            {
                sfrm = new suinfo(listBox1);

                if (bunifuiOSSwitch1.Value == true)
                {
                    textboxes.Add(sfrm.richTextBox2);
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);
                }

                sfrm.StartPosition = FormStartPosition.Manual;
                sfrm.Location = new Point(m_sPreviousLocation.X + 490, m_sPreviousLocation.Y + 66);
                sfrm.Show();
                sform2IsOpen = true;

            }
            else
            {
                sfrm.Close();
                sform2IsOpen = false;
            }
        }



        


        /*     private void Disable_MouseWheel(object sender, EventArgs e)
       {
           //  HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
           //   ee.Handled = true;
           //     int value = (sender as TrackBar).Value;
           //     double indexDbl = (value * 1.0) / trackBar1.TickFrequency;
           //      int index = System.Convert.ToInt32(Math.Round(indexDbl));

           //    trackBar1.Value = trackBar1.Minimum;

            if (trackBar1.Value < trackBar1.Maximum)
           {
               if (trackBar1.Value <= trackBar1.Minimum)
               {
                   trackBar1.Value = trackBar1.Minimum;
               }
               else
               {
                   trackBar1.Value = trackBar1.LargeChange;
               }
           }
       }

       */














        #endregion







        //     public string soundvalue()
        //      {

        //        string value = bunifuHSlider1.Value.ToString();

        //         return value;


        //     }

























        //      textBox1.Text = bunifuHSlider1.Value.ToString();














        public class CustomPanel : Panel
        {
            public CustomPanel()
            {
                // this.BackColor = Color.LightSeaGreen;
                //    SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {

                Pen greenPen = new Pen(Color.LightSeaGreen);
                greenPen.DashStyle = DashStyle.Dash;


                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                e.Graphics.DrawRectangle(greenPen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            }

        }


















        private void remaudio_Click(object sender, EventArgs e)
        {
         //   if (checkmp4.Checked)
         //   {
         //       MyMessageBox.Show("The new mp4 files with removed audio will convert automatically in mkv");
         //   }

            slan5 = languageBar3.language.SelectedItem.ToString();
            slan6 = languageBar3.language.SelectedItem.ToString();
            int s = 0;



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.LanguagesTerminology)
            {


                if (slan5.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan5 = strLanguage.Value.ToString();
                }


            }



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
            {


                if (slan6.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan6 = strLanguage.Value.ToString();
                }


            }


            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {
                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;


                    string langu = languageBar3.language.SelectedItem.ToString();
                    string info = System.IO.Path.GetDirectoryName(Main.Instance.Settings.mkvmergePath) + @"\mkvmergeinfo.exe";

                    if (!File.Exists(info))
                    {
                        MyMessageBox.Show("You have accidentally delete sinfo from sarktools");
                    }
                    else
                    {
                        int fc = 0;

                        foreach (string FileName in listBox1.Items)
                        {

                            string anyCommand = AddDoubleQuotes(FileName);
                            Process mkvprop = new Process();
                            string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                            mkvprop.StartInfo.FileName = info;
                            mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                            FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);



                            if (checkall2.Checked == true)
                            {
                        //        mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);

                                string dirr = System.IO.Path.GetDirectoryName(FileName) + @"\[Videos with Removed Audio]\";
                                FileUtil.ensureDirectoryExists(dirr);


                                if (checkmp4.Checked)
                                {
                                    mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);
                                    mkvprop.StartInfo.Arguments = @" -i " + anyCommand + @" -map 0 -c copy -an " + AddDoubleQuotes(dirr + newname + @".mp4");
                                }
                                else
                                {
                                    mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);
                                    mkvprop.StartInfo.Arguments = @" -o " + AddDoubleQuotes(dirr + newname + @".mkv") + @" --no-audio " + anyCommand;

                                }


                                mkvprop.StartInfo.CreateNoWindow = true;
                                mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                mkvprop.Start();
                                mkvprop.WaitForExit();

                            }
                            else
                            {

                                if (checkmp4.Checked)
                                {
                                    fc++;
                                    if (fc == listBox1.Items.Count)
                                    {
                                        MyMessageBox.Show("mp4 files support only remove ALL Audio. Please check the box 'All Aud'");
                                    }


                                }
                                else
                                {


                                    mkvprop.StartInfo.FileName = info;



                                    mkvprop.StartInfo.Arguments = @" --identify-verbose " + anyCommand;


                                    string dir = System.IO.Path.GetDirectoryName(FileName);


                                    mkvprop.StartInfo.CreateNoWindow = true;
                                    mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    mkvprop.StartInfo.UseShellExecute = false;
                                    mkvprop.StartInfo.RedirectStandardOutput = true;

                                    mkvprop.Start();

                                    List<string> Output = mkvprop.StandardOutput.ReadToEnd().Split('\n').ToList();

                                    mkvprop.WaitForExit();



                                    List<string> TrackList2 = new List<string>();
                                    List<string> AudTrackList = new List<string>();
                                    List<string> DeleteTrackList = new List<string>();
                                    List<string> audiotypes = new List<string>();



                                    int TheIndexOfTrackNum = 0;

                                    foreach (string OutPutLine in Output)
                                    {
                                        if (OutPutLine.Contains("Track") && OutPutLine.Contains("audio"))
                                        {
                                            TrackList2.Add(OutPutLine.Substring(TheIndexOfTrackNum, 1));
                                            AudTrackList.Add(OutPutLine);


                                        }
                                    }


                                    int i = 0;
                                    string keeptrack2 = "";
                                    List<string> keeps2 = new List<string>();
                                    List<string> keepext2 = new List<string>();

                                    foreach (string Track in AudTrackList)
                                    {
                                        if (Track.Contains(@":" + langu) || Track.Contains(@":" + slan5) || Track.Contains(@":" + slan6))
                                        {
                                            i++;
                                        }
                                        else
                                        {
                                            keeptrack2 = new string(Track.SkipWhile(c => !char.IsDigit(c))
                                                                                   .TakeWhile(c => char.IsDigit(c))
                                                                                   .ToArray());

                                            keeps2.Add(keeptrack2);
                                        }

                                    }


                                    if (i < 1)
                                    {
                                        s++;

                                    }
                                    else
                                    {
                                        string TrackList2Keep = "";
                                        string Tracks = "";
                                        foreach (string Track in keeps2)
                                        {
                                            TrackList2Keep = TrackList2Keep + Track + ",";
                                        }
                                        if (TrackList2Keep.Length > 0)
                                        {
                                            Tracks = TrackList2Keep.Substring(0, TrackList2Keep.Length - 1);
                                        }
                                        else
                                        {
                                            Tracks = "";

                                        }

                                        Process execute = new Process();
                                        execute.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);
                                        execute.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                                        FileUtil.ensureDirectoryExists(execute.StartInfo.WorkingDirectory);
                                        string subdir = System.IO.Path.GetDirectoryName(FileName) + @"\[Videos with Removed Audio]\";
                                        FileUtil.ensureDirectoryExists(subdir);

                                        if (Tracks == "")
                                        {
                                            execute.StartInfo.Arguments = @" -o " + AddDoubleQuotes(subdir + newname + @".mkv") + @" --no-audio " + anyCommand;

                                        }
                                        else
                                        {
                                            execute.StartInfo.Arguments = @" -o " + AddDoubleQuotes(subdir + newname + @".mkv") + @" -a " + Tracks + @" " + anyCommand;

                                        }
                                        execute.StartInfo.CreateNoWindow = true;
                                        execute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                        execute.Start();
                                        execute.WaitForExit();

                                    }
                                }
                            }

                            pBar1.PerformStep();
                            pBar1.Refresh();
                            int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                            pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                                new Font("Arial", (float)10.25, FontStyle.Bold),
                                Brushes.Black,
                                new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                            cnt++;
                            Cursor.Current = Cursors.Default;

                        }
                        if (s > 0)
                        {
                            MyMessageBox.Show("No Audio in " + langu + " found in " + s + " files.");

                        }

                    }

                }
                else
                {
                    MyMessageBox.Show("No Video to Remove Audio!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }
        }



















        public string aext;




        private void muxaudio_Click(object sender, EventArgs e)
        {
            if (checkmp4.Checked)
            {
                MyMessageBox.Show("The new mp4 files with muxed audio will convert automatically in mkv");
            }

                Cursor.Current = Cursors.WaitCursor;


            string def;
            if (defaud.Checked == true)
            {

                if (checkmp4.Checked)
                {
                    def = "default";
                }
                else
                {
                    def = "yes";
                }

            }
            else
            {

                if (checkmp4.Checked)
                {
                    def = "0";
                }
                else
                {
                    def = "no";
                }
            }

            string forced;

            if (forcaud.Checked == true)
            {
                forced = "yes";
            }
            else
            {
                forced = "no";
            }
















            slan = languageBar3.language.SelectedItem.ToString();



            foreach (KeyValuePair<string, string> strLanguage in LanguageSelectionContainer.Languages)
            {


                if (slan.ToLowerInvariant().Equals(strLanguage.Key.ToLowerInvariant()))
                {
                    // int co = Array.IndexOf(slan, x);
                    slan = strLanguage.Value.ToString();
                }


            }

            if (System.IO.Path.GetFileName(Main.Instance.Settings.mkvmergePath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.mp4boxPath) != " " & System.IO.Path.GetFileName(Main.Instance.Settings.ffmpegPath) != " ")
            {
                if (listBox1.Items.Count > 0)
                {

                    //     pBar1.ForeColor = Color.Red;
                    pBar1.BackColor = zcolor(192, 192, 255);
                    // Display the ProgressBar control.
                    pBar1.Visible = true;
                    // Set Minimum to 1 to represent the first file being copied.
                    pBar1.Style = ProgressBarStyle.Continuous;
                    //   pBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
                    pBar1.Minimum = 1;
                    // Set Maximum to the total number of files to copy.
                    pBar1.Maximum = (int)(listBox1.Items.Count + 1);
                    // Set the initial value of the ProgressBar.
                    pBar1.Value = 1;
                    // Set the Step property to a value of 1 to represent each file being copied.
                    pBar1.Step = 1;
                    int cnt = 1;


                    string langu = languageBar3.language.SelectedItem.ToString();

                    int au = 0;
                    foreach (string FileName in listBox1.Items)
                    {

                        string anyCommand = AddDoubleQuotes(FileName);
                        Process mkvprop = new Process();
                        string newname = System.IO.Path.GetFileNameWithoutExtension(FileName);
                        //         mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);


                        //   mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV";

                        mkvprop.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(FileName);
                        FileUtil.ensureDirectoryExists(mkvprop.StartInfo.WorkingDirectory);

                        string dirr = System.IO.Path.GetDirectoryName(FileName) + @"\newMKV\";
                        FileUtil.ensureDirectoryExists(dirr);

                        string info = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".aac";
                        string info2 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".ac3";
                        string info3 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".wav";
                        string info4 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".mp3";
                        string info5 = System.IO.Path.GetDirectoryName(FileName) + @"\" + newname + @".m4a";

                        if (File.Exists(info))
                        {
                            aext = @".aac";
                        }
                        else if (File.Exists(info2))
                        {
                            aext = @".ac3";

                        }
                        else if (File.Exists(info3))
                        {
                            aext = @".wav";
                        }
                        else if (File.Exists(info4))
                        {
                            aext = @".mp3";
                        }
                        else if (File.Exists(info5))
                        {
                            aext = @".m4a";
                        }

                        if (File.Exists(info) || File.Exists(info2) || File.Exists(info3) || File.Exists(info4) || File.Exists(info5))
                        {


                            String ToDisplay = "0";
                            String ToDisplay1 = "";
                            string capt = "";
                            MediaInfo MI = new MediaInfo();
                            MI.Open(FileName);
                            ToDisplay = MI.Get(StreamKind.Audio, 0, "StreamCount");
                            ToDisplay1 = MI.Get(StreamKind.Audio, 0, "Format");

                            if (ToDisplay == "")
                            {
                                ToDisplay = "0";
                            }


                            // MyMessageBox.Show(ToDisplay1);

                            if (checkmp4.Checked)
                            {
                                //       MyMessageBox.Show(ToDisplay);
                                //       MyMessageBox.Show(capt);
                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.ffmpegPath);
                                mkvprop.StartInfo.Arguments = @" -y -i " + anyCommand + @" -i " + AddDoubleQuotes(newname + aext) + @" -map 0:v:0 -map 0:a -map 0:s?  -map 1 -metadata:s:a:" + ToDisplay + @" language=" + slan + @" -vcodec copy -acodec copy " + @" -disposition:a:" + ToDisplay + @" " + def + @" " + AddDoubleQuotes(dirr + newname + @".mkv");
                                //  + @" -disposition:a:" + ToDisplay + @" " + def + @" "
                            }
                            else
                            {
                                mkvprop.StartInfo.FileName = System.IO.Path.GetFullPath(Main.Instance.Settings.mkvmergePath);
                                mkvprop.StartInfo.Arguments = @" -o " + AddDoubleQuotes(dirr + newname + @".mkv") + @" " + anyCommand + @" --track-name 0:" + langu + @" --language 0:" + slan + @" " + AddDoubleQuotes(newname + aext);
                            }
                            //  mkvprop.StartInfo.Arguments = @" -o " + AddDoubleQuotes(dirr + newname + @".mkv") + @" " + anyCommand + @" --forced-track 0:" + forced + @" --default-track 0:" + def + @" --track-name 0:" + langu + @" --language 0:" + slan + @" " + AddDoubleQuotes(newname + @".srt");


                            string dir = System.IO.Path.GetDirectoryName(FileName);


                            mkvprop.StartInfo.CreateNoWindow = true;
                            mkvprop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            mkvprop.Start();
                            //string output = mkvprop.StandardOutput.ReadToEnd();
                            mkvprop.WaitForExit();

                            if (Overwrite2.Checked == true)
                            {
                                System.IO.File.Copy(Path.Combine(dir + @"\newMKV\", newname + @".mkv"), Path.Combine(dir, newname + @".mkv"), true);
                                Directory.Delete(dir + @"\newMKV\", true);
                                //         File.Delete(dir + newname + aext);


                                //             DirectoryInfo di = new DirectoryInfo(dir);
                                //              FileInfo[] files = di.GetFiles("*.srt")
                                //                                   .Where(p => p.Extension == ".srt").ToArray();
                                //              foreach (FileInfo file in files)
                                //                  try
                                //                  {
                                //                      file.Attributes = FileAttributes.Normal;
                                //                      File.Delete(file.FullName);
                                //                  }
                                //                  catch { }
                            }

                            pBar1.PerformStep();
                            pBar1.Refresh();
                            int percent = (int)(((double)cnt / (double)listBox1.Items.Count) * 100);
                            pBar1.CreateGraphics().DrawString(percent.ToString() + "%",
                                new Font("Arial", (float)10.25, FontStyle.Bold),
                                Brushes.Black,
                                new PointF(pBar1.Width / 2 - 10, pBar1.Height / 2 - 7));

                            cnt++;
                            Cursor.Current = Cursors.Default;


                        }
                        else
                        {
                            au++;

                        }

                    }
                    if (au > 0)
                    {
                    
                    MyMessageBox.Show(au +" files out of " + listBox1.Items.Count + @" don't have audio or Audio files have different name");
                    }

                }
                else
                {
                    MyMessageBox.Show("No Video to Mux audio!");
                }
            }
            else
            {
                MyMessageBox.Show("Add exe paths in Options first");
            }
        }

        public minfo frm;
        public bool form2IsOpen = false; //{ get; set; }

        public Point m_PreviousLocation = new Point(int.MinValue, int.MinValue);

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            if (form2IsOpen == false)
            {
                frm = new minfo(listBox1);

                if (bunifuiOSSwitch1.Value == true)
                {
                    textboxes.Add(frm.richTextBox2);
                    ApplyTheme(zcolor(98, 114, 164), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(40, 42, 54), zcolor(68, 71, 90), zcolor(40, 42, 54), zcolor(68, 71, 90), Color.Black, zcolor(189, 147, 249), System.Drawing.Color.Pink, Color.Pink, SystemColors.Control);
                }
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(m_PreviousLocation.X + 490, m_PreviousLocation.Y + 300);
                frm.Show();
                form2IsOpen = true;
                
            }
            else
            {
                frm.Close();
                form2IsOpen = false;
            }
        }









        // previous MainForm location

        private void Main_LocationChanged_1(object sender, EventArgs e)
        {
            // All open child forms to be moved

            //       minfo kl = new minfo(listBox1);

                 if (form2IsOpen == true)
               {
            // If the main form has been moved...
            if (m_PreviousLocation.X != int.MinValue)
            {
                frm.Location = new Point(
                  frm.Location.X + Location.X - m_PreviousLocation.X,
                  frm.Location.Y + Location.Y - m_PreviousLocation.Y);

            }
            }
            m_PreviousLocation = Location;



            if (sform2IsOpen == true)
            {
                // If the main form has been moved...
                if (m_sPreviousLocation.X != int.MinValue)
                {
                    sfrm.Location = new Point(
                      sfrm.Location.X + Location.X - m_sPreviousLocation.X,
                      sfrm.Location.Y + Location.Y - m_sPreviousLocation.Y);

                }
            }
            m_sPreviousLocation = Location;


        }

        private void sortsub_Click(object sender, EventArgs e)
        {

            if (listBox1.Items.Count > 0)
            {
                string FileName = listBox1.Items[0].ToString();

                string FileExt = Path.GetExtension(FileName);
                string workdir = System.IO.Path.GetDirectoryName(FileName);


                DirectoryInfo di = new DirectoryInfo(workdir);
                int numAUD = di.GetFiles("*.srt", SearchOption.AllDirectories).Length;
                int numFileExt = di.GetFiles(@"*" + FileExt, SearchOption.AllDirectories).Length;

                List<string> videolist = new List<string>();

                FileInfo[] files = di.GetFiles("*" + FileExt, SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string name = file.ToString();
                    name = name.Remove(name.Length - 4);
                    videolist.Add(name);
                }




                int k = 0;
                FileInfo[] infos1 = di.GetFiles("*.srt");
                FileInfo[] infos2 = di.GetFiles("*.idx");
                FileInfo[] infos22 = di.GetFiles("*.sub");
                FileInfo[] infos3 = di.GetFiles("*.vtt");
                FileInfo[] infos4 = di.GetFiles("*.ass");

                int numAUD1 = infos1.Length;
                int numAUD2 = infos2.Length;
                int numAUD22 = infos22.Length;
                int numAUD3 = infos3.Length;
                int numAUD4 = infos4.Length;

                string ext = "";

                if (numAUD1 == numFileExt)
                {
                    ext = ".srt";
                    foreach (FileInfo f in infos1)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;
                }
                if (numAUD2 == numFileExt && numAUD22 == numFileExt)
                {
                    ext = ".idx";
                    foreach (FileInfo f in infos2)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;
                    ext = ".sub";
                    foreach (FileInfo f in infos22)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;


                }
                else if (numAUD2 != numAUD22)
                {
                    MyMessageBox.Show("Sub files are less (or more) than Video files to rename in your dir!");

                }
                if (numAUD3 == numFileExt)
                {
                    ext = ".vtt";
                    foreach (FileInfo f in infos3)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;


                }
                if (numAUD4 == numFileExt)
                {
                    ext = ".ass";
                    foreach (FileInfo f in infos4)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;


                }
                if (numAUD1 != numFileExt && numAUD2 != numFileExt && numAUD3 != numFileExt && numAUD4 != numFileExt && numAUD22 != numFileExt)
                {

                    if (numAUD2 == numAUD22)
                    {
                        if (numAUD1 + numAUD2 == numFileExt)
                        {
                       


                            FileInfo[] final = di.GetFiles("*.srt").Union(di.GetFiles("*.idx")).ToArray();
                            Array.Sort(final, (x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name));
                            FileInfo[] final2 = di.GetFiles("*.srt").Union(di.GetFiles("*.sub")).ToArray();
                            Array.Sort(final2, (x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name));


                            foreach (FileInfo f in final)
                            {
                                ext = f.ToString();
                                ext = ext.Substring(ext.Length - 4);
                                string renameddir = f.DirectoryName;
                                string newName = f.Name.Replace(f.ToString(), videolist[k]);
                                newName = renameddir + @"\" + newName + ext;
                                //          MyMessageBox.Show(newName);

                                File.Move(f.FullName, newName);
                                k++;
                            }

                            k = 0;
                            foreach (FileInfo f in final2)
                            {
                                ext = f.ToString();
                                ext = ext.Substring(ext.Length - 4);
                                if (ext == ".sub")
                                {
                                
                                string renameddir = f.DirectoryName;
                                string newName = f.Name.Replace(f.ToString(), videolist[k]);
                                newName = renameddir + @"\" + newName + ext;
                                //          MyMessageBox.Show(newName);

                                File.Move(f.FullName, newName);
                               }
                                k++;
                            }

                        }
                        else
                        {
                            
                         MyMessageBox.Show("Sub files are less (or more) than Video files to rename in your dir!");

                        }

                    }
                  

                }
                





            }



        }

























        private void sortaud_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                string FileName = listBox1.Items[0].ToString();

                string FileExt = Path.GetExtension(FileName);
                string workdir = System.IO.Path.GetDirectoryName(FileName);


                DirectoryInfo di = new DirectoryInfo(workdir);
                int numAUD = di.GetFiles("*.ac3", SearchOption.AllDirectories).Length;
                int numFileExt = di.GetFiles(@"*" + FileExt, SearchOption.AllDirectories).Length;

                List<string> videolist = new List<string>();

                FileInfo[] files = di.GetFiles("*" + FileExt, SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string name = file.ToString();
                    name = name.Remove(name.Length - 4);
                    videolist.Add(name);
                }


           

                int k = 0;
                    FileInfo[] infos1 = di.GetFiles("*.ac3");
                    FileInfo[] infos2 = di.GetFiles("*.aac");
                    FileInfo[] infos3 = di.GetFiles("*.mp3");
                    FileInfo[] infos4 = di.GetFiles("*.wav");

                int numAUD1 = infos1.Length;
                int numAUD2 = infos2.Length;
                int numAUD3 = infos3.Length;
                int numAUD4 = infos4.Length;

                string ext = "";

                if (numAUD1 == numFileExt)
                {
                    ext = ".ac3";
                    foreach (FileInfo f in infos1)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;
                }
                if (numAUD2 == numFileExt)
                {
                    ext = ".aac";
                    foreach (FileInfo f in infos2)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;


                }
                if (numAUD3 == numFileExt)
                {
                    ext = ".mp3";
                    foreach (FileInfo f in infos3)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;


                }
                if (numAUD4 == numFileExt)
                {
                    ext = ".wav";
                    foreach (FileInfo f in infos4)
                    {
                        string renameddir = f.DirectoryName;
                        string newName = f.Name.Replace(f.ToString(), videolist[k]);
                        newName = renameddir + @"\" + newName + ext;
                        //          MyMessageBox.Show(newName);

                        File.Move(f.FullName, newName);
                        k++;
                    }
                    k = 0;


                }
                if (numAUD1 != numFileExt && numAUD2 != numFileExt && numAUD3 != numFileExt && numAUD4 != numFileExt )
                {
                    if (numAUD1 + numAUD2 == numFileExt)
                    {

                        FileInfo[] final = di.GetFiles("*.aac").Union(di.GetFiles("*.ac3")).ToArray();
                        Array.Sort(final,(x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name));



                        foreach (FileInfo f in final)
                        {
                            ext = f.ToString();
                            ext = ext.Substring(ext.Length - 4);
                            string renameddir = f.DirectoryName;
                            string newName = f.Name.Replace(f.ToString(), videolist[k]);
                            newName = renameddir + @"\" + newName + ext;
                            //          MyMessageBox.Show(newName);

                            File.Move(f.FullName, newName);
                            k++;
                        }
                       


                    }
                    else
                    {
                        MyMessageBox.Show("Audio files are less (or more) than Video files to rename in your dir!");

                    }



                }


              

             


            }

        }
    }






}
