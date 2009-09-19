﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ScriptEngine
{
    public class ScriptAssemblyFinder : MarshalByRefObject
    {
        private readonly List<ScriptInfo> _goodTypes = new List<ScriptInfo>();

        public List<ScriptInfo> Search(string scriptDir)
        {
            _goodTypes.Clear();
            foreach (string path in Directory.GetFiles(scriptDir, "*.dll"))
            {
                var file = new FileInfo(path);
                string asmName = file.Name.Replace(file.Extension, "");

                TryLoadingPlugin(asmName);
            }
            return _goodTypes.ToList<ScriptInfo>();
        }

        internal static bool IsScriptClass(Type t)
        {
            if (t.IsClass == false || t.IsAbstract == true)
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
            foreach (Type t in asm.GetExportedTypes())
            {
                if (IsScriptClass(t) == false)
                    continue;

                AddToGoodTypesCollection(asm, t);
            }
        }

        internal int NumberOfGoodTypes
        {
            get { return _goodTypes.Count; }
        }

        private void AddToGoodTypesCollection(Assembly asm, Type t)
        {
            var info = new ScriptInfo(asm.FullName, t.FullName);
            _goodTypes.Add(info);
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
