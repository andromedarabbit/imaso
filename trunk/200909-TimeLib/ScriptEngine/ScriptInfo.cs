using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ScriptEngine
{
    public struct ScriptInfo
    {
        private readonly string _asm;
        private readonly string _classType;

        public ScriptInfo(string asm, string classType)
        {
            Debug.Assert( !string.IsNullOrEmpty(asm) );
            Debug.Assert( !string.IsNullOrEmpty(classType) );

            _asm = asm;
            _classType = classType;
        }

        public string AssemblyIncluding
        {
            get { return _asm; }
        }

        public string ClassType
        {
            get { return _classType;}
        }
    }
}
