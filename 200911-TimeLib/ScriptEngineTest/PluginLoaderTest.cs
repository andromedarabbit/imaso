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
    public class PluginLoaderTest
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
            string asmName = "ScriptEngine"; 
            // string asmName = AppConfiguration.TestAssembly2Name;
            string asmDir = AppDomain.CurrentDomain.BaseDirectory;
            // string asmDir = AppConfiguration.TestAssembly2Dir;

            //Assert.IsFalse(CurrentDomainHasThisAsm(asmName));
            var count = AppDomain.CurrentDomain.GetAssemblies().Count();

            var info = new FirstInstanceInfo(asmName, asmName + ".ScriptAssemblyFinder");
            // var info = new FirstInstanceInfo(asmName, asmName + ".JustAnotherScriptAssemblyFinder");

            using (var finder = new PluginLoader<ScriptAssemblyFinder>(asmDir, info))
            {
                Assert.IsFalse(finder.CurrentDomainHasThisAsm(asmName));
                //Assert.IsFalse(CurrentDomainHasThisAsm(asmName));

                finder.Load();

                Assert.IsTrue(finder.CurrentDomainHasThisAsm(asmName));
                // Assert.IsFalse(CurrentDomainHasThisAsm(asmName));
                Assert.AreEqual(count, AppDomain.CurrentDomain.GetAssemblies().Count());
            }

            //Assert.IsFalse(CurrentDomainHasThisAsm(asmName));
            Assert.AreEqual(count, AppDomain.CurrentDomain.GetAssemblies().Count());
        }

    }
}
