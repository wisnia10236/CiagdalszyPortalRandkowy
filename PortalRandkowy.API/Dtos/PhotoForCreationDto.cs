using System;
using Microsoft.AspNetCore.Http;

namespace PortalRandkowy.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string  Url { get; set; }
        public IFormFile File { get; set; }   //plik
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string publicId { get; set; }    //zwracane z claudinary

        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;       //ustawienie odrazu date dodanie 
        }
    }
}