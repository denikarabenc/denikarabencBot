using System;
using System.IO;

namespace Common.Creators
{
    public class FolderCreator
    {
        public FolderCreator()
        {
        }

        public void CreateFolder(string folderPath)
        {
            if(!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (UnauthorizedAccessException)
                {

                }
            }
        }
    }
}
