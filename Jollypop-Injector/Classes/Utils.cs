﻿using SharpUtils.MiscUtils;
using System.Diagnostics;

namespace Jollypop_Injector.Classes
{
    public static class Utils
    {
        public static int GetPid(string ProcessName)
        {
            if (ProcessName.EndsWith(".exe") && ProcessName != ".exe")
                ProcessName = ProcessName.Substring(0, ProcessName.Length - 4);

            Process[] matchingProcesses = Process.GetProcessesByName(ProcessName);
            if (matchingProcesses.Length <= 0)
            {
                throw new ProcessNotFoundException("The specified process was not found. Is it running?");
            }

            return matchingProcesses[0].Id;
        }
    }
}
