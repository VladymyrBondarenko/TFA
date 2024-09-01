using AutoMapper;
using TFA.Storage.Models;

namespace TFA.Storage.MappingProfiles
{
    internal class StorageToDomainProfile : Profile
    {
        public StorageToDomainProfile()
        {
            CreateMap<Forum, Domain.Models.Forum>();
            CreateMap<Topic, Domain.Models.Topic>();
        }
    }
}