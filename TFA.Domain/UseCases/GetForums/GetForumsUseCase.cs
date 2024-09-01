using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly IGetForumsStorage _storage;

        public GetForumsUseCase(IGetForumsStorage storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken = default)
        {
            return await _storage.GetForums();
        }
    }
}
