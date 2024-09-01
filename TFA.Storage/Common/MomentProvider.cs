
namespace TFA.Storage.Common
{
    internal class MomentProvider : IMomentProvider
    {
        public DateTimeOffset Now { get => DateTime.UtcNow; }
    }
}
