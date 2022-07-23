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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using Sarkui.core.util;
using Sarkui.core.gui;
using Sarkui.core.details;


namespace Sarkui
{
    /// <summary>
    /// Summary description for SarkuiSettings.
    /// </summary>
    [LogByMembers]
    public class SarkSettings
    {
        #region variables
        public enum OCGUIMode
        {
            [EnumTitle("Show Basic Settings")]
            Basic,
            [EnumTitle("Show Default Settings")]
            Default,
            [EnumTitle("Show Advanced Settings")]
            Advanced
        };
        public enum StandbySettings
        {
            [EnumTitle("Do not prevent standby")]
            SystemDefault,
            [EnumTitle("Prevent system standby")]
            DisableSystemStandby,
            [EnumTitle("Prevent monitor standby")]
            DisableMonitorStandby
        };
        private string strMainAudioFormat, strMainFileFormat, FfmpegPath, MkvpropPath, MkvmergePath, MkvextPath, Mp4boxPath, SoxPath, version,
                       defaultLanguage1, defaultLanguage2,  videoExtension, audioExtension,
                       strEac3toLastFolderPath, strEac3toLastFilePath, strEac3toLastDestinationPath, tempDirMP4,
                       fdkAacPath, defaultOutputDir,
                       appendToForcedStreams, lastUsedOneClickFolder,  chapterCreatorSortString;
        private bool autoForceFilm, autoOpenScript, bUseQAAC, bUseDGIndexNV, bUseDGIndexIM, bInput8Bit,
                     overwriteStats, keep2of3passOutput, deleteIntermediateFiles, workerAutoStart,
                     deleteAbortedOutput, openProgressWindow, bEac3toAutoSelectStreams, bUseFDKAac, bVobSubberKeepAll,
                     alwaysOnTop, addTimePosition, alwaysbackupfiles, bUseITU, bEac3toLastUsedFileMode, 
                     bAutoLoadDG, bAlwayUsePortableAviSynth, bVobSubberSingleFileExport, workerAutoStartOnStartup,
                     bEnsureCorrectPlaybackSpeed, bExternalMuxerX264, bUseNeroAacEnc, bUseffmpeg, bUseMkvprop, bUseMkvmerge, bUseMkvext, bUseMp4box, bUseSox, bEnableDirectShowSource,
                     bVobSubberExtractForced, bVobSubberShowAll,
                     bEac3toEnableEncoder, bEac3toEnableDecoder, bEac3toEnableCustomOptions, bFirstUpdateCheck,
                     bChapterCreatorCounter, bX264AdvancedSettings, bEac3toAddPrefix, workerRemoveJob;
        private decimal forceFilmThreshold, acceptableFPSError;
        private int nbPasses,  minComplexity, updateFormSplitter,
                    maxComplexity, jobColumnWidth, inputColumnWidth, outputColumnWidth, codecColumnWidth,
                    modeColumnWidth, statusColumnWidth, startColumnWidth, endColumnWidth, fpsColumnWidth,
                    updateFormUpdateColumnWidth, updateFormNameColumnWidth, updateFormLocalVersionColumnWidth,
                    updateFormServerVersionColumnWidth, updateFormLocalDateColumnWidth, updateFormServerDateColumnWidth,
                    updateFormLastUsedColumnWidth, updateFormStatusColumnWidth, updateFormServerArchitectureColumnWidth,
                    ffmsThreads, chapterCreatorMinimumLength;
        private double dpiScaleFactor, dLastDPIScaleFactor;
   //     private SourceDetectorSettings sdSettings;
  //     private AutoEncodeDefaultsSettings aedSettings;
   //     private DialogSettings dialogSettings;
        private Point mainFormLocation, updateFormLocation;
        private Size mainFormSize, updateFormSize;
   //     private FileSize[] customFileSizes;
   //     private FPS[] customFPSs;
   //     private Dar[] customDARs;
        private OCGUIMode ocGUIMode;
        private StandbySettings standbySetting;

        private ProgramSettings  avisynth, avisynthplugins, besplit, dgindexim, dgindex, dgindexnv,
                                eac3to, fdkaac, ffmpeg, ffms, flac, haali, lame, lsmash, mediainfo,
                                 mkvmerge, mp4box, mkvextract,  neroaacenc, mkvpropedit, 
                                oggenc, opus, pgcdemux, qaac, redist, tsmuxer, vsrip, x264, x265, xvid;
        Dictionary<string, string> oRedistVersions;
        #endregion
        public SarkSettings()
        {
            // get the DPI scale factor
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                dpiScaleFactor = dpiX / 96.0;
            }

            // OS / build detection


   

   //             new string[] { "Development", "http://" }, new string[] { "Custom" }};
  //          lastUpdateCheck = DateTime.Now.AddDays(-77).ToUniversalTime();
   //         disablePackageInterval = 14;
   //         updateCheckInterval = 240;
            acceptableFPSError = 0.01M;
   //         autoUpdate = true;
  //          dialogSettings = new DialogSettings();
 //           sdSettings = new SourceDetectorSettings();
  //          AedSettings = new AutoEncodeDefaultsSettings();

            autoForceFilm = true;
            bAutoLoadDG = true;
            forceFilmThreshold = new decimal(95);
            defaultLanguage1 = "English";
            defaultLanguage2 = "English";
            autoOpenScript = true;
            overwriteStats = true;
            keep2of3passOutput = false;
            nbPasses = 2;
            deleteIntermediateFiles = true;
            deleteAbortedOutput = true;
            bEac3toAutoSelectStreams = true;
            strEac3toLastFolderPath = strEac3toLastFilePath = strEac3toLastDestinationPath = "";
            bEac3toLastUsedFileMode = false;
            bEac3toEnableEncoder = bEac3toEnableDecoder = bEac3toEnableCustomOptions = false;
            bEac3toAddPrefix = true;
            openProgressWindow = true;
            videoExtension = "";
            audioExtension = "";
            alwaysOnTop = false;
 /*           httpProxyMode = ProxyMode.None;
            httpproxyaddress = "";
            httpproxyport = "";
            httpproxyuid = "";
            httpproxypwd = "";
  */          defaultOutputDir = "";
            tempDirMP4 = "";
            addTimePosition = true;
            alwaysbackupfiles = false;
            strMainFileFormat = "";
            strMainAudioFormat = "";
            minComplexity = 72;
            maxComplexity = 78;
            mainFormLocation = new Point(0, 0);
            mainFormSize = new Size(713, 478);
 /*           updateFormLocation = new Point(0, 0);
            updateFormSize = new Size(780, 313);
            updateFormSplitter = 180;
            updateFormUpdateColumnWidth = 47;
            updateFormNameColumnWidth = 105;
            updateFormLocalVersionColumnWidth = 117;
            updateFormServerVersionColumnWidth = 117;
            updateFormServerArchitectureColumnWidth = 50;
            updateFormLocalDateColumnWidth = 70;
            updateFormServerDateColumnWidth = 70;
            updateFormLastUsedColumnWidth = 70;
            updateFormStatusColumnWidth = 111;
   */         jobColumnWidth = 40;
            inputColumnWidth = 105;
            outputColumnWidth = 105;
            codecColumnWidth = 79;
            modeColumnWidth = 79;
            statusColumnWidth = 65;
            startColumnWidth = 58;
            endColumnWidth = 58;
            fpsColumnWidth = 95;
            bEnsureCorrectPlaybackSpeed = false;
            bAlwayUsePortableAviSynth = true;
            ffmsThreads = 1;
            appendToForcedStreams = "";
            ocGUIMode = OCGUIMode.Default;
            bUseITU = true;
            lastUsedOneClickFolder = "";
            bUseNeroAacEnc = bUseFDKAac = bUseQAAC = bUseDGIndexNV = bUseDGIndexIM = false;
            bUseffmpeg = bUseMkvmerge = bUseMkvprop = bUseMp4box = bUseMkvext = bUseSox = false;
            chapterCreatorMinimumLength = 900;
            bExternalMuxerX264 = true;
            bVobSubberSingleFileExport = false;
            bVobSubberKeepAll = false;
            bVobSubberExtractForced = false;
            bVobSubberShowAll = false;
            chapterCreatorSortString = "duration";
            bEnableDirectShowSource = false;
 //           bFirstUpdateCheck = true;
            dLastDPIScaleFactor = 0;
            oRedistVersions = new Dictionary<string, string>();
            bChapterCreatorCounter = true;
            bX264AdvancedSettings = false;
            workerAutoStart = true;
            workerAutoStartOnStartup = false;
            workerRemoveJob = true;
            bInput8Bit = true;
   //         ResetWorkerSettings();
   //         ResetWorkerPriority();
            version = "";
            standbySetting = StandbySettings.DisableSystemStandby;
        }

        #region properties

      


        [XmlIgnore]
        public double DPIScaleFactor
        {
            get { return dpiScaleFactor; }
        }

        public double LastDPIScaleFactor
        {
            get { return dpiScaleFactor; }
            set { dLastDPIScaleFactor = value; }
        }

        public Point MainFormLocation
        {
            get { return mainFormLocation; }
            set { mainFormLocation = value; }
        }

        public Size MainFormSize
        {
            get { return mainFormSize; }
            set { mainFormSize = value; }
        }

        public Point UpdateFormLocation
        {
            get { return updateFormLocation; }
            set { updateFormLocation = value; }
        }

        public Size UpdateFormSize
        {
            get { return updateFormSize; }
            set { updateFormSize = value; }
        }

        public int UpdateFormSplitter
        {
            get { return updateFormSplitter; }
            set { updateFormSplitter = value; }
        }

        public int UpdateFormUpdateColumnWidth
        {
            get { return updateFormUpdateColumnWidth; }
            set { updateFormUpdateColumnWidth = value; }
        }

        public int UpdateFormNameColumnWidth
        {
            get { return updateFormNameColumnWidth; }
            set { updateFormNameColumnWidth = value; }
        }

        public int UpdateFormLocalVersionColumnWidth
        {
            get { return updateFormLocalVersionColumnWidth; }
            set { updateFormLocalVersionColumnWidth = value; }
        }

        public int UpdateFormServerVersionColumnWidth
        {
            get { return updateFormServerVersionColumnWidth; }
            set { updateFormServerVersionColumnWidth = value; }
        }

        public int UpdateFormServerArchitectureColumnWidth
        {
            get { return updateFormServerArchitectureColumnWidth; }
            set { updateFormServerArchitectureColumnWidth = value; }
        }

        public int UpdateFormLocalDateColumnWidth
        {
            get { return updateFormLocalDateColumnWidth; }
            set { updateFormLocalDateColumnWidth = value; }
        }

        public int UpdateFormServerDateColumnWidth
        {
            get { return updateFormServerDateColumnWidth; }
            set { updateFormServerDateColumnWidth = value; }
        }

        public int UpdateFormLastUsedColumnWidth
        {
            get { return updateFormLastUsedColumnWidth; }
            set { updateFormLastUsedColumnWidth = value; }
        }

        public int UpdateFormStatusColumnWidth
        {
            get { return updateFormStatusColumnWidth; }
            set { updateFormStatusColumnWidth = value; }
        }

        public int JobColumnWidth
        {
            get { return jobColumnWidth; }
            set { jobColumnWidth = value; }
        }

        public int InputColumnWidth
        {
            get { return inputColumnWidth; }
            set { inputColumnWidth = value; }
        }

        public int OutputColumnWidth
        {
            get { return outputColumnWidth; }
            set { outputColumnWidth = value; }
        }

        public int CodecColumnWidth
        {
            get { return codecColumnWidth; }
            set { codecColumnWidth = value; }
        }

        public int ModeColumnWidth
        {
            get { return modeColumnWidth; }
            set { modeColumnWidth = value; }
        }

        public int StatusColumnWidth
        {
            get { return statusColumnWidth; }
            set { statusColumnWidth = value; }
        }

        public int StartColumnWidth
        {
            get { return startColumnWidth; }
            set { startColumnWidth = value; }
        }

        public int EndColumnWidth
        {
            get { return endColumnWidth; }
            set { endColumnWidth = value; }
        }

        public int FPSColumnWidth
        {
            get { return fpsColumnWidth; }
            set { fpsColumnWidth = value; }
        }

 /*       public FileSize[] CustomFileSizes
        {
            get { return customFileSizes; }
            set { customFileSizes = value; }
        }

        public FPS[] CustomFPSs
        {
            get { return customFPSs; }
            set { customFPSs = value; }
        }

        public Dar[] CustomDARs
        {
            get { return customDARs; }
            set { customDARs = value; }
        }
 */
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string Eac3toLastFolderPath
        {
            get { return strEac3toLastFolderPath; }
            set { strEac3toLastFolderPath = value; }
        }

        public string Eac3toLastFilePath
        {
            get { return strEac3toLastFilePath; }
            set { strEac3toLastFilePath = value; }
        }

        public string Eac3toLastDestinationPath
        {
            get { return strEac3toLastDestinationPath; }
            set { strEac3toLastDestinationPath = value; }
        }

        public bool Eac3toLastUsedFileMode
        {
            get { return bEac3toLastUsedFileMode; }
            set { bEac3toLastUsedFileMode = value; }
        }

 

        public bool Eac3toEnableEncoder
        {
            get { return bEac3toEnableEncoder; }
            set { bEac3toEnableEncoder = value; }
        }

        public bool Eac3toEnableDecoder
        {
            get { return bEac3toEnableDecoder; }
            set { bEac3toEnableDecoder = value; }
        }

        /// <summary>
        /// true if HD Streams Extractor should automatically select tracks
        /// </summary>
        public bool Eac3toAutoSelectStreams
        {
            get { return bEac3toAutoSelectStreams; }
            set { bEac3toAutoSelectStreams = value; }
        }

        public bool Eac3toAddPrefix
        {
            get { return bEac3toAddPrefix; }
            set { bEac3toAddPrefix = value; }
        }

        public bool Eac3toEnableCustomOptions
        {
            get { return bEac3toEnableCustomOptions; }
            set { bEac3toEnableCustomOptions = value; }
        }

        public bool ChapterCreatorCounter
        {
            get { return bChapterCreatorCounter; }
            set { bChapterCreatorCounter = value; }
        }

        public bool X264AdvancedSettings
        {
            get { return bX264AdvancedSettings; }
            set { bX264AdvancedSettings = value; }
        }

        public bool EnableDirectShowSource
        {
            get { return bEnableDirectShowSource; }
            set { bEnableDirectShowSource = value; }
        }

        /// <summary>
        /// True if no update check has been started yet
        /// </summary>
        public bool FirstUpdateCheck
        {
            get { return bFirstUpdateCheck; }
            set { bFirstUpdateCheck = value; }
        }

        /// <summary>
        /// Gets / sets whether the one-click advanced settings will be shown
        /// </summary>
        public OCGUIMode OneClickGUIMode
        {
            get { return ocGUIMode; }
            set { ocGUIMode = value; }
        }

        /// <summary>
        /// Gets / sets the standby settings
        /// </summary>
        public StandbySettings StandbySetting
        {
            get { return standbySetting; }
            set { standbySetting = value; }
        }

        /// <summary>
        /// Gets / sets whether the playback speed in video preview should match the fps
        /// </summary>
        public bool EnsureCorrectPlaybackSpeed
        {
            get { return bEnsureCorrectPlaybackSpeed; }
            set { bEnsureCorrectPlaybackSpeed = value; }
        }

        /// <summary>
        /// Maximum error that the bitrate calculator should accept when rounding the framerate
        /// </summary>
        public decimal AcceptableFPSError
        {
            get { return acceptableFPSError; }
            set { acceptableFPSError = value; }
        }

       

        

       

       

        public bool AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set { alwaysOnTop = value; }
        }

       
        public bool AddTimePosition
        {
            get { return addTimePosition; }
            set { addTimePosition = value; }
        }

        /// <summary>
        /// bool to decide whether to use external muxer for the x264 encoder
        /// </summary>
        public bool UseExternalMuxerX264
        {
            get { return bExternalMuxerX264; }
            set { bExternalMuxerX264 = value; }
        }

        /// <summary>
        /// gets / sets the default output directory
        /// </summary>
        public string DefaultOutputDir
        {
            get { return defaultOutputDir; }
            set { defaultOutputDir = value; }
        }

        /// <summary>
        /// gets / sets the temp directory for MP4 Muxer
        /// </summary>
        public string TempDirMP4
        {
            get
            {
                if (String.IsNullOrEmpty(tempDirMP4) || Path.GetPathRoot(tempDirMP4).Equals(tempDirMP4, StringComparison.CurrentCultureIgnoreCase))
                    return String.Empty;
                return tempDirMP4;
            }
            set { tempDirMP4 = value; }
        }

        ///<summary>
        /// gets / sets whether sarkui backup files from updater or not
        /// </summary>
        public bool AlwaysBackUpFiles
        {
            get { return alwaysbackupfiles; }
            set { alwaysbackupfiles = value; }
        }

        /// <summary>
        /// folder containing the avisynth plugins
 /*       /// </summary>
        public string AvisynthPluginsPath
        {
            get
            {
                UpdateCacher.CheckPackage("avisynth_plugin");
                return Path.GetDirectoryName(avisynthplugins.Path);
            }
        }
*/
        /// <summary>
        /// folder containing local copies of update files
        /// </summary>
      

        /// <summary>
        /// should force film automatically be applies if the film percentage crosses the forceFilmTreshold?
        /// </summary>
        public bool AutoForceFilm
        {
            get { return autoForceFilm; }
            set { autoForceFilm = value; }
        }

        /// <summary>
        /// should the file autoloaded incrementally if VOB
        /// </summary>
        public bool AutoLoadDG
        {
            get { return bAutoLoadDG; }
            set { bAutoLoadDG = value; }
        }

        /// <summary>
        /// gets / sets whether sarkui automatically opens the preview window upon loading an avisynth script
        /// </summary>
        public bool AutoOpenScript
        {
            get { return autoOpenScript; }
            set { autoOpenScript = value; }
        }

        /// <summary>
        /// gets / sets whether the progress window should be opened for each job
        /// </summary>
        public bool OpenProgressWindow
        {
            get { return openProgressWindow; }
            set { openProgressWindow = value; }
        }

        /// <summary>
        /// the threshold to apply force film. If the film percentage is higher than this threshold,
        /// force film will be applied
        /// </summary>
        public decimal ForceFilmThreshold
        {
            get { return forceFilmThreshold; }
            set { forceFilmThreshold = value; }
        }

        /// <summary>
        /// <summary>
        /// first default language
        /// </summary>
        public string DefaultLanguage1
        {
            get { return defaultLanguage1; }
            set { defaultLanguage1 = value; }
        }

        /// <summary>
        /// second default language
        /// </summary>
        public string DefaultLanguage2
        {
            get { return defaultLanguage2; }
            set { defaultLanguage2 = value; }
        }

        /// <summary>
        /// sets / gets if the stats file is updated in the 3rd pass of 3 pass encoding
        /// </summary>
        public bool OverwriteStats
        {
            get { return overwriteStats; }
            set { overwriteStats = value; }
        }

        /// <summary>
        ///  gets / sets if the output is video output of the 2nd pass is overwritten in the 3rd pass of 3 pass encoding
        /// </summary>
        public bool Keep2of3passOutput
        {
            get { return keep2of3passOutput; }
            set { keep2of3passOutput = value; }
        }

        /// <summary>
        /// sets the number of passes to be done in automated encoding mode
        /// </summary>
        public int NbPasses
        {
            get { return nbPasses; }
            set { nbPasses = value; }
        }

        /// <summary>
        /// sets / gets if intermediate files are to be deleted after encoding
        /// </summary>
        public bool DeleteIntermediateFiles
        {
            get { return deleteIntermediateFiles; }
            set { deleteIntermediateFiles = value; }
        }

        /// <summary>
        /// gets / sets if the output of an aborted job is to be deleted
        /// </summary>
        public bool DeleteAbortedOutput
        {
            get { return deleteAbortedOutput; }
            set { deleteAbortedOutput = value; }
        }

        public string VideoExtension
        {
            get { return videoExtension; }
            set { videoExtension = value; }
        }

        public string AudioExtension
        {
            get { return audioExtension; }
            set { audioExtension = value; }
        }

     

    
        /// <summary>
        /// gets / sets the text to append to forced streams
        /// </summary>
        public string AppendToForcedStreams
        {
            get { return appendToForcedStreams; }
            set { appendToForcedStreams = value; }
        }

        public string MainAudioFormat
        {
            get { return strMainAudioFormat; }
            set { strMainAudioFormat = value; }
        }

        public string MainFileFormat
        {
            get { return strMainFileFormat; }
            set { strMainFileFormat = value; }
        }

        public string LastUsedOneClickFolder
        {
            get { return lastUsedOneClickFolder; }
            set { lastUsedOneClickFolder = value; }
        }

        public int MinComplexity
        {
            get { return minComplexity; }
            set { minComplexity = value; }
        }

        public int MaxComplexity
        {
            get { return maxComplexity; }
            set { maxComplexity = value; }
        }

        public int FFMSThreads
        {
            get { return ffmsThreads; }
            set { ffmsThreads = value; }
        }

        public bool UseITUValues
        {
            get { return bUseITU; }
            set { bUseITU = value; }
        }

        public int ChapterCreatorMinimumLength
        {
            get { return chapterCreatorMinimumLength; }
            set { chapterCreatorMinimumLength = value; }
        }

        public string ChapterCreatorSortString
        {
            get { return chapterCreatorSortString; }
            set { chapterCreatorSortString = value; }
        }

        public bool VobSubberExtractForced
        {
            get { return bVobSubberExtractForced; }
            set { bVobSubberExtractForced = value; }
        }

        public bool VobSubberSingleFileExport
        {
            get { return bVobSubberSingleFileExport; }
            set { bVobSubberSingleFileExport = value; }
        }

        public bool VobSubberKeepAll
        {
            get { return bVobSubberKeepAll; }
            set { bVobSubberKeepAll = value; }
        }

        public bool VobSubberShowAll
        {
            get { return bVobSubberShowAll; }
            set { bVobSubberShowAll = value; }
        }


        public bool Input8Bit
        {
            get { return bInput8Bit; }
            set { bInput8Bit = value; }
        }

        public bool WorkerAutoStart
        {
            get { return workerAutoStart; }
            set { workerAutoStart = value; }
        }

        public bool WorkerAutoStartOnStartup
        {
            get { return workerAutoStartOnStartup; }
            set { workerAutoStartOnStartup = value; }
        }

        public bool WorkerRemoveJob
        {
            get { return workerRemoveJob; }
            set { workerRemoveJob = value; }
        }

        /// <summary>
        /// always use portable avisynth
        /// </summary>
        public bool AlwaysUsePortableAviSynth
        {
            get { return bAlwayUsePortableAviSynth; }
            set { bAlwayUsePortableAviSynth = value; }
        }

        /// <summary>
        /// filename and full path of the ffmpeg executable
        /// </summary>
        public string ffmpegPath
        {
            get
            {
                if (!File.Exists(FfmpegPath))
                    ffmpegPath = " ";// Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffmpeg\ffmpeg.exe"); // @"tools\eac3to\neroAacEnc.exe");
                return FfmpegPath;
            }
            set
            {
                if (!File.Exists(value))
                    FfmpegPath = " ";// Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\ffmpeg\ffmpeg.exe"); // @"tools\eac3to\neroAacEnc.exe");
                else
                    FfmpegPath = value;
            }
        }


        /// <summary>
        /// filename and full path of the mkvprop executable
        /// </summary>
        public string mkvpropPath
        {
            get
            {
                if (!File.Exists(MkvpropPath))
                    MkvpropPath = " ";// Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\mkvpropedit.exe"); // @"tools\eac3to\neroAacEnc.exe");
               return MkvpropPath;
               
            }
            set
            {
                if (!File.Exists(value))
                    MkvpropPath = " ";// Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\mkvpropedit.exe"); // @"tools\eac3to\neroAacEnc.exe");
                else
                    MkvpropPath = value;
            }
        }

        /// <summary>
        /// filename and full path of the mkvmerge executable
        /// </summary>
        /// 
        public string mkvmergePath
        {
            get
            {
                if (!File.Exists(MkvmergePath))
                    MkvmergePath = " "; // Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\mkvmerge.exe");
                return MkvmergePath;
            }
            set
            {
                if (!File.Exists(value))
                    MkvmergePath = " "; // Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\mkvmerge.exe");
                else
                    MkvmergePath = value;
            }
        }




        public string mkvextPath
        {
            get
            {
                if (!File.Exists(MkvextPath))
                    MkvextPath = " "; // Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\mkvmerge.exe");
                return MkvextPath;
            }
            set
            {
                if (!File.Exists(value))
                    MkvextPath = " "; // Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\mkvmerge.exe");
                else
                    MkvextPath = value;
            }
        }
        public string mp4boxPath
        {
            get
            {
                if (!File.Exists(Mp4boxPath))
                    Mp4boxPath = " "; //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\atomicparsley.exe");
                return Mp4boxPath;
            }
            set
            {
                if (!File.Exists(value))
                    Mp4boxPath = " "; //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\atomicparsley.exe");
                else
                    Mp4boxPath = value;
            }
        }
        public string soxPath
        {
            get
            {
                if (!File.Exists(SoxPath))
                    SoxPath = " "; //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\atomicparsley.exe");
                return SoxPath;
            }
            set
            {
                if (!File.Exists(value))
                    SoxPath = " "; //Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"sarktools\atomicparsley.exe");
                else
                    SoxPath = value;
            }
        }
        /// 
        /// <summary>
        /// filename and full path of the fdkaac executable
        /// </summary>
        public string FDKAacPath
        {
            get
            {
                if (!File.Exists(fdkAacPath))
                    fdkAacPath = ""; // Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\fdkaac\fdkaac.exe");
                return fdkAacPath;
            }
            set
            {
                if (!File.Exists(value))
                    fdkAacPath = ""; // Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"tools\fdkaac\fdkaac.exe");
                else
                    fdkAacPath = value;
            }
        }

        public bool UseDGIndexNV
        {
            get { return bUseDGIndexNV; }
            set { bUseDGIndexNV = value; }
        }

        public bool UseDGIndexIM
        {
            get { return bUseDGIndexIM; }
            set { bUseDGIndexIM = value; }
        }

        public bool UseFfmpeg
        {
            get { return bUseffmpeg; }
            set { bUseffmpeg = value; }
        }

        public bool UseMkvpropedit
        {
            get { return bUseMkvprop;  }
            set { bUseMkvprop = value; }
        }

        public bool UseMkvmerge
        {
            get { return bUseMkvmerge; }
            set { bUseMkvmerge = value; }
        }
        public bool UseMkvext
        {
            get { return bUseMkvext; }
            set { bUseMkvext = value; }
        }
        public bool UseMp4box
        {
            get { return bUseMp4box; }
            set { bUseMp4box = value; }
        }
        public bool UseSox
        {
            get { return bUseSox; }
            set { bUseSox = value; }
        }

        public bool UseFDKAac
        {
            get { return bUseFDKAac; }
            set { bUseFDKAac = value; }
        }

        public bool UseQAAC
        {
            get { return bUseQAAC; }
            set { bUseQAAC = value; }
        }

        [XmlIgnore]
        public Dictionary<string, string> RedistVersions
        {
            get { return oRedistVersions; }
            set { oRedistVersions = value; }
        }

        /*     public ProgramSettings AviMuxGui
             {
                 get { return avimuxgui; }
                 set { avimuxgui = value; }
             }

             public ProgramSettings AviSynth
             {
                 get { return avisynth; }
                 set { avisynth = value; }
             }

             public ProgramSettings AviSynthPlugins
             {
                 get { return avisynthplugins; }
                 set { avisynthplugins = value; }
             }

             public ProgramSettings BeSplit
             {
                 get { return besplit; }
                 set { besplit = value; }
             }

             public ProgramSettings DGIndexIM
             {
                 get { return dgindexim; }
                 set { dgindexim = value; }
             }

             public ProgramSettings DGIndex
             {
                 get { return dgindex; }
                 set { dgindex = value; }
             }

             public ProgramSettings DGIndexNV
             {
                 get { return dgindexnv; }
                 set { dgindexnv = value; }
             }

             public ProgramSettings Eac3to
             {
                 get { return eac3to; }
                 set { eac3to = value; }
             }

             public ProgramSettings Fdkaac
             {
                 get { return fdkaac; }
                 set { fdkaac = value; }
             }
     */
        /// <summary>
        /// program settings of ffmpeg
        /// </summary>
        /// 

/*
        public ProgramSettings FFmpeg
        {
            get { return ffmpeg; }
            set { ffmpeg = value; }
        }

        public ProgramSettings FFMS
        {
            get { return ffms; }
            set { ffms = value; }
        }

        public ProgramSettings Flac
        {
            get { return flac; }
            set { flac = value; }
        }

        public ProgramSettings Haali
        {
            get { return haali; }
            set { haali = value; }
        }

        public ProgramSettings Lame
        {
            get { return lame; }
            set { lame = value; }
        }

        public ProgramSettings LSMASH
        {
            get { return lsmash; }
            set { lsmash = value; }
        }

        public ProgramSettings MediaInfo
        {
            get { return mediainfo; }
            set { mediainfo = value; }
        }


        /// <summary>
        /// program settings of mkvmerge
        /// </summary>
        public ProgramSettings MkvMerge
        {
            get { return mkvmerge; }
            set { mkvmerge = value; }
        }

        public ProgramSettings Mp4Box
        {
            get { return mp4box; }
            set { mp4box = value; }
        }

        public ProgramSettings NeroAacEnc
        {
            get { return neroaacenc; }
            set { neroaacenc = value; }
        }

        public ProgramSettings OggEnc
        {
            get { return oggenc; }
            set { oggenc = value; }
        }

        public ProgramSettings Opus
        {
            get { return opus; }
            set { opus = value; }
        }

        public ProgramSettings PgcDemux
        {
            get { return pgcdemux; }
            set { pgcdemux = value; }
        }

        public ProgramSettings QAAC
        {
            get { return qaac; }
            set { qaac = value; }
        }

        public ProgramSettings Redist
        {
            get { return redist; }
            set { redist = value; }
        }

        public ProgramSettings TSMuxer
        {
            get { return tsmuxer; }
            set { tsmuxer = value; }
        }

        public ProgramSettings VobSubRipper
        {
            get { return vsrip; }
            set { vsrip = value; }
        }

        /// <summary>
        /// program settings of x264 8bit
        /// </summary>
        public ProgramSettings X264
        {
            get { return x264; }
            set { x264 = value; }
        }

        public ProgramSettings X265
        {
            get { return x265; }
            set { x265 = value; }
        }

        public ProgramSettings XviD
        {
            get { return xvid; }
            set { xvid = value; }
        }
   */   
   #endregion
    /*
        private bool bPortableAviSynth;
        /// <summary>
        /// portable avisynth in use
        /// </summary>
        [XmlIgnore()]
        public bool PortableAviSynth
        {
            get { return bPortableAviSynth; }
            set { bPortableAviSynth = value; }
        }

        private bool bAviSynthPlus;
        /// <summary>
        /// avisynth+ in use
        /// </summary>
        [XmlIgnore()]
        public bool AviSynthPlus
        {
            get { return bAviSynthPlus; }
            set { bAviSynthPlus = value; }
        }
**/
        #region Methods
        private void VerifySettings()
        {
   /*         bool bReset = false;
            foreach (WorkerSettings oSettings in arrWorkerSettings)
            {
                if (oSettings.MaxCount < 1)
                    bReset = true;
                if (oSettings.JobTypes.Count < 1)
                    bReset = true;
            }
            if (bReset)
                ResetWorkerSettings();

            bReset = false;
            Dictionary<JobType, WorkerPriority> arrDict = new Dictionary<JobType, WorkerPriority>();
            foreach (WorkerPriority oPriority in arrWorkerPriority)
            {
                if (arrDict.ContainsKey(oPriority.JobType))
                {
                    bReset = true;
                    break;
                }
                arrDict.Add(oPriority.JobType, oPriority);
            }
            if (bReset || arrDict.Count != Enum.GetNames(typeof(JobType)).Length)
                ResetWorkerPriority();
    */    }

   /*     public void ResetWorkerSettings()
        {
            arrWorkerSettings = new List<WorkerSettings>();
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.Audio }));
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.Audio, JobType.Demuxer, JobType.Indexer, JobType.Muxer }));
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.OneClick }));
            arrWorkerSettings.Add(new WorkerSettings(1, new List<JobType>() { JobType.Video }));
            iWorkerMaximumCount = 2;
        }

        public void ResetWorkerPriority()
        {
            arrWorkerPriority = new List<WorkerPriority>();
            arrWorkerPriority.Add(new WorkerPriority(JobType.Audio, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Demuxer, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Indexer, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Muxer, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.OneClick, WorkerPriorityType.BELOW_NORMAL, false));
            arrWorkerPriority.Add(new WorkerPriority(JobType.Video, WorkerPriorityType.IDLE, true));
        }
*/
        public int DPIRescale(int iOriginalValue)
        {
            return (int)Math.Round(iOriginalValue * dpiScaleFactor, 0);
        }

        public int DPIReverse(int iOriginalValue)
        {
            return (int)Math.Round(iOriginalValue / dLastDPIScaleFactor * dpiScaleFactor, 0);
        }

        public void DPIRescaleAll()
        {
            if (dpiScaleFactor == dLastDPIScaleFactor)
                return;

            if (dLastDPIScaleFactor == 0)
            {
                mainFormLocation = new Point(0, 0);
                mainFormSize = new Size(DPIRescale(713), DPIRescale(478));
                updateFormLocation = new Point(0, 0);
                updateFormSize = new Size(DPIRescale(780), DPIRescale(313));
                updateFormSplitter = DPIRescale(180);
                updateFormUpdateColumnWidth = DPIRescale(47);
                updateFormNameColumnWidth = DPIRescale(105);
                updateFormLocalVersionColumnWidth = DPIRescale(117);
                updateFormServerVersionColumnWidth = DPIRescale(117);
                updateFormServerArchitectureColumnWidth = DPIRescale(50);
                updateFormLocalDateColumnWidth = DPIRescale(70);
                updateFormServerDateColumnWidth = DPIRescale(70);
                updateFormLastUsedColumnWidth = DPIRescale(70);
                updateFormStatusColumnWidth = DPIRescale(111);
                jobColumnWidth = DPIRescale(40);
                inputColumnWidth = DPIRescale(105);
                outputColumnWidth = DPIRescale(105);
                codecColumnWidth = DPIRescale(79);
                modeColumnWidth = DPIRescale(79);
                statusColumnWidth = DPIRescale(65);
                startColumnWidth = DPIRescale(58);
                endColumnWidth = DPIRescale(58);
                fpsColumnWidth = DPIRescale(95);
            }
            else
            {
                mainFormLocation = new Point(0, 0);
                mainFormSize = new Size(DPIReverse(mainFormSize.Width), DPIReverse(mainFormSize.Height));
                updateFormLocation = new Point(0, 0);
                updateFormSize = new Size(DPIReverse(updateFormSize.Width), DPIReverse(updateFormSize.Height));
                updateFormSplitter = DPIReverse(updateFormSplitter);
                updateFormUpdateColumnWidth = DPIReverse(updateFormUpdateColumnWidth);
                updateFormNameColumnWidth = DPIReverse(updateFormNameColumnWidth);
                updateFormLocalVersionColumnWidth = DPIReverse(updateFormLocalVersionColumnWidth);
                updateFormServerVersionColumnWidth = DPIReverse(updateFormServerVersionColumnWidth);
                updateFormServerArchitectureColumnWidth = DPIReverse(updateFormServerArchitectureColumnWidth);
                updateFormLocalDateColumnWidth = DPIReverse(updateFormLocalDateColumnWidth);
                updateFormServerDateColumnWidth = DPIReverse(updateFormServerDateColumnWidth);
                updateFormLastUsedColumnWidth = DPIReverse(updateFormLastUsedColumnWidth);
                updateFormStatusColumnWidth = DPIReverse(updateFormStatusColumnWidth);
                jobColumnWidth = DPIReverse(jobColumnWidth);
                inputColumnWidth = DPIReverse(inputColumnWidth);
                outputColumnWidth = DPIReverse(outputColumnWidth);
                codecColumnWidth = DPIReverse(codecColumnWidth);
                modeColumnWidth = DPIReverse(modeColumnWidth);
                statusColumnWidth = DPIReverse(statusColumnWidth);
                startColumnWidth = DPIReverse(startColumnWidth);
                endColumnWidth = DPIReverse(endColumnWidth);
                endColumnWidth = DPIReverse(endColumnWidth);
            }
        }

        public bool IsDGIIndexerAvailable()
        {
            if (!bUseDGIndexNV)
                return false;

            // check if the license file is available
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt")))
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(dgindexim.Path), "license.txt")))
                {
                    // license.txt available in the other indexer directory. copy it
                    Directory.CreateDirectory(Path.GetDirectoryName(dgindexnv.Path));
                    File.Copy(Path.Combine(Path.GetDirectoryName(dgindexim.Path), "license.txt"), Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt"));
                }
                else
                    return false;
            }

            // DGI is not available in a RDP connection
            if (SystemInformation.TerminalServerSession == true)
                return false;

            return true;
        }

        public bool IsDGMIndexerAvailable()
        {
            if (!bUseDGIndexIM)
                return false;

            // check if the license file is available
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(dgindexim.Path), "license.txt")))
            {
                if (File.Exists(Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt")))
                {
                    // license.txt available in the other indexer directory. copy it
                    Directory.CreateDirectory(Path.GetDirectoryName(dgindexim.Path));
                    File.Copy(Path.Combine(Path.GetDirectoryName(dgindexnv.Path), "license.txt"), Path.Combine(Path.GetDirectoryName(dgindexim.Path), "license.txt"));
                }
                else
                    return false;
            }

            return true;
        }

        public void InitializeProgramSettings()
        {
            // initialize program settings if required
   //         if (avimuxgui == null)
  //              avimuxgui = new ProgramSettings("avimux_gui");
            if (avisynth == null)
                avisynth = new ProgramSettings("avs");
            if (avisynthplugins == null)
                avisynthplugins = new ProgramSettings("avisynth_plugin");
            if (besplit == null)
                besplit = new ProgramSettings("besplit");
            if (dgindexim == null)
                dgindexim = new ProgramSettings("dgindexim");
            if (dgindex == null)
                dgindex = new ProgramSettings("dgindex");
            if (dgindexnv == null)
                dgindexnv = new ProgramSettings("dgindexnv");
            if (eac3to == null)
                eac3to = new ProgramSettings("eac3to");
            if (fdkaac == null)
                fdkaac = new ProgramSettings("fdkaac");
            if (ffmpeg == null)
                ffmpeg = new ProgramSettings("ffmpeg");
            if (ffms == null)
                ffms = new ProgramSettings("ffms");
            if (flac == null)
                flac = new ProgramSettings("flac");
            if (haali == null)
                haali = new ProgramSettings("haali");
            if (lame == null)
                lame = new ProgramSettings("lame");
            if (lsmash == null)
                lsmash = new ProgramSettings("lsmash");
            if (mediainfo == null)
                mediainfo = new ProgramSettings("mediainfo");
            if (mkvmerge == null)
                mkvmerge = new ProgramSettings("mkvmerge");
            if (mkvextract == null)
                mkvextract = new ProgramSettings("mkvextract");
            if (mp4box == null)
                mp4box = new ProgramSettings("mp4box");
            //if (sox == null)
            //    soxPath = new ProgramSettings("sox");
            if (neroaacenc == null)
                neroaacenc = new ProgramSettings("neroaacenc");
            if (mkvpropedit == null)
                mkvpropedit = new ProgramSettings("mkvpropedit");
            if (oggenc == null)
                oggenc = new ProgramSettings("oggenc2");
            if (opus == null)
                opus = new ProgramSettings("opus");
            if (pgcdemux == null)
                pgcdemux = new ProgramSettings("pgcdemux");
            if (qaac == null)
                qaac = new ProgramSettings("qaac");
            if (redist == null)
                redist = new ProgramSettings("redist");
            if (tsmuxer == null)
                tsmuxer = new ProgramSettings("tsmuxer");
            if (vsrip == null)
                vsrip = new ProgramSettings("vsrip");
            if (x264 == null)
                x264 = new ProgramSettings("x264");
            if (x265 == null)
                x265 = new ProgramSettings("x265");
            if (xvid == null)
                xvid = new ProgramSettings("xvid_encraw");

            VerifySettings();
        }
        #endregion
    }


}