﻿using Common.Creators;
using System;
using System.IO;

namespace BotLogger.Preparers
{
    public class LogPreparer
    {
        private readonly string logFolderPath;
        private readonly string logfileName;
        private readonly string logProcessFolderPath;
        private readonly string logProcessfileName;

        public LogPreparer()
        {
            logFolderPath = Directory.GetCurrentDirectory() + "/" + "Logs";
            logProcessFolderPath = logFolderPath;
            logfileName = "log";
            logProcessfileName = "logProcess";
        }

        private void PrepareLog()
        {            
            Directory.CreateDirectory(logFolderPath);

            if (Directory.Exists(logFolderPath))
            {
                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateTxtFile(logFolderPath, logfileName);
            }
        }

        private void PrepareProcessLog()
        {
            Directory.CreateDirectory(logProcessFolderPath);

            if (Directory.Exists(logProcessFolderPath))
            {
                FileCreator fileCreator = new FileCreator();
                fileCreator.CreateTxtFile(logProcessFolderPath, logProcessfileName);
            }
        }

        public void PrepareLogs()
        {
            PrepareLog();
            PrepareProcessLog();
        }

        public string GetLogFilePath()
        {
            if (!File.Exists(logFolderPath + "/" + logfileName + ".txt"))
            {                
                return String.Empty;
            }

            return logFolderPath + "/" + logfileName + ".txt";
        }

        public string GetLogProcessFilePath()
        {
            if (!File.Exists(logProcessFolderPath + "/" + logProcessfileName + ".txt"))
            {
                return String.Empty;
            }

            return logProcessFolderPath + "/" + logProcessfileName + ".txt";
        }
    }
}
