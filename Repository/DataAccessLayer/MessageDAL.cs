
namespace Repository.DataAccessLayer
{
    public class MessageDAL
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UserDAL User { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoomDAL ChatRoom { get; set; }
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; }
    }
}
