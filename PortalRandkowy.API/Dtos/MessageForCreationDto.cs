using System;

namespace PortalRandkowy.API.Dtos
{
    public class MessageForCreationDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime DateSend { get; set; }
        public string Content { get; set; }
        
        public MessageForCreationDto()
        {
            DateSend = DateTime.Now;
        }
    }
}