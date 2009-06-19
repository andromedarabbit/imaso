using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogManagedTest
{
    public class ManagedToNative
    {
        public static void CallMe()
        {
            LogManaged.Write("does it work?");
        }
    }
}
