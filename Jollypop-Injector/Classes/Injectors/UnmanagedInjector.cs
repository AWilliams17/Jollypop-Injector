﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Jollypop_Injector.Injectors
{
    class UnmanagedInjector : InjectorBase
    {
        private string _injector32BitPath;
        private string _injector64BitPath;

        public enum Bitness
        {
            BIT_32 = 0,
            BIT_64 = 1
        }

        public UnmanagedInjector()
        {
            InjectorOutput = new ObservableCollection<string>();
            SetPaths();
        }


        private void SetPaths()
        {
            string applicationDirectory = Directory.GetCurrentDirectory();
            _injector32BitPath = $"{applicationDirectory}\\Unmanaged32Bit.exe";
            _injector64BitPath = $"{applicationDirectory}\\Unmanaged64Bit.exe";

            if (!(File.Exists(_injector32BitPath) && File.Exists(_injector64BitPath)))
                throw new FileNotFoundException("One or both of the unmanaged injectors are missing. " +
                    "Please ensure they are in the same folder as this application. Unmanaged injection will fail until they are.");
        }


        public void Inject(int TargetPID, string DllLocation, Bitness injectorBitness)
        {
            string injectorArguments = $"{TargetPID} {DllLocation}";

            if (injectorBitness == Bitness.BIT_32)
                DoInjection(_injector32BitPath, injectorArguments);
            else
                DoInjection(_injector64BitPath, injectorArguments);
        }
    }
}
