using System;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }         // ? opcjonalne,(daje to nam ze narazie jest null ale jak ktos przeczyta wiadomosc to wtedy pojawia sie wartosci kiedy)
        public DateTime DateSend { get; set; }
        public string MessageContainer { get; set; }
    }
}