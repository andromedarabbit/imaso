using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ScriptEngine
{
    public class ScriptAssemblyFinder : MarshalByRefObject
    {
        private readonly List<string> _scriptAssemblies = new List<string>();

        public List<string> Search(string scriptDir)
        {
            _scriptAssemblies.Clear();
            foreach (string path in Directory.GetFiles(scriptDir, "*.dll"))
            {
                var file = new FileInfo(path);
                string asmName = file.Name.Replace(file.Extension, "");

                TryLoadingPlugin(asmName);
            }
            return _scriptAssemblies.ToList<string>();
        }

        public int NumberOfScriptAssemblies
        {
            get
            {
                return _scriptAssemblies.Count;
            }
        }

        internal static bool IsScriptClass(Type t)
        {
            if (t.IsClass == false || t.IsInterface == true)
                return false;

            if (t.IsAbstract == true && t.IsSealed == false) // static 클래스 == abstract seald
                return false;
            
            if (Attribute.IsDefined(t, typeof(EventScriptAttribute)) == true)
                return true;

            if (Attribute.IsDefined(t, typeof(MethodScriptAttribute)) == true)
                return true;

            return false;
        }

        internal void TryLoadingPlugin(string asmName)
        {
            Assembly asm = AppDomain.CurrentDomain.Load(asmName);
            TryLoadingPlugin(asm);
        }

        internal void TryLoadingPlugin(Assembly asm)
        {
            Debug.Assert(asm != null);

            string asmName = asm.FullName;
            if(string.IsNullOrEmpty(asmName))
                return;

            // 약간의 최적화 코드: .NET Framework가 제공하는 기본 어셈블리라면 더 볼 것도 없다.
            if (asmName.StartsWith("mscorlib")
                 || asmName.StartsWith("System,")
                 || asmName.StartsWith("System."))
            {
                return;
            }

            foreach (Type t in asm.GetExportedTypes())
            {
                if (IsScriptClass(t))
                {
                    _scriptAssemblies.Add(asm.FullName);
                    return;
                }
            }
        }

        internal bool CurrentDomainHasThisAsm(string asmName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Debug.Assert(asm != null);
                Debug.Assert(asm.FullName != null);

                if (asm.FullName.Contains(asmName))
                    return true;
            }
            return false;
        }
    }
}
