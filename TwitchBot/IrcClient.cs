using System.IO;
using System.Net.Sockets;
using System.Timers;
using TwitchBot.Interfaces;

namespace TwitchBot
{
    public class IrcClient : IIrcClient
    {
        private readonly int timeBeetweenMessagesCanBeSent = 2 * 1000; //miliseconds

        private string username;
        private string channel;

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private Timer timer;
        private bool messageCanBeSent;

        public IrcClient(string ip, int port, string username, string password, string channel)
        {
            this.username = username;
            this.channel = channel;
            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());

            RegisterTwitchIRC(username, password);

            messageCanBeSent = true;
            timer = new Timer(timeBeetweenMessagesCanBeSent);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            messageCanBeSent = true;
        }

        private void RegisterTwitchIRC(string username, string password)
        {
            LoginToTwitch(username, password);
            RequestCommands();
            // RequestTags();
        }

        private void LoginToTwitch(string username, string password)
        {
            outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + username);
            outputStream.WriteLine("USER " + username + " 8 * :" + username);
            outputStream.Flush();
        }

        private void RequestMembership()
        {
            SendIrcMessage("CAP REQ :twitch.tv/membership");
        }

        private void RequestCommands()
        {
            SendIrcMessage("CAP REQ :twitch.tv/commands");
        }

        private void RequestTags()
        {
            SendIrcMessage("CAP REQ :twitch.tv/tags");
        }

        private void SendIrcMessage(string message)
        {
            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        private void SendControlledIrcMessage(string message)
        {
            if (!messageCanBeSent)
            {
                return;
            }
            SendIrcMessage(message);

            messageCanBeSent = false;
        }

        public void LeaveRoom()
        {
            outputStream.WriteLine("PART #" + channel);
            outputStream.Flush();
            SendChatMessage("/me Leaves the channel! FeelsBadMan");
        }

        public void JoinRoom()
        {
            outputStream.WriteLine("JOIN #" + channel);            
            outputStream.Flush();
            SendChatMessage("/me Joins the channel! FeelsGoodMan");            
        }

        public void SendInformationChatMessage(string message)
        {
            SendIrcMessage(":" + username + "!" + username + "@" + username + ".tmi.twtich.tv PRIVMSG #" + channel + " :" + message);
        }

        public void SendChatMessage(string message)
        {
            SendControlledIrcMessage(":" + username + "!" + username + "@" + username + ".tmi.twtich.tv PRIVMSG #" + channel + " :" + message);
        }

        public string ReadMessage()
        {
            try
            {
                string message = inputStream.ReadLine();
                if (message.Contains("PING :tmi.twitch.tv"))
                {
                    outputStream.WriteLine("PONG :tmi.twitch.tv");
                    outputStream.Flush();
                }
                return message;
            }
            catch(System.Exception e)
            {
                BotLogger.Logger.Log(Common.Models.LoggingType.Error, e);
                return string.Empty;
            }
        }

        public void PongMessage()
        {
            outputStream.WriteLine("PONG :tmi.twitch.tv");
            outputStream.Flush();
        }

        public void JoinedMessage()
        {
            SendChatMessage("/me Joins the channel!");           
        }
    }
}
