using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denikarabencBotOBSSetup.Models
{
    public class SourceScenePair
    {
        private SourceEnum source;
        private string scene;

        public SourceScenePair()
        { }

        public SourceScenePair(SourceEnum source, string scene)
        {
            this.source = source;
            this.scene = scene;
        }

        public SourceEnum Source { get => source; set => source = value; }
        public string Scene { get => scene; set => scene = value; }
    }
}
