namespace Common.Interfaces
{
    public interface IStreamUpdater
    {
        string SetStreamGameAndReturnWhichGameIsSet(string game);
        string SetStreamStatusAndReturnWhichStatusIsSet(string status);
    }
}
