using AutoMapper;
using TFA.Server.Models;

namespace TFA.Server.MappingProfiles
{
    internal class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Domain.Models.Forum, Forum>();
            CreateMap<Domain.Models.Topic, Topic>();
        }
    }
}