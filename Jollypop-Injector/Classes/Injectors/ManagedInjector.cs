using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Jollypop_Injector.Injectors
{
    class ManagedInjector : InjectorBase
    {
        private string _monoJabberPath;
        private string _monoLoaderDLLPath;
        private string _memToolsDLLPath;

        public ManagedInjector()
        {
            InjectorOutput = new ObservableCollection<string>();
            SetMonoJabberPaths();
        }

        private void SetMonoJabberPaths()
        {
            string applicationDirectory = Directory.GetCurrentDirectory();
            _monoJabberPath = $"{applicationDirectory}\\MonoJabber.exe";
            _monoLoaderDLLPath = $"{applicationDirectory}\\MonoLoaderDLL.dll";
            _memToolsDLLPath = $"{applicationDirectory}\\MemTools.dll";
        }

        public void Inject(string TargetName, string DllLocation, string NameSpace, string ClassName, string MethodName)
        {
            string injectorArguments = $"{TargetName} {DllLocation} {NameSpace} {ClassName} {MethodName}";

            if (!(File.Exists(_monoJabberPath) && File.Exists(_monoLoaderDLLPath) && File.Exists(_memToolsDLLPath)))
                throw new FileNotFoundException("Either MonoJabber.exe, MonoLoaderDLL.dll, or MemTools.dll are missing." +
                    "Please ensure they are in the same folder as this application. Managed injection attempts will fail until they are.");

            DoInjection(_monoJabberPath, injectorArguments);

        }
    }
}
