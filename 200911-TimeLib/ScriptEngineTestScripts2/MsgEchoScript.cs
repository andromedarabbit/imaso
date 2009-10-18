using System;

using ScriptEngine;
using ScriptEngineTest;
using ScriptEngineTestScripts;

namespace ScriptEngineTestScripts2
{
    [EventScript(EventType.ChatMsgReceived)]
    public class MsgEchoScript
    {
        public void Execute(ChatMsgArgs args)
        {
            Console.Write(args.Msg);
        }
    }
}
