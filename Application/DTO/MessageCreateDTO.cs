﻿namespace Application.DTO
{
    public class MessageCreateDTO
    {
        public int ChatRoomId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; }
    }
}
