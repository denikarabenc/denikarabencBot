using TwitchBot;

namespace denikarabencBot.ViewModels
{
    public class BaseViewModel
    {
        private BotRunner bot;
        private string twitchChannelName;
       

        public BaseViewModel()
        {
            twitchChannelName = Properties.Settings.Default.TwitchUserName;
            //IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "denikarabencbot", "oauth:vv0yeswj1kpcmyvi381006bl7rxaj4", TwitchChannelName);
            //Bot = new BotRunner(irc);
        }

        public BotRunner Bot { get => bot; set => bot = value; }
        public string TwitchChannelName
        {
            get => twitchChannelName;
            set
            {
                twitchChannelName = value;
                Properties.Settings.Default.TwitchUserName = twitchChannelName;
                //OnPropertyChanged(nameof(TwitchChannelName));
            }
        }
    }
}
