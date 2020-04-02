using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PortalRandkowy.API.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.Internal;

namespace PortalRandkowy.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)    //zainicjalizowanie db
        {
            _context = context;
        }

        public void SeedUsers()
        {
            if (!_context.Users.Any())  // sprawdzamy czy juz sa tam jakies dane jesli nie to wpisujemy testowe dane
            {



                var userData = File.ReadAllText("Data/UserSeedData.json"); //wczytanie wszystkich seedow z pliku
                var users = JsonConvert.DeserializeObject<List<User>>(userData); // konwertujemy dane na podstawie modelu User do listy a pozniej dla obiektu

                foreach (var user in users)         // kazdy z listy dodajemy do bazy i tworzymy passwordhash i passwordsalt
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHashSalt("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    _context.Users.Add(user);

                }

                _context.SaveChanges();     //zapisujemy baze
            }

        }

        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

    }
}