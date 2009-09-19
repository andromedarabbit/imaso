using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScriptEngine
{
    class PluginFinder : PluginLoader<ScriptAssemblyFinder>
    {
        public PluginFinder(string pluginDirectory)
            : base(
                pluginDirectory, new ScriptInfo()
            )
        {
          
        }

        public List<ScriptInfo> GetScripts(string dirPath)
        {
            return AssemblyFinder.Search(dirPath);
        }


    }
}
