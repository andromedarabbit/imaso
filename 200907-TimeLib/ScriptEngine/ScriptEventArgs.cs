using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptEngine
{
    public abstract class ScriptEventArgs
    {
        private readonly int _eventNo;

        protected ScriptEventArgs(int eventNo)
        {
            this._eventNo = eventNo;
        }

        public int EventNo
        {
            get { return _eventNo; }
        }
    }
}
