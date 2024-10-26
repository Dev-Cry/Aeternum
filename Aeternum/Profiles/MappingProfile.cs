using AutoMapper;
using Aeternum.Entities.User;
using Aeternum.DTOs.User;
using Aeternum.DTOs.Role;

namespace Aeternum.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapování mezi ApplicationUser a ApplicationUserDTO
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();

            // Mapování pro vytváření uživatele
            CreateMap<ApplicationUserCreateDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorujte ID, protože je generováno
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapování pro aktualizaci uživatele
            CreateMap<ApplicationUserUpdateDTO, ApplicationUser>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapování pro registraci uživatele
            CreateMap<ApplicationUserRegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapování pro přihlášení uživatele
            CreateMap<ApplicationUserLoginDTO, ApplicationUser>();

            // Mapování mezi ApplicationRole a RoleDto
            CreateMap<ApplicationRole, ApplicationRoleDTO>().ReverseMap();
        }
    }
}