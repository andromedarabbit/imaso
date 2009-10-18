using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ScriptEngine;
using NUnit.Framework;

namespace ScriptEngineTest
{
    [TestFixture]
    public class HelpEventScriptTest
    {
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void DeliberatlyThrowException()
        {
            var a = new ScriptInfo ();
            Console.WriteLine(a);

            var engine = new Engine();
            engine.InvokeMethod("Help", "메시지");
        }
    }
}
