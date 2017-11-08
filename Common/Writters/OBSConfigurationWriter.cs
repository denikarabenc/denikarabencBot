

using System;
using System.Collections.Generic;
using System.IO;

namespace Common.Writters
{
    public class OBSConfigurationWriter
    {
        private string[] GetOBSProfileFolders()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = folderPath + "/Roaming/obs-studio/basic/profiles";

            if (!Directory.Exists(folderPath))
            {
                throw new InvalidOperationException("OBS studio is not installed. Check if there is configuration obs data in %appdata%");
            }

            return Directory.GetDirectories(folderPath);

        }

        //private void SetOBSProfileConfiguration(string folderPath)
        //{
        //    string configFile = folderPath + "/basic.ini";
        //    if (!File.Exists(configFile))
        //    {
        //        return;
        //    }

        //    StreamReader reader = new StreamReader(configFile);
        //    string line;
        //    int counter = 0;
        //    Dictionary<string, int> writeToFile = new Dictionary<string, int>();

        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        if (line.StartsWith("Mode"))
        //        {
        //            writeToFile.Add("Mode", counter);
        //        }

        //        if (line == "[Output]")
        //        {
        //            writeToFile.Add("[Output]", counter);
        //        }
        //        if (line == "[Output]")
        //        {
        //            writeToFile.Add("[Output]", counter);
        //        }
        //        if (line == "[Output]")
        //        {
        //            writeToFile.Add("[Output]", counter);
        //        }
        //        if (line == "[Output]")
        //        {
        //            writeToFile.Add("[Output]", counter);
        //        }
        //        counter++;
        //    }



        ////    using (StreamWriter writer = File.AppendText(filePath))
        ////    {
        ////        //writter.Write("\r\nLog Entry : ");
        ////        writer.Write("[{0} {1}]", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
        ////        writer.WriteLine(" : {0}", ex.Message);
        ////        if (String.IsNullOrEmpty(ex.InnerException.Message))
        ////        {
        ////            writer.WriteLine("\t\t {0}", ex.InnerException);
        ////        }
        ////        writer.WriteLine("\t\t {0}, {1}", "Stack trace: ", ex.StackTrace);
        ////    }
        //}

        //public void CreateEntry(string npcName) //npcName = "item1"
        //{
        //    var fileName = "test.txt";
        //    var endTag = String.Format("[/{0}]", npcName);
        //    var lineToAdd = "//Add a line here in between the specific boundaries";

        //    var txtLines = File.ReadAllLines(fileName).ToList();   //Fill a list with the lines from the txt file.
        //    txtLines.Insert(txtLines.IndexOf(endTag), lineToAdd);  //Insert the line you want to add last under the tag 'item1'.
        //    File.WriteAllLines(fileName, txtLines);                //Add the lines including the new one.
        //}

        //public void WriteConfiguration(string userProfile)
        //{
            
        //}
    }
}
