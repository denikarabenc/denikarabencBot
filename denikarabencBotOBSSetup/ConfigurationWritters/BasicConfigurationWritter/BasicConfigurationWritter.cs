using Common.Creators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBotOBSSetup.ConfigurationWritters.BasicConfigurationWritter
{
    public class BasicConfigurationWritter
    {
        private const string basicFileName = "basic";

        private string obsBasicConfigurationFolderPathWithoutProfile;

        private FileCreator fileCreator;

        public BasicConfigurationWritter()
        {
            obsBasicConfigurationFolderPathWithoutProfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\obs-studio\\basic\\profiles\\";
            fileCreator = new FileCreator();
        }

        private bool OBSProfileExists(string profileName)
        {
            return profileName != null && Directory.Exists(obsBasicConfigurationFolderPathWithoutProfile + profileName);
        }

        public void WriteConfiguration(string profileName, Action<string> reportCallback)
        {
            BotLogger.Logger.Log(Common.Models.LoggingType.Info, "[BasicConfigurationWritter] -> WriteConfiguration started.");

            if (!OBSProfileExists(profileName))
            {
                throw new InvalidOperationException("OBS profile does not exist!");
            }

            reportCallback.Invoke(Properties.Resources.OBSConfiguration_CREATING_BASIC_BACKUP);

            CreateBasicBackup(profileName);

            reportCallback.Invoke(Properties.Resources.OBSConfiguration_SETTING_UP_BASIC_CONFIGURATION);

            WriteOBSHotkey(profileName);           

            reportCallback.Invoke(Properties.Resources.OBSConfiguration_DONE);
        }

        private void CreateBasicBackup(string profileName)
        {
            fileCreator.CreateBackupFile(obsBasicConfigurationFolderPathWithoutProfile + profileName, basicFileName, ".ini");            
        }

        private void WriteOBSHotkey(string profileName) //TODO
        {
            string filename = obsBasicConfigurationFolderPathWithoutProfile + profileName + "/" + basicFileName + ".ini";
            int index = 0;
            string lineToWrite = @"ReplayBuffer={\n    ""ReplayBuffer.Save"": [\n        {\n            ""key"": ""OBS_KEY_F13""\n        }\n    ]\n}";

            var txtLines = File.ReadAllLines(filename).ToList();
            bool hotkeysTagFound = false;
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "[Hotkeys]")
                    {
                        hotkeysTagFound = true;
                        BotLogger.Logger.Log(Common.Models.LoggingType.Info, String.Format("[BasicConfigurationWritter] -> Hotkeys tag found on {0} position", index));
                        break;
                    }
                    index++;
                }
            }

            if (hotkeysTagFound)
            {
                txtLines.Insert(index + 1, lineToWrite);
            }
            else
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Info, "[BasicConfigurationWritter] -> Hotkeys tag not found. Adding one");
                txtLines.Insert(index, "[Hotkeys]");
                txtLines.Insert(index + 1, lineToWrite);
            }

            File.WriteAllLines(filename, txtLines);
        }
    }
}
