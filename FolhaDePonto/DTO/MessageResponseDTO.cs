﻿namespace Application.DTO
{
    public class MessageResponseDTO
    {
        public int UserId { get; set; }
        public int ChatRoomId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; }
    }
}
