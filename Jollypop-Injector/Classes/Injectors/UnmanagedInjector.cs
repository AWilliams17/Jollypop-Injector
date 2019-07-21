using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jollypop_Injector.Injectors
{
    // DLLPath
    // TargetName
    // Inject
    // OutputLines
    // InjectorProcess

    // Bitness
    class UnmanagedInjector
    {
        public ObservableCollection<string> UnmanagedInjectorOutput { get; private set; }


        public UnmanagedInjector()
        {
            UnmanagedInjectorOutput = new ObservableCollection<string>();
        }
    }
}
