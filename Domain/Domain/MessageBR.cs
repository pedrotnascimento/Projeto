using Repository.Models;

namespace BusinessRule.Domain
{
    public class MessageBR
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public UserBR User { get; set; } 
        public int ChatRoomId { get; set; } 
        public ChatRoomBR ChatRoom { get; set; } 
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; }
    }
}
