namespace Repository.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Payload { get; set; }
        public DateTime Timestamp { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ChatRoomId{ get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
