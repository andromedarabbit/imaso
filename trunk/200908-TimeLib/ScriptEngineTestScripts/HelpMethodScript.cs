using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ScriptEngine;

namespace ScriptEngineTestScripts
{
    [MethodScript("Help", "테스트용 스크립트!")]
    public class HelpMethodScript
    {
        public void Execute(string args)
        {
            Console.WriteLine("테스트 메시지: " + args);
        }
    }
}
