using System;
using System.Collections.Generic;
using System.Text;

namespace MetaDataSample
{
    public class Duck : AbstractBird
    {
        public override void Sing()
        {
            Console.WriteLine("������ �����Ҹ�. �в�.");
        }
    }
}
