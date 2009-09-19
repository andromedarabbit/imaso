using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using ScriptEngine;
using NUnit.Framework;

namespace ScriptEngineTest
{
    [TestFixture]
    public class ScriptAssemblyFinderTest
    {

        [MethodScript("TestMethodScript", "테스트용 스크립트!")]
        public class TestMethodScript
        {
        }

        [EventScript(0)]
        public class TestEventScript
        {
        }
        
        #region IsScriptClass 

        [Test]
        public void NotScriptClass()
        {
            Assert.IsFalse(
                ScriptAssemblyFinder.IsScriptClass(typeof (ScriptAssemblyFinderTest))
                );

        }

        [Test]
        public void MethodScriptIsScriptClass()
        {
            Assert.IsTrue(
                ScriptAssemblyFinder.IsScriptClass(typeof(TestMethodScript))
                );

        }

        [Test]
        public void EventScriptIsScriptClass()
        {
            Assert.IsTrue(
                ScriptAssemblyFinder.IsScriptClass(typeof(TestEventScript))
                );

        }

        #endregion 

        /*
        [Test]
        public void LoadScriptAssembly()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string asmPath = Path.Combine(baseDir, @"TestScripts\Assemblies\ScriptEngineTestScripts.dll");

            Assembly asm = Assembly.LoadFile(asmPath);

            var finder = new ScriptAssemblyFinder();
            finder.TryLoadingPlugin(asm);

            Assert.GreaterOrEqual(finder.NumberOfGoodTypes, 1);
        }

        [Test]
        public void LoadNonScriptAssembly()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string asmPath = Path.Combine(baseDir, @"ScriptEngine.dll");

            Assembly asm = Assembly.LoadFile(asmPath);

            var finder = new ScriptAssemblyFinder();
            finder.TryLoadingPlugin(asm);

            Assert.AreEqual(finder.NumberOfGoodTypes, 0);
        }
         * */
    }
}

