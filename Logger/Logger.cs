using BotLogger.Preparers;
using Common.Helpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace BotLogger
{
    public static class Logger
    {
        private static LogPreparer logPreparer;

        static Logger()
        {
            logPreparer = new LogPreparer();
            logPreparer.PrepareLogs();
        }

        public static void Log(LoggingType loggingType, string logMessage)
        {
            string filePath = logPreparer.GetLogFilePath();

            if (filePath == String.Empty)
            {
                logPreparer.PrepareLogs();
                filePath = logPreparer.GetLogFilePath();
            }

            
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.Write("[{0} {1}] {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), loggingType.ToDescriptionString());                
                writer.WriteLine(" : {0}", logMessage);
            }
        }

        public static void Log(LoggingType loggingType, Exception ex)
        {
            string filePath = logPreparer.GetLogFilePath();

            if (filePath == String.Empty)
            {
                logPreparer.PrepareLogs();
                filePath = logPreparer.GetLogFilePath();
            }


            using (StreamWriter writer = File.AppendText(filePath))
            {
                //writter.Write("\r\nLog Entry : ");
                writer.Write("[{0} {1}] {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), loggingType.ToDescriptionString());
                writer.WriteLine(" : {0}", ex.Message);
                if (String.IsNullOrEmpty(ex.InnerException?.Message))
                {
                    writer.WriteLine("\t\t {0}", ex.InnerException);
                }
                writer.WriteLine("\t\t {0}, {1}", "Stack trace: ", ex.StackTrace);
            }
        }

        public static void Log(LoggingType loggingType, string logMessage, Exception ex)
        {
            logPreparer.PrepareLogs();

            string filePath = logPreparer.GetLogFilePath();

            if (filePath == String.Empty)
            {
                logPreparer.PrepareLogs();
                filePath = logPreparer.GetLogFilePath();
            }


            using (StreamWriter writer = File.AppendText(filePath))
            {
                //writter.Write("\r\nLog Entry : ");
                writer.Write("[{0} {1}] {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), loggingType.ToDescriptionString());
                writer.WriteLine(" : {0}", logMessage);
                writer.WriteLine("\t\t {0}", ex.Message);
                if (String.IsNullOrEmpty(ex.InnerException?.Message))
                {
                    writer.WriteLine("\t\t {0}", ex.InnerException);
                }
                writer.WriteLine("\t\t {0}, {1}", "Stack trace was: ", ex.StackTrace);
                
            }
        }

        public static void LogProcess()
        {
            logPreparer.PrepareLogs();

            string filePath = logPreparer.GetLogProcessFilePath();

            if (filePath == String.Empty)
            {
                logPreparer.PrepareLogs();
                filePath = logPreparer.GetLogFilePath();
            }

            using (StreamWriter writer = File.AppendText(filePath))
            {
                //writter.Write("\r\nLog Entry : ");
                writer.Write("[{0} {1}]", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                writer.WriteLine(" : Active Processes");

                System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
                List<string> currentRunningApplications = new List<string>();
                foreach (System.Diagnostics.Process p in processes)
                {
                    if (!String.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        writer.WriteLine("\t\t" + p.MainWindowTitle);
                    }
                }

            }
        }
    }    
}
