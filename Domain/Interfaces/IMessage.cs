

using BusinessRule.Domain;

namespace BusinessRule.Interfaces
{
    public interface IMessage
    {
        MessageBR SendMessage(MessageBR message);
        List<MessageBR> GetMessages(int chatRoomId);
    }
}
