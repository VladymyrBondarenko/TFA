namespace TFA.Storage.Common
{
    public interface IMomentProvider
    {
        DateTimeOffset Now { get; }
    }
}