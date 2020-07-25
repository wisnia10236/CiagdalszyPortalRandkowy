using System;

namespace PortalRandkowy.API.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }         // ? opcjonalne,(daje to nam ze narazie jest null ale jak ktos przeczyta wiadomosc to wtedy pojawia sie wartosci kiedy)
        public DateTime DateSend { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}