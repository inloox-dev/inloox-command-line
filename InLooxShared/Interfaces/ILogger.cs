namespace InLooxShared.Interfaces
{
    public interface ILogger
    {
        void WriteInfo(string text);
        void WriteError(string text);
    }
}
