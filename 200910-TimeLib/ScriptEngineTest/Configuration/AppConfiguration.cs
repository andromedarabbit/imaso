using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ScriptEngineTest.Configuration
{
    public static class AppConfiguration
    {
        private const string TEST_ASSEMBLY_NAME = "ScriptEngineTestScripts";

        public static string TestAssemblyName
        {
            get { return TEST_ASSEMBLY_NAME; }
        }

        public static string TestAssemblyDir
        {
            get
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(baseDir, @"TestScripts\Assemblies");
            }
        }

        public static string TestAssemblyPath
        {
            get
            {
                return Path.Combine(TestAssemblyDir, TestAssemblyName + ".dll");
            }
        }
    }
}
