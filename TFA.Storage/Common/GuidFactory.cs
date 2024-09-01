namespace TFA.Storage.Common
{
    internal class GuidFactory : IGuidFactory
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
