using System;   
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tests.Normalize
{
    using NUnit.Framework;
    using AIMLbot.Normalize;

    [TestFixture]
    public class StringNormailzationTests
    {
        [Test]
        public void KoreanNumericWithSpecialChars()
        {
            string text = "{} 가나다라마바사 12345 ?![";

            string characterRemovePattern = "[^\\p{L}0-9]";
            Regex regex = new Regex(characterRemovePattern, RegexOptions.IgnorePatternWhitespace);
            string normalizedText = regex.Replace(text, string.Empty);

            Assert.AreEqual("가나다라마바사12345", normalizedText);
        }

        [Test]
        public void EnglishNumericWithSpecialChars()
        {
            string text = "{} abCD 12345 ?![";

            string characterRemovePattern = "[^0-9a-zA-Z]";
            Regex regex = new Regex(characterRemovePattern, RegexOptions.IgnorePatternWhitespace);
            string normalizedText = regex.Replace(text, string.Empty);

            Assert.AreEqual("abCD12345", normalizedText);
        }
    }
}
