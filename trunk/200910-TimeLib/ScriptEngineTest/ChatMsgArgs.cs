using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptEngine;
using ScriptEngineTest;

namespace ScriptEngineTestScripts
{
    public class ChatMsgArgs : ScriptEventArgs
    {
        private readonly string _msg;

        public ChatMsgArgs(string msg)
            : base(EventType.ChatMsgReceived)
        {
            this._msg = msg;
        }

        public string Msg
        {
            get { return _msg; }
        }
    }
}
