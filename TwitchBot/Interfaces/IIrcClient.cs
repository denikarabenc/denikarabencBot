namespace TwitchBot.Interfaces
{
    public interface IIrcClient
    {
        void PongMessage();
        void JoinRoom();
        void LeaveRoom();
        void SendChatMessage(string message);
        string ReadMessage();
    }
}
