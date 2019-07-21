﻿using SharpUtils.MiscUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jollypop_Injector.Classes
{
    public static class Utils
    {
        public static int GetPid(string ProcessName)
        {
            Process[] matchingProcesses = Process.GetProcessesByName(ProcessName);
            if (matchingProcesses.Length <= 0)
            {
                throw new ProcessNotFoundException("The specified process was not found. Is it running?");
            }

            return matchingProcesses[0].Id;
        }
    }
}
