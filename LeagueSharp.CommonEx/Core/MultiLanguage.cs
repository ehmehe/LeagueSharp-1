#region

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace LeagueSharp.CommonEx.Core
{
    /// <summary>
    /// Class used to Translate textes in the Menu
    /// </summary>
    public static class MultiLanguage
    {
        /// <summary>
        /// Dictionary of Translations
        /// </summary>
        public static Dictionary<string, string> Translations = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textToTranslate"></param>
        /// <returns></returns>
        public static string _(string textToTranslate)
        {
            return Translations.ContainsKey(textToTranslate) ? Translations[textToTranslate] : textToTranslate;
        }

        /// <summary>
        /// Class of Translated entries.
        /// </summary>
        public class TranslatedEntry
        {
            /// <summary>
            ///  String that will be translated.
            /// </summary>
            [XmlAttribute] public string TextToTranslate;
            /// <summary>
            /// Translated String
            /// </summary>
            [XmlAttribute] public string TranslatedText;
        }
    }
}