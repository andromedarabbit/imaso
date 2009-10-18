using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    internal class MethodScriptInvoker
    {
        private readonly string _scriptName;
        private readonly ScriptMethodInfo _scriptMethod;

        public MethodScriptInvoker(string scriptName, ScriptMethodInfo scriptMethod)
        {
            _scriptName = scriptName;
            _scriptMethod = scriptMethod;
        }

        public string ScriptName
        {
            get { return _scriptName; }
        }

        public ScriptMethodInfo Method
        {
            get
            {
                return _scriptMethod;
            }
        }

        public void Invoke(params object[] args)
        {
            Object scriptObj = null;
            try
            {
                if (_scriptMethod.ScriptMethod.IsStatic == false)
                {
                    scriptObj = Activator.CreateInstance(_scriptMethod.ClassType, false);
                }

                _scriptMethod.ScriptMethod.Invoke(scriptObj, args);
            }
            finally
            {
                var disposableInterface = scriptObj as IDisposable;
                if (disposableInterface != null)
                    disposableInterface.Dispose();
            }
        }
        
    }
}
