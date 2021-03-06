using System.Diagnostics;
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
            CreateMap<UserForUpdateDto,User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto,Photo>();
            CreateMap<UserForRegisterDto,User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();       // zeby dzialalo tez w odwrotna strone
            CreateMap<Message,MessageToReturnDto>() // for member pokazuje dla mappera co ma mapowac dokladnie czyli dla sender photourl ma zmapowac od usera photo ktore jest main
                    .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                    .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}