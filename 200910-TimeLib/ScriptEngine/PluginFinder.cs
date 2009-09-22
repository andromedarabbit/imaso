using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScriptEngine
{
    public class PluginFinder : PluginLoader<ScriptAssemblyFinder>
    {
        public PluginFinder(string pluginDirectory)
            : base(
                pluginDirectory, new ScriptInfo("ScriptEngine", "ScriptEngine.ScriptAssemblyFinder")
            )
        {
          
        }

        public List<string> GetScriptAssemblies()
        {
            return AssemblyFinder.Search(this.PluginDirectory);
        }


    }
}
