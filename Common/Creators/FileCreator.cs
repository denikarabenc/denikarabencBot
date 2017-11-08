using System;
using System.IO;

namespace Common.Creators
{
    public class FileCreator
    {
        public FileCreator()
        {          
        }

        public void CreateTxtFile(string folderPath, string fileName)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder path does not exist");
            }
            try
            {
                if (!File.Exists(folderPath + "/" + fileName + ".txt"))
                {
                    using (FileStream fs = File.Create(folderPath + "/" + fileName + ".txt"))
                    {
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        public void CreateFile(string folderPath, string fileName, string extention)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder path does not exist");
            }

            if (!extention.StartsWith("."))
            {
                extention = "." + extention;
            }

            try
            {
                if (!File.Exists(folderPath + "/" + fileName + extention))
                {
                    using (FileStream fs = File.Create(folderPath + "/" + fileName + extention))
                    {
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }
    }
}
