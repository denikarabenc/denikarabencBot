using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBotOBSSetup.ConfigurationWritters
{
    public class OBSConfigurationProvider
    {
        public OBSConfigurationProvider()
        {

        }

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

        private bool OBSProfileExists(string profileName)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            folderPath = folderPath + "/Roaming/obs-studio/basic/profiles/"+profileName;

            return Directory.Exists(folderPath);
        }
       
    }
}
