using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventScriptAttribute : Attribute
    {
        private readonly int _eventType;

        public EventScriptAttribute(int eventType)
        {
            this._eventType = eventType;
        }

        public int EventType
        {
            get { return this._eventType; }
        }
    }
}
