using AutoMapper;
using JS17.API.Models.Dtos;
using JS17.API.Persistence.Models;

namespace JS17.API.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Post, PostDto>();
    }
}
