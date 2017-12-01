namespace TwitchBot.Interfaces
{
    public interface IIrcClient
    {
        /// <summary>
        /// Keep connection to Twitch alive
        /// </summary>
        void PongMessage();
        /// <summary>
        /// Join the chat room
        /// </summary>
        void JoinRoom();
        /// <summary>
        /// Leave the chat room
        /// </summary>
        void LeaveRoom();
        /// <summary>
        /// Sends message to chat with checking the message limit
        /// </summary>
        /// <param name="message">Exact message to be sent to chat</param>
        /// <remarks>Use this for commands</remarks>
        void SendChatMessage(string message);
        /// <summary>
        /// Sends message to chat without checking the message limit
        /// </summary>
        /// <param name="message">Exact message to be sent to chat</param>
        /// <remarks>Use this for showing errors</remarks>
        void SendInformationChatMessage(string message);
        /// <summary>
        /// Read the message form chat
        /// </summary>
        /// <returns></returns>
        string ReadMessage();
    }
}
