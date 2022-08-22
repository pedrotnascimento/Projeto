using Repository.DataAccessLayer;


namespace Repository.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        MessageDAL Create(MessageDAL message);
        List<MessageDAL> Query(FilterMessageDAL filter);
    }
}
