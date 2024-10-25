using Aeternum.DTOs.User;
using Aeternum.Entities.User;
using AutoMapper;

namespace Aeternum.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapování mezi ApplicationUser a jednotlivými DTOs
            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserCreateDTO>();
            CreateMap<ApplicationUserCreateDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserRegisterDTO>();
            CreateMap<ApplicationUserRegisterDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserLoginDTO>();
            CreateMap<ApplicationUserLoginDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserUpdateDTO>();
            CreateMap<ApplicationUserUpdateDTO, ApplicationUser>();
        }
    }
}
