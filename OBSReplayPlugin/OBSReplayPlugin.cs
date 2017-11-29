using CLROBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLogger;
using OBSReplayPlugin.OBSPlugin;

namespace OBSReplayPlugin
{
    public class OBSReplayPlugin :  Plugin
    {
        private readonly string logPrefix;
        public OBSReplayPlugin()
        {
            logPrefix = "[OBSReplayPlugin] ->";
            Description = "Video window for Replay functionality";
            Name = "Replay Window Plugin";
        }

        public string Description { get; }
            

        public string Name { get; }

        public bool LoadPlugin()
        {
            API.Instance.AddImageSourceFactory(new WindowFactory());

            Logger.Log(String.Format("{0} Plugin loaded", logPrefix));

            return true; //TODO
        }

        public void OnStartStream()
        {
            Logger.Log(String.Format("{0} Stream started", logPrefix));
        }

        public void OnStopStream()
        {
            Logger.Log(String.Format("{0} Stream stopped", logPrefix));
        }

        public void UnloadPlugin()
        {
            Logger.Log(String.Format("{0} Plugin unloaded", logPrefix));
        }
    }
}
