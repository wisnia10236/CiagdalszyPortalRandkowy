using System.Linq;
using AutoMapper;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Helpers
{
    public class AutoMapperProfiles : Profile// konfiguracja Automappera aby wiedzial skad ma mapowac
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()   // utworz mappera z listy User do UserForListDto
                    .ForMember(dest => dest.PhotoUrl, opt =>    // ustawiamy  ze ma wziac PhotoUrl z Photo dla DTO
                    {
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);    // przekazujemy ze ma wziac glowne zdj z photo i wpisujemy ze to wskazanie url
                    })
                    .ForMember(dest => dest.Age, opt =>
                    {
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge());             // stworzona klasa aby liczyla wiek
                    });
            CreateMap<User, UserForDetailedDto>()
                    .ForMember(dest => dest.PhotoUrl, opt =>
                    {
                        opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                    })
                    .ForMember(dest => dest.Age, opt =>
                    {
                        opt.MapFrom(src => src.DateOfBirth.CalculateAge());             // stworzona klasa aby liczyla wiek
                    });
            CreateMap<Photo, PhotosForDetailedDto>();
        }

    }
}