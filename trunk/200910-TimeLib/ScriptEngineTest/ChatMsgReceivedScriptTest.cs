  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ScriptEngine;
using ScriptEngineTestScripts;
using NUnit.Framework;


namespace ScriptEngineTest
{
    [TestFixture]
    public class ChatMsgReceivedScriptTest
    {
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void DeliberatlyThrowException()
        {
            var engine = new Engine();

            var args = new ChatMsgArgs("메시지");
            engine.InvokeEvent(args);
        }
    }
}
