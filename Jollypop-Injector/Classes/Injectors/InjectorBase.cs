﻿using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Jollypop_Injector.Injectors
{
    abstract class InjectorBase
    {
        public ObservableCollection<string> InjectorOutput { get; internal set; }
        
        private Process CreateInjectorProcess(string InjectorPath, string InjectorArguments)
        {
            Process injectorProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = InjectorPath,
                    Arguments = InjectorArguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            return injectorProcess;
        }

        public void DoInjection(string InjectorPath, string InjectorArguments)
        {
            InjectorOutput.Clear();

            Process injectorProcess = CreateInjectorProcess(InjectorPath, InjectorArguments);
            injectorProcess.Start();
            while (!injectorProcess.StandardOutput.EndOfStream)
            {
                string outputLine = injectorProcess.StandardOutput.ReadLine();
                InjectorOutput.Add(outputLine);
            }
        }
    }
}
