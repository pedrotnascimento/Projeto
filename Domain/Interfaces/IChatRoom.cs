

using BusinessRule.Domain;

namespace BusinessRule.Interfaces
{
    public interface IChatRoom
    {
        ChatRoomBR CreateChatRoom(ChatRoomBR chatRoom);
        List<ChatRoomBR> GetChatRooms();
    }
}
