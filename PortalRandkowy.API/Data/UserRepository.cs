using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Models;
using System.Linq;
using PortalRandkowy.API.Helpers;

namespace PortalRandkowy.API.Data
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos);  // sciagamy z bazy danych liste uztk i dolaczamy do nich zdjecia
            return await PagedList<User>.CreateListAsync(users,userParams.PageNumber,userParams.PageSize);      
            // zwaramy liste  dla klasy stronicowania i tworzymy lisste przez stworzona statyczna metode i przekazyjemy dla niej parametry
        }

         public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userid)
        {
            var photoMain = await _context.Photos.Where(u => u.UserId == userid).FirstOrDefaultAsync(p => p.IsMain);
            
            return photoMain;
        }
    }
}