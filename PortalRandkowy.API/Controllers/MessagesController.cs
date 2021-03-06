using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;
using PortalRandkowy.API.Helpers;
using PortalRandkowy.API.Models;
using System.Collections.Generic;

namespace PortalRandkowy.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]        // dla calej metody dajemy metode ktora po kazdym wywolaniu bedzie zapisywala godzine (Last Activity)
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public MessagesController(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


         [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId,int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();


            var messageFromRepo = await _repository.GetMessage(id);

            if(messageFromRepo == null)
                return NotFound("chuj ci w dupe");
            
            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery] MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            
            messageParams.UserId = userId;                      // przekazujemy id uzytk do dto
            var messegesFromRepo = await _repository.GetMessagesForUser(messageParams);         // pobieramy z repo paginacje
            var messagesToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messegesFromRepo);          // mapujemy inffo z messagesfromrepo do messagetoreturndto

            Response.AddPagination(messegesFromRepo.CurrentPage, messegesFromRepo.PageSize, messegesFromRepo.TotalCount, messegesFromRepo.TotalPages);      // dodajemy paginacje(stronnicowanie)
            foreach(var message in messagesToReturn)
            {
                message.MessageContainer = messageParams.MessageContainer;
            }

            return Ok(messagesToReturn);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            var messagesFromRepo = await _repository.GetMessagesThread(userId,recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {
            var sender = await _repository.GetUser(userId);
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            
            messageForCreationDto.SenderId = userId;        // wrzucamy do dto id usera
            var recipient = await _repository.GetUser(messageForCreationDto.RecipientId);       // poprzez id z dto pobieramy z repo usera

            if(recipient == null)           // sprawdzamy czy istnieje uztykownik do ktorego chcemy wyslac wiadomosc
                return BadRequest("Nie mozna znalezc uzytkownika");
            
            var message = _mapper.Map<Message>(messageForCreationDto);       // mapujemy wiadomosc z messageforcreationdto do message

            _repository.Add(message);       // dodajemy do bazy nasza wiadomosc

            

            if(await _repository.SaveAll())
            {                 // zapisujemy na bazie 
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute(nameof(GetMessage), new {id = message.Id, userId = message.SenderId} ,messageToReturn);   // wysylamy to do metody get message z wartosciami id i user id i zwracamy dla systemu messageToReturn
            }

            throw new Exception("Utworzenie wiadomosci nie powiodlo sie");   
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();
            
            var messageFromRepo = await _repository.GetMessage(id);

            if(messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;
            if(messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;
            if(messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repository.Delete(messageFromRepo);

            if(await _repository.SaveAll())
                return NoContent();
            throw new Exception("Błąd podczas usuwania wiadomosci");
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))       // sprawdzamy czy zgadza sie user(sprawdzamy czy id jest rowny)
                return Unauthorized();

            var message = await _repository.GetMessage(id);

            if(message.RecipientId != userId)
                return Unauthorized();
            
            message.IsRead = true;
            message.DateRead = DateTime.Now;

            if(await _repository.SaveAll())
                return NoContent();
            throw new Exception("Błąd");
        }

    }

}