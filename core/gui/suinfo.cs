/*  Copyright (c) MediaArea.net SARL. All Rights Reserved.
 *
 *  Use of this source code is governed by a BSD-style license that can
 *  be found in the License.html file in the root of the source tree.
 */

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//
// Microsoft Visual C# example
//
// To make this example working, you must put MediaInfo.Dll and Example.ogg
// in the "./Bin/__ConfigurationName__" folder
// and add MediaInfoDll.cs to your project
//
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;
//using MediaInfoLib;
using Sarkui.core;
using System.Collections.Generic;
using Sarkui.core.details;

namespace Sarkui
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public partial class suinfo : Form
    {
        List<string> lola = new List<string>();



        public suinfo(ListBox frm)
        {
           
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            foreach (string it in frm.Items)
            {
                lola.Add(it);
            }
            //form1 = frm;

            //        minfo2 lola = new minfo2();
            //     lola.Load += new minfo2_Load();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
       
        }



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>

        /*   [STAThread]
            static void Main2()
            {
                Application.Run(new minfo());
            }
    */

        private void suinfo_Load_1(object sender, System.EventArgs e)
        {

            richTextBox2.AppendText(" [ Sub Streams ]\n");
            //richTextBox2.AppendText("\n");

            richTextBox2.Find("[ Sub Streams ]"); //Find the text provided

            
            if (richTextBox2.BackColor == Color.FromArgb(255, 40, 42,54))
            {
                richTextBox2.SelectionColor = Color.Yellow;

            }
            else
            {
                richTextBox2.SelectionColor = Color.Brown;

            }
            richTextBox2.Select(0, 1);            //Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"
            String ToDisplay = "";
            MediaInfo MI = new MediaInfo();


            //         ToDisplay = MI.Option("Info_Version", "0.7.0.0;MediaInfoDLL_Example_CS;0.7.0.0");
            //         if (ToDisplay.Length == 0)
            //          {
            //             richTextBox2.Text = "MediaInfo.Dll: this version of the DLL is not compatible";
            //             return;
            //         }

            //         Information about MediaInfo
            //                 ToDisplay += "\r\n\r\nInfo_Parameters\r\n";
            //               ToDisplay += MI.Option("Info_Parameters");

            //         ToDisplay += "\r\n\r\nInfo_Capacities\r\n";
            //               ToDisplay += MI.Option("Info_Capacities");

            //        ToDisplay += "\r\n\r\nInfo_Codecs\r\n";
            //      ToDisplay += MI.Option("Info_Codecs");

            //An example of how to use the library
            //          ToDisplay += "\r\nFile\r\n";
            //   Main newm = new Main();
            //       richTextBox2.ForeColor = Color.Red;


            //      RichTextBox _RichTextBox = new RichTextBox(); //Initialize a new RichTextBox of name _RichTextBox
       //     richTextBox2.Text = "\r[Audio Languages]\r\n";
      //      richTextBox2.SelectionColor = Color.Red;

       //     richTextBox2.Text = ToDisplay;
        //    richTextBox2.Find("Audio Languages"); //Find the text provided
            //   richTextBox2.Find("Audio Languages"); //Find the text provided
            //   richTextBox2.SelectionColor = Color.Red;
            int ind = 1;
            int line = 0;

            foreach (string file in lola)
            {            
                //    MyMessageBox.Show(file);
                MI.Open(file);
                ToDisplay += "--------------------------------------------------------------------------------------------\r";
                ToDisplay += "File " + ind + ": " + MI.Get(StreamKind.General, 0, "FileNameExtension") + "\r\n\n";
                ToDisplay += MI.Get(StreamKind.General, 0, "Text_Language_List");
                //    MI.Option("Inform", "Audio;%Language/String%, %Format%, %BitRate/String%");
                //       ToDisplay += MI.Inform();
                ToDisplay += "\n";
                ToDisplay += MI.Get(StreamKind.General, 0, "Text_Format_WithHint_List");
                ToDisplay += "\n";


                //       MI.Option("Inform", "Audio; %BitRate/String%");
                //        ToDisplay += MI.Inform();

                //         ToDisplay += MI.Get(StreamKind.General, 0, "Audio_Format_WithHint_List");

                //      ToDisplay += MI.Get(StreamKind.General, 0, "CodecID/String");

                ToDisplay += "--------------------------------------------------------------------------------------------\r";

                MI.Close();
           
                //        richTextBox2.Text = ToDisplay;
                ind++;
                line++;
          //      richTextBox2.AppendText(ToDisplay);

            }
            richTextBox2.AppendText(ToDisplay);
            richTextBox2.Select(0, 0);            //Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"

            //       richTextBox2.AppendText(ToDisplay);

            //         MI.Open(@"C:\Users\vector\Desktop\test\a.mkv");
            /*
                        ToDisplay += "\r\n\r\nInform with Complete=false\r\n";
                        MI.Option("Complete");
                        ToDisplay += MI.Inform();

                        ToDisplay += "\r\n\r\nInform with Complete=true\r\n";
                        MI.Option("Complete", "1");
                        ToDisplay += MI.Inform();

                        ToDisplay += "\r\n\r\nCustom Inform\r\n";
                        MI.Option("Inform", "General;File size is %FileSize% bytes");
                        ToDisplay += MI.Inform();

                        ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='FileSize'\r\n";
                        ToDisplay += MI.Get(0, 0, "FileSize");

                        ToDisplay += "\r\n\r\nGet with Stream=General and Parameter=46\r\n";
                        ToDisplay += MI.Get(0, 0, 46);
            */
            //    ToDisplay += "Audio Languages\r\n\n";
            //     ToDisplay += MI.Get(StreamKind.General,0, "Audio_Language_List");

            //          ToDisplay += "\r\n\r\nGet with Stream=General and Parameter='AudioCount'\r\n";
            //          ToDisplay += MI.Get(StreamKind.General, 0, "AudioCount");

            //         ToDisplay += "\r\n\r\nGet with Stream=Audio and Parameter='StreamCount'\r\n";
            //         ToDisplay += MI.Get(StreamKind.Audio, 0, "StreamCount");

            //        ToDisplay += "\r\n\r\nClose\r\n";

            //Example with a stream
            //      ToDisplay+="\r\n"+ExampleWithStream()+"\r\n";

            //Displaying the text




        }
        /*
                String ExampleWithStream()
                {
                    //Initilaizing MediaInfo
                    MediaInfo MI = new MediaInfo();

                    //From: preparing an example file for reading
                    FileStream From = new FileStream(@"C:\Users\vector\Desktop\test\a.mkv", FileMode.Open, FileAccess.Read);

                    //From: preparing a memory buffer for reading
                    byte[] From_Buffer = new byte[64 * 1024];
                    int From_Buffer_Size; //The size of the read file buffer

                    //Preparing to fill MediaInfo with a buffer
                    MI.Open_Buffer_Init(From.Length, 0);

                    //The parsing loop
                    do
                    {
                        //Reading data somewhere, do what you want for this.
                        From_Buffer_Size = From.Read(From_Buffer, 0, 64 * 1024);

                        //Sending the buffer to MediaInfo
                        System.Runtime.InteropServices.GCHandle GC = System.Runtime.InteropServices.GCHandle.Alloc(From_Buffer, System.Runtime.InteropServices.GCHandleType.Pinned);
                        IntPtr From_Buffer_IntPtr = GC.AddrOfPinnedObject();
                        Status Result = (Status)MI.Open_Buffer_Continue(From_Buffer_IntPtr, (IntPtr)From_Buffer_Size);
                        GC.Free();
                        if ((Result & Status.Finalized) == Status.Finalized)
                            break;

                        //Testing if MediaInfo request to go elsewhere
                        if (MI.Open_Buffer_Continue_GoTo_Get() != -1)
                        {
                            Int64 Position = From.Seek(MI.Open_Buffer_Continue_GoTo_Get(), SeekOrigin.Begin); //Position the file
                            MI.Open_Buffer_Init(From.Length, Position); //Informing MediaInfo we have seek
                        }
                    }
                    while (From_Buffer_Size > 0);

                    //Finalizing
                    MI.Open_Buffer_Finalize(); //This is the end of the stream, MediaInfo must finnish some work

                    //Get() example
                    //     return "Container format is " + MI.Get(StreamKind.General, 0, "Format");
                    return "Container format is " + MI.Get(StreamKind.Audio, 0, "AudioCount");

                }
       

        */

      

      




            
        





















    }
}