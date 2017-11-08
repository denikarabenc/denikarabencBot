using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TwitchBot.LocalGameChecker
{
    public class LocalRunningGameChecker
    {
        private readonly LocalGameMapper mapper;

        public LocalRunningGameChecker(LocalGameMapper mapper)
        {
            mapper.ThrowIfNull(nameof(mapper));
            this.mapper = mapper;
        }

        public string GetWhichKnownGameIsRunning()
        {
            Process[] processes = Process.GetProcesses();
            List<string> currentRunningApplications = new List<string>();
            foreach (Process p in processes)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    currentRunningApplications.Add(p.MainWindowTitle);
                }
            }
            return KnownGameRunning(currentRunningApplications);
        }

        private string KnownGameRunning(List<string> currentRunningApplications)
        {
            foreach (string application in currentRunningApplications)
            {
                if (mapper.LocalGameList.Keys.Contains(application))
                {
                    return mapper.LocalGameList[application];
                }
            }
            return null;
        }
    }
}
