namespace Repository.Models
{
    public class ChatRoom
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
