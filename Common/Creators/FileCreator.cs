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

        public void CreateFileIfNotExist(string folderPath, string fileName, string extention)
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

        public void CreateBackupFile(string folderPath, string fileName, string extention)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder path does not exist");
            }

            if (!extention.StartsWith("."))
            {
                extention = "." + extention;
            }

            int backupNumber = 0;

            try
            {
                if (File.Exists(folderPath + "/" + fileName + extention))
                {
                    while (File.Exists(folderPath + "/" + fileName + extention + ".bak" + backupNumber))
                    {
                        backupNumber++;
                    }

                    using (FileStream stream = File.OpenRead(folderPath + "/" + fileName + extention))
                    {
                        using (FileStream fs = File.Create(folderPath + "/" + fileName + extention + ".bak" + backupNumber))
                        {
                            byte[] buffer = new Byte[4096];
                            int bytesRead;

                            // while the read method returns bytes
                            // keep writing them to the output stream
                            while ((bytesRead =
                                    stream.Read(buffer, 0, 4096)) > 0)
                            {
                                fs.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }
    }
}
