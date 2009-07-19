using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScriptEngine
{
    public class AssemblyLoader
    {
        public static void Load(string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            Debug.Assert(assembly != null);
        }
    }
}
