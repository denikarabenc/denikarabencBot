using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLROBS;

namespace OBSReplayPlugin.OBSPlugin
{
    public class WindowFactory : AbstractImageSourceFactory
    {
        public override ImageSource Create(XElement data)
        {
            return null;
        }
        public override bool ShowConfiguration(XElement data)
        {
            return false;
        }

    }
}
