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
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDto PhotoForCreationDto)
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

            PhotoForCreationDto.Url = uploadResult.Url.ToString();     //dla dto jak przekazujemy url z claudinary 
            PhotoForCreationDto.publicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(PhotoForCreationDto);        //mapujemy zmiany na bazie z photoForCreationDto  na photo

            if (!userFromRepo.Photos.Any(p => p.IsMain))        // sprawdzamy czy nie ma glownego zdjecia, jesli nie ma to nadpisujemy te
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);             //zapisujemy w bazie

            if (await _repository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);              //mapujemy z photoforreturndto do photo
                return CreatedAtRoute(nameof(GetPhoto), new { userId, id = photo.Id},photoToReturn);     //przesyla dla routa "getphoto" aby utworzyc nowy rekord w bazie , dla photo to return
                // wysylajac dla nmiego userid i photoid       
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

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            
            var user = await _repository.GetUser(userId);           //pobieramy z repo uzytkownika

            if(!user.Photos.Any(p => p.Id == id))            //sprawdzamy czy zdjecie jest
                return Unauthorized();
            
            var PhotoFromRepo = await _repository.GetPhoto(id);     //pobieramy z repo zdj

            if(PhotoFromRepo.IsMain)            //sprawdzamy czy zdj jest juz glowne
                return BadRequest("To jest głowne zdjecie");
            
            var currentMainPhoto = await _repository.GetMainPhotoForUser(userId);           //pobieramy aktualne zdj glowne

            currentMainPhoto.IsMain = false;                //zmieniamy go ze nie jest glownym

            PhotoFromRepo.IsMain = true;                    //zdj ktore chcemy aby bylo glownym zmieniamy ze 
            
            if (await _repository.SaveAll())
                return NoContent();

            return BadRequest("Nie mozna ustawic zdj jako glownego");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId , int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            
            var user = await _repository.GetUser(userId);           //pobieramy z repo uzytkownika

            if(!user.Photos.Any(p => p.Id == id))            //sprawdzamy czy jakiekolwiek zdjecia jest
                return Unauthorized();
            
            var PhotoFromRepo = await _repository.GetPhoto(id);     //pobieramy z repo zdj

            if(PhotoFromRepo.IsMain)            //sprawdzamy czy zdj jest juz glowne
                return BadRequest("Nie można usunąć zdjęcia głównego");

            
            if (PhotoFromRepo.public_id != null)            // jesli ma public id
            {
                var deleteParams = new DeletionParams(PhotoFromRepo.public_id);     // przekazujemy dla cloudinary jakie zdj ma usunąć przekazując jej public id 
                var result = _cloudynary.Destroy(deleteParams);    // usuwanie w cloudinary

                if(result.Result == "ok")           // jesli jest git to usuwamy z repo
                    _repository.Delete(PhotoFromRepo);
            }

            if (PhotoFromRepo.public_id == null)  
                _repository.Delete(PhotoFromRepo);
            

            if (await _repository.SaveAll())            // zapisujemy zmiany jesli ok to wyswietlamy ok jesli nie to badrequest
                return Ok();

            return BadRequest("Nie udało się usunąć zdjęcia");    

            
        }


    }
}