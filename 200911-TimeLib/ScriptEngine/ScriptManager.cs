using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScriptEngine
{
    public class ScriptManager : IDisposable
    {
        private readonly PluginFinder _pluginFinder;
        private readonly Dictionary<long, EventScriptInvoker> _eventInvokers;
        private readonly Dictionary<string, MethodScriptInvoker> _methodInvokers;

        public ScriptManager(string pluginDirectory)
        {
            _pluginFinder = new PluginFinder(pluginDirectory);
            _eventInvokers = new Dictionary<long, EventScriptInvoker>();
            _methodInvokers = new Dictionary<string, MethodScriptInvoker>();
        }

        public void Initialize()
        {
            _pluginFinder.Load();

            LoadScriptAssemblies();
            RegisterScripts();
        }

        #region IDisposable 구현

        // Track whether Dispose has been called.
        private bool _disposed = false;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_pluginFinder != null)
                    {
                        _pluginFinder.Dispose();
                    }                    
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
               

                // Note disposing has been done.
                _disposed = true;

            }
        }

        ~ScriptManager()
        {
            Dispose(false);
        }

        #endregion

        private void LoadScriptAssemblies()
        {
            // BinDirectory에 있는 어셈블리를 모두 적재한다.
            var assemblies = _pluginFinder.GetScriptAssemblies();
            foreach(string assemblyName in assemblies)
            {
                AppDomain.CurrentDomain.Load(assemblyName);
            }
        }

        // TODO: ScriptAssemblyFinder.TryLoading 와 사실상 동일한 코드가 많으므로 리팩터링 대상이다.
        private void RegisterScripts()
        {
            var statelessScheduledScriptTypes = new List<Type>();
            foreach(var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Debug.Assert(asm != null);

                string asmName = asm.FullName;
                if(string.IsNullOrEmpty(asmName))
                    continue;

                // 약간의 최적화 코드: .NET Framework가 제공하는 기본 어셈블리라면 더 볼 것도 없다.
                if (asmName.StartsWith("mscorlib")
                     || asmName.StartsWith("System,")
                     || asmName.StartsWith("System."))
                {
                    continue;
                }

                foreach (Type t in asm.GetExportedTypes())
                {
                     if (t.IsClass == false || t.IsInterface == true)
                        continue;

                    if (t.IsAbstract == true && t.IsSealed == false) // static 클래스 == abstract seald
                        continue;
                    
                    if (Attribute.IsDefined(t, typeof(EventScriptAttribute)) == true)
                    {                        
                        object[] eventAttrs = t.GetCustomAttributes(typeof(EventScriptAttribute), false);
                        foreach (EventScriptAttribute eventAttr in eventAttrs)
                        {
                            Debug.Assert(eventAttr != null);

                            var eventType = eventAttr.EventType;

                            MethodInfo methodInfo = t.GetMethod("Execute", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                            Debug.Assert(methodInfo != null);                    
                            var scriptMethod = new ScriptMethodInfo(t, methodInfo);
                           
                            EventScriptInvoker invoker = null;
                            if (_eventInvokers.TryGetValue(eventType, out invoker) == false)
                            {
                                invoker = new EventScriptInvoker(eventType);

                                _eventInvokers.Add(eventType, invoker);
                            }

                            invoker.Methods.Add(scriptMethod);
                        }
                    }

                    if (Attribute.IsDefined(t, typeof(MethodScriptAttribute)) == true)
                    {
                        object[] methodAttrs = t.GetCustomAttributes(typeof(MethodScriptAttribute), false);
                        foreach (MethodScriptAttribute methodAttr in methodAttrs)
                        {
                            Debug.Assert(methodAttr != null);

                            var methodName = methodAttr.Name;

                            MethodInfo methodInfo = t.GetMethod("Execute", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
                            Debug.Assert(methodInfo != null);
                            var scriptMethod = new ScriptMethodInfo(t, methodInfo);

                            MethodScriptInvoker invoker = null;
                            if (_methodInvokers.TryGetValue(methodName, out invoker))
                            {
                                throw new ApplicationException("Two different method scripts with the same name found: " + methodName);
                            }
                            
                            invoker = new MethodScriptInvoker(methodName, scriptMethod);
                            _methodInvokers.Add(methodName, invoker);
                        }
                    }
                }
            }
        }

        public void InvokeEvent(ScriptEventArgs args)
        {
            Debug.Assert(args != null);

            EventScriptInvoker invoker;
            if (_eventInvokers.TryGetValue(args.EventNo, out invoker) == false)
            {
                var msg = string.Format("이벤트 번호 {0}짜리 스크립트는 없습니다.", args.EventNo);
                throw new ApplicationException(msg);
            }

            invoker.Invoke(args);
        }

        public void InvokeMethod(string methodName, params object[] args)
        {
            Debug.Assert( string.IsNullOrEmpty(methodName) == false );
            
            MethodScriptInvoker invoker;
            if (_methodInvokers.TryGetValue(methodName, out invoker) == false)
            {
                var msg = string.Format("{0}인 이름을 가진 메서드형 스크립트는 없습니다.", methodName);
                throw new ApplicationException(msg);
            }

            invoker.Invoke(args);
        }

    }
}
