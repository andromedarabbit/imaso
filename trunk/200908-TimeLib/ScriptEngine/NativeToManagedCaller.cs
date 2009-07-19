using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace ScriptEngine
{
    public class NativeToManagedCaller
    {
        public static void Call(string className, string methodName)
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type classType = assembly.GetType(className);
                if(classType == null)
                    continue;

                MethodInfo methodInfo = classType.GetMethod(methodName);
                Debug.Assert(methodInfo != null);

                methodInfo.Invoke(null, null);
            }
        }
    }
}
