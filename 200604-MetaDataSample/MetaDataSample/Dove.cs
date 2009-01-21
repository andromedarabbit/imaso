using System;
using System.Collections.Generic;
using System.Text;

namespace MetaDataSample
{
    public class Dove : AbstractBird
    {
        public override void Sing()
        {
            Console.WriteLine("비둘기 울음 소리. 구구?");
        }
    }
}
