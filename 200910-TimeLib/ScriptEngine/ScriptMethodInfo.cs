using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScriptEngine
{
    internal class ScriptMethodInfo
    {
        private readonly Type _classType;
        private readonly MethodInfo _scriptMethod;

        public ScriptMethodInfo(Type classType, MethodInfo scriptMethod)
        {
            this._classType = classType;
            this._scriptMethod = scriptMethod;
        }

        public Type ClassType
        {
            get { return _classType; }
        }

        public MethodInfo ScriptMethod
        {
            get { return _scriptMethod; }
        }
    }
}
