using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarkui.core.util
{

    
    public class AudioExtContainer
    {
        // used by all tools except MP4box
        private static readonly Dictionary<string, string> extensionsaudio;
   

        /// <summary>
        /// uses the ISO 639-2/B language codes
        /// </summary>
        public static Dictionary<string, string> audioextensions
        {
            get
            {
                return extensionsaudio;
            }
        }

      



        private static void addaudioext(string name, string iso3B)
        {
            extensionsaudio.Add(name, iso3B);

        }

        static AudioExtContainer()
        {
           

            extensionsaudio = new Dictionary<string, string>();



            addaudioext(".mp3", "libmp3lame");
            addaudioext(".aac", "aac");
            addaudioext(".ac3", "ac3");
            addaudioext(".wav", "");
            addaudioext(".m4a", "aac");


        }

        ///<summary>



        public static string GetLanguageFromFileName(string strFileName)
        {
            ArrayList arrText = new ArrayList();
            string strText = string.Empty;
            foreach (char c in strFileName.ToLowerInvariant())
            {
                if (!char.IsLetter(c))
                {
                    if (!String.IsNullOrEmpty(strText))
                        arrText.Add(strText);
                    strText = string.Empty;
                }
                else
                    strText += c;
            }
            if (!String.IsNullOrEmpty(strText))
                arrText.Add(strText);
            arrText.Reverse();

            foreach (string strTextPart in arrText)
            {
                foreach (KeyValuePair<string, string> strLanguage in extensionsaudio)
                {
                    if (strTextPart.Equals(strLanguage.Key.ToLowerInvariant()))
                        return strLanguage.Key;
                }
            }

            return string.Empty;
        }
    }
}