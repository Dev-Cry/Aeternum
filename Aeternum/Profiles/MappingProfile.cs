using Aeternum.Entities.User;
using AutoMapper;
namespace Aeternum.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Příklad mapování
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UserDto, ApplicationUser>();
        }
    }
}