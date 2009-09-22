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
        private const string TEST_ASSEMBLY2_NAME = "ScriptEngineTestScripts2";

        public static string TestAssemblyName
        {
            get { return TEST_ASSEMBLY_NAME; }
        }

        public static string TestAssembly2Name
        {
            get { return TEST_ASSEMBLY2_NAME; }
        }

        public static string TestAssemblyDir
        {
            get
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(baseDir, @"TestScripts\Assemblies");
            }
        }

        public static string TestAssembly2Dir
        {
            get
            {
                return TestAssemblyDir;
            }
        }


        public static string TestAssemblyPath
        {
            get
            {
                return Path.Combine(TestAssemblyDir, TestAssemblyName + ".dll");
            }
        }


        public static string TestAssembly2Path
        {
            get
            {
                return Path.Combine(TestAssembly2Dir, TestAssembly2Name + ".dll");
            }
        }
    }
}
