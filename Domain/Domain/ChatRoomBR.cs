namespace BusinessRule.Domain
{
    public class ChatRoomBR
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserBR> Users { get; set; }
        public List<MessageBR> Messages { get; set; }

    }
}
