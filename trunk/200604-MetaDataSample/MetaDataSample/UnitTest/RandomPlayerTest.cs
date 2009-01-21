#if UNIT_TEST

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace MetaDataSample
{
    [TestFixture]
    public class RandomPlayerTest
    {
        [Test]
        public void NextSong()
        {
            BirdPlayer player = new BirdPlayer();
            Assert.AreNotEqual(player.MoveNext(), player.MoveNext());
        }
    }
}

#endif