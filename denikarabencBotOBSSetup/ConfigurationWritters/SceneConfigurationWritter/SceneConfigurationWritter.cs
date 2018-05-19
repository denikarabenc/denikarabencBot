using Common.Creators;
using denikarabencBotOBSSetup.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBotOBSSetup.ConfigurationWritters.SceneConfigurationWritter
{
    public class SceneConfigurationWritter
    {
        private string obsSceneConfigurationFolderPath;

        private FileCreator fileCreator;

        public SceneConfigurationWritter()
        {
            obsSceneConfigurationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\obs-studio\\basic\\scenes\\";
            fileCreator = new FileCreator();
        }

        private bool OBSScenesProfileExists(string profileName)
        {
            return profileName != null && File.Exists(obsSceneConfigurationFolderPath + profileName + ".json");
        }

        public void WriteConfiguration(string profileName, List<string> replayScenes, List<string> clipScenes, Action<string> reportCallback)
        {
            BotLogger.Logger.Log(Common.Models.LoggingType.Info, "[SceneConfigurationWritter] -> WriteConfiguration started.");

            if (!OBSScenesProfileExists(profileName))
            {
                throw new InvalidOperationException("OBS scene profile does not exist!");
            }

            reportCallback.Invoke(Properties.Resources.OBSConfiguration_CREATING_SCENES_BACKUP);

            //CreateScenesBackup(profileName);

            reportCallback.Invoke(Properties.Resources.OBSConfiguration_SETTING_UP_SCENES_CONFIGURATION);

            WriteOBSReplayScenes(profileName, replayScenes);

            reportCallback.Invoke(Properties.Resources.OBSConfiguration_DONE);
        }

        private void CreateScenesBackup(string profileName)
        {
            fileCreator.CreateBackupFile(obsSceneConfigurationFolderPath, profileName, ".json");
        }

        private void WriteOBSReplayScenes(string profileName, List<string> replayScenes)
        {
            OBSSceneInfo obsSceneInfo;
            using (StreamReader file = File.OpenText(obsSceneConfigurationFolderPath + profileName + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
               // Object obsSceneInfo = (Object)serializer.Deserialize(file, typeof(Object));
                obsSceneInfo = OBSSceneInfo.FromJson(file.ReadToEnd());                            
            }

            if (!ScenesExist(obsSceneInfo, replayScenes))
            {
                throw new InvalidOperationException("OBS scene does not exist!");
            }

            AddTwithcReplaySourceIfNeeded(ref obsSceneInfo);
            ModifyOBSSceneInfoWithReplay(ref obsSceneInfo, replayScenes);

            string json = obsSceneInfo.ToJson();

            System.IO.File.WriteAllText(obsSceneConfigurationFolderPath + profileName + ".json", json);
        }

        private bool ScenesExist(OBSSceneInfo obsSceneInfo, List<string> scenes) //TODO return ErrorCode and send which scene does not exist
        {
            Dictionary<string, bool> sceneExistance = new Dictionary<string, bool>();
            for (int i = 0; i < scenes.Count; i++)
            {
                sceneExistance.Add(scenes[i], false);
            }

            List<Source> containedScenes = obsSceneInfo.Sources.Where(x => (scenes.Contains(x.Name))).ToList();

            for (int i = 0; i < containedScenes.Count; i++)
            {
                if (sceneExistance.ContainsKey(containedScenes[i].Name))
                {
                    sceneExistance[containedScenes[i].Name] = true;
                }
            }

            bool result = sceneExistance.Count > 0;
            foreach(bool value in sceneExistance.Values)
            {
                if (!value)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private Source CreateTwitchReplaySource()
        {
            Source replaySource = new Source();
            replaySource.DeinterlaceFieldOrder = 0;
            replaySource.DeinterlaceMode = 0;
            replaySource.Enabled = true;
            replaySource.Flags = 0;
            replaySource.Hotkeys = new SourceHotkeys();
            replaySource.Id = Properties.Resources.OBSConfiguration_OBS_SCENE_ID_WINDOW_CAPTURE;
            replaySource.Mixers = 0;
            replaySource.MonitoringType = 0;
            replaySource.Muted = false;
            replaySource.Name = Properties.Resources.OBSConfiguration_DENIKARABENCBOT_REPLAY;
            replaySource.PrivateSettings = new PrivateSettings();
            replaySource.PushToMute = false;
            replaySource.PushToMuteDelay = 0;
            replaySource.PushToTalk = false;
            replaySource.PushToTalkDelay = 0;
            replaySource.Settings = new SourceSettings() { Cursor = false, Priority = 1, Window = Properties.Resources.OBSConfiguration_OBS_SCENE_ID_REPLAY_SETTING_WINDOW };
            replaySource.Sync = 0;
            replaySource.Volume = 1;

            return replaySource;
        }

        private void AddTwithcReplaySourceIfNeeded(ref OBSSceneInfo obsSceneInfo)
        {
            bool shouldAdd = true;
            foreach (Source scene in obsSceneInfo.Sources)
            {
                if (scene.Name == Properties.Resources.OBSConfiguration_DENIKARABENCBOT_REPLAY)
                {
                    shouldAdd = false;
                    break;
                }
            }

            if (shouldAdd)
            {
                obsSceneInfo.Sources.Add(CreateTwitchReplaySource());
            }
        }

        private void ModifyOBSSceneInfoWithReplay(ref OBSSceneInfo obsSceneInfo, List<string> scenesToModify)
        {
            foreach (string sceneName in scenesToModify)
            {
                foreach (Source scene in obsSceneInfo.Sources)
                {
                    if (scene.Name != sceneName)
                    {
                        continue;
                    }

                    var denikarabencBotReplay = scene.Settings.Items.Where(x => x.Name == Properties.Resources.OBSConfiguration_DENIKARABENCBOT_REPLAY).ToList();
                    if (denikarabencBotReplay != null && denikarabencBotReplay.Count > 0)
                    {
                        break;
                    }                   

                    scene.Settings.Items.Add(CreateTwitchReplaySourceItem());
                    break;
                }
            }
        }

        private Item CreateTwitchReplaySourceItem()
        {
            Item replaySourceItem = new Item();

            replaySourceItem.Align = 5; //What is this for
            replaySourceItem.Bounds = new Bounds() {X = 1024, Y= 575};
            replaySourceItem.BoundsAlign = 0;
            replaySourceItem.BoundsType = 2;
            replaySourceItem.CropBottom = 0;
            replaySourceItem.CropLeft = 0;
            replaySourceItem.CropRight = 0;
            replaySourceItem.CropTop = 0;
            replaySourceItem.Id = 9999; //For safety;
            replaySourceItem.Locked = false;
            replaySourceItem.Name = Properties.Resources.OBSConfiguration_DENIKARABENCBOT_REPLAY;
            replaySourceItem.Pos = new Bounds() { X = 0, Y = 0 };
            replaySourceItem.PrivateSettings = new PrivateSettings();
            replaySourceItem.Rot = 0;
            replaySourceItem.Scale = new Bounds() { X = 1, Y = 1 };
            replaySourceItem.ScaleFilter = ScaleFilter.Disable;
            replaySourceItem.Visible = true;

            return replaySourceItem;
        }
    }
}
