using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using NUnit.Framework;
using ScriptEngine;

namespace ScriptEngineTest
{
    using Configuration;

    [TestFixture]
    public class PluginFinderTest
    {
        private static bool CurrentDomainHasThisAsm(string asmName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Assert.IsNotNull(asm);
                Assert.IsNotNull(asm.FullName);

                if (asm.FullName.Contains(asmName + ","))
                    return true;
            }
            return false;
        }


        [Test]
        public void LoadAndUnload()
        {
            const string asmName = "ScriptEngine";
            string asmDir = AppDomain.CurrentDomain.BaseDirectory;

            var count = AppDomain.CurrentDomain.GetAssemblies().Count();

            using (var finder = new PluginFinder(asmDir))
            {
                finder.Load();
                List<string> scripts = finder.GetScriptAssemblies();

                Assert.AreEqual(2, scripts.Count);

                Assert.IsTrue(finder.CurrentDomainHasThisAsm(asmName));
                Assert.AreEqual(count, AppDomain.CurrentDomain.GetAssemblies().Count());
            }

            Assert.AreEqual(count, AppDomain.CurrentDomain.GetAssemblies().Count());
        }

    }
}
