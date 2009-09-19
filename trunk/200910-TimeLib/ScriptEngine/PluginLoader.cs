using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ScriptEngine
{
    public class PluginLoader<TFirstScript>
        :  IDisposable
        where TFirstScript : class
    {
        private AppDomain _appDomain;
        private TFirstScript _assemblyFinder;
        private readonly ScriptInfo _firstScriptInfo;
        private readonly string _pluginDirectory;

        public PluginLoader(string pluginDirectory, ScriptInfo assemblyFinderType)// TAssemblyFinder finder)
        {
            Debug.Assert(string.IsNullOrEmpty(pluginDirectory) == false);

            _firstScriptInfo = assemblyFinderType;
            _pluginDirectory = pluginDirectory;
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
                    if (_assemblyFinder != null)
                    {
                        var disposable = _assemblyFinder as IDisposable;
                        if(disposable != null)
                            disposable.Dispose();
                    }
                    
                    Unload();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
               

                // Note disposing has been done.
                _disposed = true;

            }
        }

        ~PluginLoader()
        {
            Dispose(false);
        }

        #endregion

        protected AppDomain Domain
        {
            get { return _appDomain; }
        }

        protected TFirstScript AssemblyFinder
        {
            get { return _assemblyFinder; }
        }

        private System.Security.Policy.Evidence Evidence
        {
            get { return AppDomain.CurrentDomain.Evidence; }
        }

        private AppDomainSetup SetupInformation
        {
            get
            {
                var setup = new AppDomainSetup();
                setup.ApplicationName = _firstScriptInfo.ClassType;
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                setup.PrivateBinPath = _pluginDirectory; 
                setup.CachePath = Path.Combine(_pluginDirectory, "cache" + Path.DirectorySeparatorChar);
                setup.ShadowCopyFiles = "true";
                setup.ShadowCopyDirectories = _pluginDirectory;
                return setup;
            }
        }

        public void Load()
        {
#if DEBUG
            int numberOfAsms = AppDomain.CurrentDomain.GetAssemblies().Count();
#endif
            _appDomain = AppDomain.CreateDomain(_firstScriptInfo.AssemblyIncluding, Evidence, SetupInformation);
            
            _assemblyFinder = (TFirstScript)_appDomain.CreateInstanceAndUnwrap(
              _firstScriptInfo.AssemblyIncluding,
              _firstScriptInfo.ClassType
              );

            // _appDomain.AssemblyResolve += new ResolveEventHandler(Domain_AssemblyResolve);

#if DEBUG
            Debug.Assert(numberOfAsms == AppDomain.CurrentDomain.GetAssemblies().Count());
#endif
        }

        /*
        private static Assembly Domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AppDomain domain = sender as AppDomain;
            string pluginFolder = domain.RelativeSearchPath;
            string path = Path.Combine(pluginFolder, args.Name.Split(',')[0] + ".dll");
            return domain.Load(AssemblyName.GetAssemblyName(path));
        }
         * */

        public void Unload()
        {
            if (_appDomain == null)
                return;

            var disposable = _assemblyFinder as IDisposable;
            if(disposable != null)
                disposable.Dispose();

            AppDomain.Unload(_appDomain);
            _appDomain = null;
        }


        internal bool CurrentDomainHasThisAsm(string asmName)
        {
            var instance = _assemblyFinder as ScriptAssemblyFinder;
            if (instance == null)
                throw new ApplicationException();
            return instance.CurrentDomainHasThisAsm(asmName);
        }
    }
}
