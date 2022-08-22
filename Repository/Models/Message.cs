namespace Repository.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ChatRoomId{ get; set; }
        public ChatRoom ChatRoom { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
