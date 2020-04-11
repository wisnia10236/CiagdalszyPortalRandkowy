using AutoMapper;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Helpers
{
    public class AutoMapperProfiles : Profile// konfiguracja Automappera aby wiedzial skad ma mapowac
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>(); // utworz mappera z listy User do UserForListDto
            CreateMap<User, UserForDetailedDto>();
        }
    }
}