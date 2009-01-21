#if UNIT_TEST

using System.Configuration;
using NUnit.Framework;

namespace MetaDataSample
{
    [TestFixture]
    public class AppSettingsTest
    {
        private const string keyName = "Bird";

        [Test]
        public void ReadBirdType()
        {
            string birdType = ConfigurationManager.AppSettings[keyName];
            Assert.AreNotSame(birdType, null);
        }
    }
}

#endif