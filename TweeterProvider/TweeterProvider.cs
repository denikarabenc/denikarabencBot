using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweeterProvider
{
    public class TweeterProvider : ITweeterProvider
    {
        private string screenName;

        public string ScreenName { get => screenName; set => screenName = value; }

        public TweeterProvider()
        {            
        }

        public TweeterProvider(string screenName)
        {
            this.screenName = screenName;
        }

        public object GetLatestStatus()
        {
            //TODO add retrieve from twitter
            throw new NotImplementedException();
        }

        public void PostStatus()
        {
            throw new NotImplementedException();
        }
    }
}
