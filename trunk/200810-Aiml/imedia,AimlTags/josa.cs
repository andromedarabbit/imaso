using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using AIMLbot.Utils; 

namespace imedia.AimlTags
{
    [CustomTag]
    public class josa : AIMLTagHandler
    {
        private readonly string _tagname = "josa";

        public josa()
        {
            this.inputString = _tagname;
        }

        protected override string ProcessChange()
        {
            if( string.Compare(this.templateNode.Name, _tagname, true) != 0 )
                return string.Empty;

            if (this.templateNode.Attributes.Count != 1)
                return string.Empty;

            string subject = this.templateNode.InnerText;
            string suffix = string.Empty;

            if (subject.Length == 0)
                return string.Empty;

            foreach(XmlAttribute xmlAttr in this.templateNode.Attributes)
            {

                if(xmlAttr.Name == "suffix")
                    suffix = xmlAttr.Value;
            }

            return Combine(subject, suffix);
        }

        internal static string Combine(string subject, string suffix)
        {
            if (suffix != "은는" && suffix != "이가" && suffix != "과와" && suffix != "을를")
            {
                if (subject.Length > 0)
                {
                    return subject;
                }
                return string.Empty;
            }

            char[] josaCandidates = suffix.ToCharArray();


            return Combine(subject, josaCandidates);
        }

        private static string Combine(string subject, char[] josaCadidates)
        {            
            char lastChar = subject[subject.Length - 1];
            string lastCharString = lastChar.ToString();

            HangulUtil.LanguageType lang = HangulUtil.GetLangageType(lastCharString);
            if(
                lang != HangulUtil.LanguageType.Korean
                && lang != HangulUtil.LanguageType.KoreanJaum
                && lang != HangulUtil.LanguageType.KoreanMoum
                ) // 한국어가 아닐 때
            {
                RegexOptions options = RegexOptions.IgnorePatternWhitespace;
                options |= RegexOptions.IgnoreCase;

                Regex regex = new Regex("[aeiou]", options);
                if (regex.IsMatch(lastCharString) == true)
                    return subject + josaCadidates[1];

                return subject + josaCadidates[0];
            }
            
            // 한국어일 때
            char[] decomposed = HangulUtil.DivideJaso(lastChar);

            if ( 
                decomposed.Length < 3
                || (decomposed.Length == 3 && decomposed[2] == ' ')
                )
            {
                return subject+josaCadidates[1];
            }

            return subject+josaCadidates[0];
        }
    }
}
