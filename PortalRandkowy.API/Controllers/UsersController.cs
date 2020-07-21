using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Helpers;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]        // dla calej metody dajemy metode ktora po kazdym wywolaniu bedzie zapisywala godzine (Last Activity)
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)     // fromquery daje nam to ze mozemy na URL (users?Number=2) dawac wartosci 
                                                                                         //dla pol zeby je szytwno wrzucal np ze strona ma byc 2
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);         // (filtrowanie) - pobieranmy id z tokena
            var userFromRepo = await _repo.GetUser(currentUserId);              // (filtrowanie) - wyszukujemy go

            userParams.UserId = currentUserId;          // (filtrowanie) - przypisujemy go do params

            if (string.IsNullOrEmpty(userParams.Gender))        //  (filtrowanie) - sprawdzamy czy ma gender ustawiony
            {
                // (filtrowanie) - przypisujemy mu wyszukanie takie ze jesli jest mezczyzna to wyswietla sie kobiety, jesli nie to kobiety wyswietlaja sie bo jest kobieta
                userParams.Gender = userFromRepo.Gender == "mezczyzna" ? "kobieta" : "mezczyzna";     
                 
            }


            var users = await _repo.GetUsers(userParams);


            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users); //mapujemy z listy users do kolekcji UserForLIstDto aby wyswietlal nam liste useróww

            Response.AddPagination(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);   // dodanie do naglowka odpowiedzi , informacje o paginacji

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user); //mapujemy z listy user do kolekcji UserForLIstDto aby wyswietlal nam dokladny opis usera

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int userId, UserForUpdateDto userForUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);         // przepisujemy usera do wartosci

            _mapper.Map(userForUpdateDto,userFromRepo);   //mapujemy zmiany dla userfromrepo(czyli dla usera w bazie danych)

            if(await _repo.SaveAll())  {         //zapisujemy do bazy
                return NoContent();
            }
            
            throw new Exception($"Aktualizacja uzytkownika o id: {userId} nie powiodla sie przy zapisywaniu do bazy danych");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            
            var like = await _repo.GetLike(id,recipientId);     // pobieramy like

            if(like != null)                // sprawdzamy czy juz nie jest takiego polubienia
                return BadRequest("Już lubisz tego uztkownika");
            
            if(await _repo.GetUser(recipientId) == null)    // sprawdzamy czy istnieje taki uzytk
                return NotFound();

            like = new Like         // tworzymy like
            {
                UserLikesId = id,
                UserIsLikedId = recipientId
            };

            _repo.Add<Like>(like);      // dodajemy to do bazy

            if(await _repo.SaveAll())
                return Ok();
            
            return BadRequest("Nie mozna polubic uzytkownika");
        }   


    }
}
