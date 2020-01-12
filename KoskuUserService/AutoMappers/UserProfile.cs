using AutoMapper;
using KoskuUserService.Contracts;

namespace KoskuUserService.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
