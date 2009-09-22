using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using NUnit.Framework;
using ScriptEngine;
using ScriptEngineTestScripts;

namespace ScriptEngineTest
{
    using Configuration;

    [TestFixture]
    public class ScriptManagerTest
    {     
        [Test]
        public void LoadAndUnload()
        {
            string asmName = AppConfiguration.TestAssembly2Name;
            string asmDir = AppConfiguration.TestAssembly2Dir;

            var count = AppDomain.CurrentDomain.GetAssemblies().Count();
            int newCount = 0;

            using (var scriptManger = new ScriptManager(asmDir))
            {
                scriptManger.Initialize();
                newCount = AppDomain.CurrentDomain.GetAssemblies().Count();
                Assert.GreaterOrEqual(newCount, count);

                var eventArgs = new ChatMsgArgs("채팅 메시지");
                scriptManger.InvokeEvent(eventArgs);
            }

            Assert.AreEqual(newCount, AppDomain.CurrentDomain.GetAssemblies().Count());
        }
        
    }
}
