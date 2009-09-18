using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScriptEngine
{
    struct ScriptInfo
    {
        private readonly Assembly _asm;
        private readonly Type _classType;

        public ScriptInfo(Assembly asm, Type classType)
        {
            _asm = asm;
            _classType = classType;
        }

        public Assembly AssemblyIncluding
        {
            get { return _asm; }
        }

        public Type ClassType
        {
            get { return _classType;}
        }
    }
}
