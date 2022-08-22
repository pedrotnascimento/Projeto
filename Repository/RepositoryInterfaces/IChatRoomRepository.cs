using Repository.DataAccessLayer;
using Repository.Models;

namespace Repository.RepositoryInterfaces
{
    public interface IChatRoomRepository
    {
        ChatRoomDAL CreateChatRoom(ChatRoomDAL chatRoom);
        List<ChatRoomDAL> GetChatRooms();
    }
}
