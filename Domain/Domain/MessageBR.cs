namespace BusinessRule.Domain
{
    public class MessageBR
    {
        
        public int UserId { get; set; } 
        public int ChatRoomId { get; set; } 
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; }
    }
}
