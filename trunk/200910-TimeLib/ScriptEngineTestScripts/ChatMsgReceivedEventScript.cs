using System;

using ScriptEngine;
using ScriptEngineTest;

namespace ScriptEngineTestScripts
{
    [EventScript(EventType.ChatMsgReceived)]
    public class ChatMsgReceivedEventScript
    {
        public void Execute(ChatMsgArgs args)
        {
            Console.Write("테스트용 채팅 메시지: " + args.Msg);
        }
    }
}
