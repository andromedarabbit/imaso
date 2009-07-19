using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    public class Engine
    {
        private readonly string ScriptBaseDirectory;


        public Engine()
        {
            ScriptBaseDirectory = "Scripts";
        }

        private string AsseblyDirectory
        {
            get
            {
                return Path.Combine(ScriptBaseDirectory, "Assemblies");
            }
        }

        public void Initialize()
        {

        }

        public void InvokeMethod(string scriptName, string help)
        {
            throw new NotImplementedException();
        }

        public void InvokeEvent(ScriptEventArgs args)
        {
            throw new NotImplementedException();
            
        }

    }
}
