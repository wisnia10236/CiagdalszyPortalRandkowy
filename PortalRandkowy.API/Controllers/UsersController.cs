using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalRandkowy.API.Data;
using PortalRandkowy.API.Dtos;

namespace PortalRandkowy.API.Controllers
{

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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users); //mapujemy z listy users do kolekcji UserForLIstDto aby wyswietlal nam liste userów

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDto>(user); //mapujemy z listy user do kolekcji UserForLIstDto aby wyswietlal nam dokladny opis usera

            return Ok(userToReturn);
        }
    }

}