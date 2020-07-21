namespace PortalRandkowy.API.Models
{
    public class Like
    {
        public int UserLikesId { get; set; }    // uzytkownik lubi
        public int UserIsLikedId { get; set; }    // uzytkownik jest lubiany

        public User UserLikes { get; set; }
        public User UserIsLiked { get; set; }
    }
}