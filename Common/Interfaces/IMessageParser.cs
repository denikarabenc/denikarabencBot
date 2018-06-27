namespace Common.Interfaces
{
    public interface IMessageParser
    {
        /// <summary>
        /// Gets Senders user name
        /// </summary>
        /// <param name="message">original message got via irc</param>
        /// <returns>Senders user name</returns>
        string GetMessageSenderUserName(string message);

        /// <summary>
        /// Gets what user typed
        /// </summary>
        /// <param name="message">original message got via irc</param>
        /// <returns>What user typed</returns>
        string GetParsedMessage(string message);


        /// <summary>
        /// Get's what user typed
        /// </summary>
        /// <param name="message">original message got via irc</param>
        /// <returns></returns>
        string GetParsedModsMessage(string message);
    }
}
