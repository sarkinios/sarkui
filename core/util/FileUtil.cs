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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

//using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace Sarkui.core.util
{
    delegate bool FileExists(string filename);

    class FileUtil
    {
        public static DirectoryInfo CreateTempDirectory()
        {
            while (true)
            {
                string file = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), Path.GetRandomFileName());

                if (!File.Exists(file) && !Directory.Exists(file))
                {
                    Main.Instance.DeleteOnClosing(file);
                    return Directory.CreateDirectory(file);
                }
            }
        }

        public static bool DeleteFile(string strFile, LogItem oLog)
        {
            if (!File.Exists(strFile))
                return true;

            int iCounter = 0;
            string strError = String.Empty;
            while (iCounter++ < 10)
            {
                try
                {
                    File.Delete(strFile);
                    break;
                }
                catch (IOException ex)
                {
                    strError = ex.Message;
                    Sarkui.core.util.Util.Wait(1000);
                }
                catch (Exception ex)
                {
                    strError = ex.Message;
                    break;
                }
            }
            if (oLog != null && !String.IsNullOrEmpty(strError))
                oLog.Error("Error deleting file " + strFile + ": " + strError);
            return (!File.Exists(strFile));
        }


        public static void DeleteDirectoryIfExists(string p, bool recursive)
        {
            try
            {
                if (Directory.Exists(p))
                    Directory.Delete(p, recursive);
            }
            catch { }
        }

        public static DirectoryInfo ensureDirectoryExists(string p)
        {
            if (Directory.Exists(p))
                return new DirectoryInfo(p);
            if (string.IsNullOrEmpty(p))
                throw new IOException("Can't create directory");
            ensureDirectoryExists(GetDirectoryName(p));
            Sarkui.core.util.Util.Wait(100);
            return Directory.CreateDirectory(p);
        }

        public static string GetDirectoryName(string file)
        {
            string path = string.Empty;
            try
            {
                path = Path.GetDirectoryName(file);
            }
            catch { }
            return path;
        }

        /// <summary>
        /// Generates a unique filename by adding numbers to the filename.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="fileExists"></param>
        /// <returns></returns>
        public static string getUniqueFilename(string original, FileExists fileExists)
        {
            if (!fileExists(original)) return original;
            string prefix = Path.Combine(Path.GetDirectoryName(original),
                Path.GetFileNameWithoutExtension(original)) + "_";
            string suffix = Path.GetExtension(original);
            for (int i = 0; true; i++)
            {
                string filename = prefix + i + suffix;
                if (!fileExists(filename)) return filename;
            }
        }

        public static List<string> AllFiles(string folder)
        {
            List<string> list = new List<string>();
            AddFiles(folder, list);
            return list;
        }

        private static void AddFiles(string folder, List<string> list)
        {
            list.AddRange(Directory.GetFiles(folder));
            foreach (string subFolder in Directory.GetDirectories(folder))
                AddFiles(subFolder, list);
        }

        private const int BUFFER_SIZE = 2 * 1024 * 1024; // 2 MBs
        public static void copyData(Stream input, Stream output)
        {
            int count = -1;
            byte[] data = new byte[BUFFER_SIZE];
            while ((count = input.Read(data, 0, BUFFER_SIZE)) > 0)
            {
                output.Write(data, 0, count);
            }
        }

        /// <summary>
        /// Returns the full path and filename, but without the extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathWithoutExtension(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// Returns TimeSpan value formatted
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToShortString(TimeSpan ts)
        {
            string time;
            time = ts.Hours.ToString("00");
            time = time + ":" + ts.Minutes.ToString("00");
            time = time + ":" + ts.Seconds.ToString("00");
            time = time + "." + ts.Milliseconds.ToString("000");
            return time;
        }

        /// <summary>
        /// Adds extra to the filename, modifying the filename but keeping the extension and folder the same.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static string AddToFileName(string filename, string extra)
        {
            return Path.Combine(
                Path.GetDirectoryName(filename),
                Path.GetFileNameWithoutExtension(filename) + extra + Path.GetExtension(filename));
        }

        /// <summary>
        /// Returns true if the filename matches the filter specified. The format
        /// of the filter is the same as that of a FileDialog.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool MatchesFilter(string filter, string filename)
        {
            if (string.IsNullOrEmpty(filter))
                return true;

            bool bIsFolder = Directory.Exists(filename);

            filter = filter.ToLowerInvariant();
            filename = Path.GetFileName(filename).ToLowerInvariant();
            string[] filters = filter.Split('|');

            for (int i = 1; i < filters.Length; i += 2)
            {
                string[] iFilters = filters[i].Split(';');
                foreach (string f in iFilters)
                {
                    if (f.IndexOf('*') > -1)
                    {
                        if (!f.StartsWith("*."))
                            throw new Exception("Invalid filter format");

                        if (f == "*.*" && filename.IndexOf('.') > -1)
                            return true;

                        if (f == "*." && bIsFolder)
                            return true;

                        string extension = f.Substring(1);
                        if (filename.EndsWith(extension))
                            return true;
                    }
                    else if (f == filename)
                        return true;
                    else return false;

                }
            }

            return false;
        }

        /// <summary>
        /// Backup File
        /// </summary>
        /// <param name"sourcePath">Path of the Source file</param>
        /// <param name="overwrite"></param>
        public static void BackupFile(string sourcePath, bool overwrite)
        {
            try
            {
                if (File.Exists(sourcePath))
                {
                    String targetPath;
                    if (sourcePath.Contains(System.Windows.Forms.Application.StartupPath))
                        targetPath = sourcePath.Replace(System.Windows.Forms.Application.StartupPath, System.Windows.Forms.Application.StartupPath + @"\backup");
                    else
                        targetPath = System.Windows.Forms.Application.StartupPath + @"\backup\" + (new FileInfo(sourcePath)).Name;
                    if (File.Exists(targetPath))
                        File.Delete(targetPath);

                    FileUtil.ensureDirectoryExists(Path.GetDirectoryName(targetPath));

                    File.Move(sourcePath, targetPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while moving file: \n" + sourcePath + "\n" + ex.Message, "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Checks if a directory is writable
        /// </summary>
        /// <param name"strPath">path to check</param>
        public static bool IsDirWriteable(string strPath)
        {
            try
            {
                bool bDirectoryCreated = false;

                // does the root directory exists
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                    bDirectoryCreated = true;
                }

                string newFilePath = string.Empty;
                // combine the random file name with the path
                do
                    newFilePath = Path.Combine(strPath, Path.GetRandomFileName());
                while (File.Exists(newFilePath));

                // create & delete the file
                FileStream fs = File.Create(newFilePath);
                fs.Close();
                File.Delete(newFilePath);

                if (bDirectoryCreated)
                    Directory.Delete(strPath);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the proper output path
        /// </summary>
        /// <param name"strInputFileOrFolder">input file or folder name</param>
        /// <returns>either the default output path or a path based on the input file/folder</returns>
        public static string GetOutputFolder(string strInputFileOrFolder)
        {
            string outputPath = Main.Instance.Settings.DefaultOutputDir;

            // checks if the default output dir does exist and is writable
            if (string.IsNullOrEmpty(outputPath) || !Directory.Exists(outputPath) || !IsDirWriteable(outputPath))
            {
                // default output directory does not exist, use the input folder instead
                if (Directory.Exists(strInputFileOrFolder))
                    outputPath = strInputFileOrFolder;
                else
                    outputPath = Path.GetDirectoryName(strInputFileOrFolder);
            }

            return outputPath;
        }

        /// <summary>
        /// Gets the file prefix based on the folder structure
        /// </summary>
        /// <param name"strInputFile">input file name</param>
        /// <returns>a file prefix if a DVD/Blu-ray structure is found or an emtpy string</returns>
      

        /// <summary>
        /// Gets the file prefix based on the folder structure
        /// </summary>
        /// <param name"text">the text to search in</param>
        /// <param name"pattern">RegEx search pattern</param>
        /// <param name"bIgnoreCase">if the search should be not case sensitive</param>
        /// <returns>true if the pattern does match the text</returns>
        public static bool RegExMatch(string text, string pattern, bool bIgnoreCase)
        {
            // https://msdn.microsoft.com/en-us/library/az24scfc(v=vs.110).aspx
            Regex regex = new Regex(pattern);
            if (bIgnoreCase)
                regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(text);
        }

        /// <summary>
        /// Gets the Blu-ray source path (if possible)
        /// </summary>
        /// <param name"strInputFileOrFolder">the source file or folder name</param>
        /// <returns>empty if the source is not a Blu-ray structure, otherwise a path pointing to the PLAYLIST dir</returns>
        
        /// <summary>
        /// Gets the DVD source path (if possible)
        /// </summary>
        /// <param name"strInputFileOrFolder">the source file or folder name</param>
        /// <returns>empty if the source is not a DVD structure, otherwise the full path of an IFO file</returns>
        
        /// <summary>
        /// Attempts to delete all files and directories listed 
        /// in job.FilesToDelete if settings.DeleteIntermediateFiles is checked
        /// </summary>
        /// <param name="job">the job which should just have been completed</param>
        

        /// <summary>
        /// Detects the AviSynth version/date and writes it into the log
        /// </summary>
        /// <param name="oLog">the version information will be added to the log if available</param>
       

        /// <summary>
        /// Detects the file version/date and writes it into the log
        /// </summary>
        /// <param name="strName">the name in the log</param>
        /// <param name="strFile">the file to check</param>
        /// <param name="oLog">the LogItem where the information should be added</param>
       

        /// <summary>
        /// Gets the file version/date
        /// </summary>
        /// <param name="fileName">the file to check</param>
        /// <param name="fileVersion">the file version</param>
        /// <param name="fileDate">the file date</param>
        /// <param name="fileProductName">the file product name</param>
        /// <returns>true if file can be found, false if file cannot be found</returns>
        private static bool GetFileInformation(string fileName, out string fileVersion, out string fileDate, out string fileProductName)
        {
            fileVersion = fileDate = fileProductName = string.Empty;
            if (!File.Exists(fileName))
                return false;

            FileVersionInfo FileProperties = FileVersionInfo.GetVersionInfo(fileName);
            fileVersion = FileProperties.FileVersion;
            if (!String.IsNullOrEmpty(fileVersion))
                fileVersion = fileVersion.Replace(", ", ".");
            fileDate = File.GetLastWriteTimeUtc(fileName).ToString("dd-MM-yyyy");
            fileProductName = FileProperties.ProductName;
            return true;
        }

        #region redist actions
        /// <summary>
        /// Gets the Redist information based on the registry
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// Gets the sub keys of a root key and the redist information in them
        /// </summary>
        /// <param name="strRegKey">root registry key</param>

        /// <summary>
        /// Tries to find the redist information based on a given reg key
        /// </summary>
        /// <param name="oKey">the reg key to check</param>
        /// <param name="year">the year of the redist package</param>
        /// <param name="architecture">the architecture of the redist package</param>
        /// <param name="version">the version of the redist package</param>


        /// <summary>
        /// copies runtime/redist files
        /// </summary>


        /// <summary>
        /// copies runtime/redist files
        /// </summary>


        /// <summary>
        /// removes runtime/redist files
        /// </summary>

        #endregion

        /// <summary>
        /// Enables or disables the portable AviSynth build
        /// </summary>
        /// <param name="bRemove">if true the files will be removed</param>


        /// <summary>
        /// Creates UTF-8 formatted text file containing the track name of a file.
        /// Allows mp4box to use int'l chars in track names, as well as control chars 
        /// such as colons, equals signs, quotation marks, &c.
        /// </summary>
        public static string CreateUTF8TracknameFile(string trackName, string fileName, int trackNumber)
        {
            string tracknameFilePath = Path.GetFullPath(fileName) + "_TRACKNAME" + trackNumber.ToString() + ".srt";
            try
            {
                using (StreamWriter sw = new StreamWriter(tracknameFilePath, false, Encoding.UTF8))
                {
                    sw.Write(trackName);
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return tracknameFilePath;
        }

        private static object _locker = new object();
        public static void WriteToFile(string fileName, string text, bool append)
        {
            try
            {
                lock (_locker)
                {
                    if (append)
                        System.IO.File.AppendAllText(fileName, text);
                    else
                        System.IO.File.WriteAllText(fileName, text);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error writing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Gets the Haali Installation Directory (may not exist)
        /// </summary>
        /// <returns></returns>
       

        /// <summary>
        /// Checks if Haali Media Splitter is installed
        /// </summary>
        /// <returns></returns>
        

        /// <summary>
        /// Installs the Haali Media Splitter
        /// </summary>
        /// <param name="oLog">the LogItem</param>
        /// <returns>true if installation is successful, false if not</returns>
    }
}