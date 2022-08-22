using Repository.DataAccessLayer;


namespace Repository.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        void Create(MessageDAL message);
        void Query(int chatRoomId);
    }
}
