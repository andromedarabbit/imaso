using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    internal class EventScriptInvoker
    {
        private readonly long _scriptEventId;
        private readonly List<ScriptMethodInfo> _scriptMethods = new List<ScriptMethodInfo>();

        public EventScriptInvoker(long scriptEventId)
        {
            _scriptEventId = scriptEventId;
        }

        public long ScriptEventId
        {
            get { return _scriptEventId; }
        }

        public List<ScriptMethodInfo> Methods
        {
            get
            {
                return _scriptMethods;
            }
        }

        public void Invoke(ScriptEventArgs args)
        {
            foreach (ScriptMethodInfo item in _scriptMethods)
            {
                Object scriptObj = null;
                try
                {
                    if (item.ScriptMethod.IsStatic == false)
                    {
                        scriptObj = Activator.CreateInstance(item.ClassType, false);
                    }

                    item.ScriptMethod.Invoke(scriptObj, new object[] { args });
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
}
