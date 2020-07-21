using System;
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
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();  // sciagamy z bazy danych liste uztk i dolaczamy do nich zdjecia + (sortowanie) orderby

            users = users.Where(u => u.Id != userParams.UserId);        // (filtrowanie) - odffiltrujemy siebie
            users = users.Where(u => u.Gender == userParams.Gender);    // (filtrowanie) - filtrujemy tak aby wyswietlaly plec przeciwna (mezczyzna - kobieta)

            if(userParams.MinAge != 18 || userParams.MaxAge != 100)     // (filtrowanie) - sprawdzamy czy jest filtracja ze wzgledu na wiek
            {
            var minDate = DateTime.Today.AddYears(-userParams.MaxAge - 1);   // ustalenie roku tak aby nie wyswietlali sie ludzie ponizej 18 roku zycia albo jaki chcemy
            var MaxDate = DateTime.Today.AddYears(-userParams.MinAge);      // ustalenie roku tak aby nie wyswietlali sie ludzie powyzej 100 roku zycia albo jaki chcemy
            users = users.Where(u => u.DateOfBirth >= minDate && u.DateOfBirth <= MaxDate); // (filtrowanie) - filtrujemy ludzi ze wzgledu na wiek ktory chcemy zeby pokazywal
            }

            if(userParams.ZodiacSign != "wszystkie")
            {
                users = users.Where(u => u.ZodiacSign == userParams.ZodiacSign);
            }

           
            if (!string.IsNullOrEmpty(userParams.OrderBy))      // (sortowanie) sprawdzamy czy nie jest pusty jesli cos tam jest to
            {
                // (sortowanie) czy wpisany jest w  zmiennej orderby created, jesli tak to sortujemy wedlug utworzonego konta jesli nie to po ostatniej aktywnosci
                switch (userParams.OrderBy)                     
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }



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