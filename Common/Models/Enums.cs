namespace Common.Models
{
    public enum CommandType
    {
        Ping,
        NotExist,
        ModsRequest,
        AddCommand,
        EditCommand,
        ReadCommand,
        TwitchStatusCommand,
        ChangeTitleCommand,
        UserInputCommand,
        MediaCommand,
    }

    public enum UserType // Last should have all previuos permision. Get status of user form twitch and check if he is one of the roles. Use description to map it to string.
    {
        Regular,
        Follower,
        Sub,
        Mod,
        Editor,
        King,
        Invalid
    }
}
