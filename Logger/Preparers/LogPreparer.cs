using Common.Creators;
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
        private readonly double maximumLogSizeInKb;
        private int splittedLogCounter;

        public LogPreparer()
        {
            logFolderPath = Directory.GetCurrentDirectory() + "/" + "Logs";
            logProcessFolderPath = logFolderPath;
            logfileName = "log";
            logProcessfileName = "logProcess";
            maximumLogSizeInKb = 1000 * 100; //100 Mb
            splittedLogCounter = GetSplittedLogCount();
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

        public string GetLogFilePathForLogging()
        {
            if (!File.Exists(logFolderPath + "/" + logfileName + ".txt"))
            {                
                return String.Empty;
            }

            SplitLogFileIfTooBig();

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

        private string GetLogFilePath()
        {
            if (!File.Exists(logFolderPath + "/" + logfileName + ".txt"))
            {
                return String.Empty;
            }

            return logFolderPath + "/" + logfileName + ".txt";
        }

        private string GetLogFilePathWithoutExtention()
        {
            if (!File.Exists(logFolderPath + "/" + logfileName + ".txt"))
            {
                return String.Empty;
            }

            return logFolderPath + "/" + logfileName;
        }

        private int GetSplittedLogCount()
        {
            if (File.Exists(GetLogFilePathWithoutExtention() + "." + splittedLogCounter.ToString()))
            {
                splittedLogCounter++;
                GetSplittedLogCount();
            }
            return splittedLogCounter;
        }

        private void SplitLogFileIfTooBig()
        {
            FileInfo fileInfo = new FileInfo(GetLogFilePath());
            if (fileInfo.Length > maximumLogSizeInKb * 1024)
            {
                try
                {
                    File.Move(GetLogFilePath(), GetLogFilePathWithoutExtention() + "." + splittedLogCounter.ToString());
                    splittedLogCounter++;
                }
                catch
                {
                   //No need to handle exception. This means something is being logged to file, so we need to wait for it to finish. Probably will never happen.
                }
            }
        }
    }
}
