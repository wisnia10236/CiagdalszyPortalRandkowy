using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Helpers;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<ClaudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudynary;

        public PhotosController(IUserRepository repository, IMapper mapper, IOptions<ClaudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;           //wstrzykiwanie zaleznosci dla photocontroller (cloudinaryconfig,repo,mapper)
            _repository = repository;
            _mapper = mapper;


            Account account = new Account(              //przypisujemy konto do claoudinary aby mogl wysylac zdj do chmury
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudynary = new Cloudinary(account);
        }

        [HttpPost]
        [System.Obsolete]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoForCreationDto PhotoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();

            var userFromRepo = await _repository.GetUser(userId);         // przepisujemy usera do wartosci

            var file = PhotoForCreationDto.File;            //sciagamy z klasy plik
            var uploadResult = new ImageUploadResult();     //tworzymy mmiejsce na plik lokalnie
            if (file.Length > 0)            // sprawdzamy czy plik zostal wyslany do API przez uzytkownika (czy jest wiekszy od zera)
            {
                using (var stream = file.OpenReadStream())          // jesli istnieje to cos z nim robimy
                {
                    var uploadParams = new ImageUploadParams(){         //wpisujemy parametry obrazka
                        File = new FileDescription(file.Name,stream),    //jego nazwa i sam obrazek
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")    //tranformacja wrzuconego przez uztk zdj(nie wazne jaki rozmiar) to go zmieni na 500 500
                    };

                    uploadResult= _cloudynary.Upload(uploadParams);
                }
            }

            PhotoForCreationDto.Url = uploadResult.Uri.ToString();      //dla dto jak przekazujemy url z claudinary 
            PhotoForCreationDto.publicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(PhotoForCreationDto);        //mapujemy zmiany na bazie z photoForCreationDto  na photo

            if (!userFromRepo.Photos.Any(p => p.IsMain))        // sprawdzamy czy nie ma glownego zdjecia, jesli nie ma to nadpisujemy te
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);             //zapisujemy w bazie

            if (await _repository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);              //mapujemy z photoforreturndto do photo
                return CreatedAtRoute("GetPhoto", new { id = photo.Id}, photoToReturn);     //zwara ze zostalo utworzone oraz informacje o nim get photo to jest abysmy dostali info o niej         
            }
                
            return BadRequest("Nie można dodać zdjęcia");
        }


        [HttpGet("{id}", Name = "GetPhoto")]    
        public async Task<IActionResult> GetPhoto(int id)
        {
            var PhotoFromRepo = await _repository.GetPhoto(id);  // z repozytorium pobieramy zdjecie przez metode

            var photoForReturn = _mapper.Map<PhotoForReturnDto>(PhotoFromRepo);         //mapujemy z PhotoForReturnDto na PhotoFromRepo

            return Ok(photoForReturn);
        }

    }
}