using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace MetaDataSample
{
    class Program
    {
        static void Main(string[] args)
        {
            BirdPlayer player = new BirdPlayer();
            player.PlayNext();
            player.PlayNext();
        }
    }
}
