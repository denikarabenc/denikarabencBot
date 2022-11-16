using Common.Interfaces;
using System;

namespace TwitchBot.Helpers
{
    public class TwitchMessageParser : IMessageParser
    {
        public string GetMessageSenderUserName(string message)
        {
            string userName = string.Empty;
            if (message.Contains("PRIVMSG"))
            {
                int intIndexParseSign = message.IndexOf('!');
                userName = message.Substring(1, intIndexParseSign - 1); // parse username from specific section (without quotes)
                                                                        // Format: ":[user]!"
                                                                        // Get user's message
            }

            return userName;
        }

        /// <summary>
        /// Get's what user typed
        /// </summary>
        /// <param name="message">original message got via irc</param>
        /// <returns>user's message</returns>
        public string GetParsedMessage(string message)
        {
            try
            {
                int intIndexParseSign = message.IndexOf(" :");
                message = message.Substring(intIndexParseSign + 2);
            }
            catch (Exception e)
            {
                string messageForLog = message ?? "null";
                BotLogger.Logger.Log(Common.Models.LoggingType.Warning, string.Format("[MessageParser] -> GetParsedMessage warning! Message was: {0}", messageForLog), e);
            }

            return message;
        }

        /// <summary>
        /// Get's what user typed
        /// </summary>
        /// <param name="message">original message got via irc</param>
        /// <returns>mod's message</returns>
        public string GetParsedModsMessage(string message)
        {
            int intIndexParseSign = message.IndexOf(": ");
            message = message.Substring(intIndexParseSign + 2);

            return message;
        }
    }
}
