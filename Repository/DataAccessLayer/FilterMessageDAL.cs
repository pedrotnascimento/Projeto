namespace Repository.DataAccessLayer
{
    public class FilterMessageDAL
    {
        public int ChatRoom { get; set; }
        public int OrderDirection { get; set; } = 1;
        public string OrderBy { get; set; } = "Timestamp";
        public int Quantity { get; set; } = 50;
    }
}