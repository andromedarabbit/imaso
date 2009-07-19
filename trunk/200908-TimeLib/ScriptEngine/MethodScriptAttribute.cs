using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MethodScriptAttribute : Attribute
    {
        private readonly string _name;
        private readonly string _help;

        public MethodScriptAttribute(string name, string help)
        {
            this._name = name;
            this._help = help;
        }

        public string Name 
        {
            get { return this._name; }
        }

        public string Help
        {
            get { return this._help; }
        }
    }
}

