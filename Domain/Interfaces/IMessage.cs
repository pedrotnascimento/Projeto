

using BusinessRule.Domain;

namespace BusinessRule.Interfaces
{
    public interface IMessage
    {
        MessageBR SendMessage(MessageBR dayMoment);
        List<MessageBR> GetMessages(int chatRoomId);
    }
}
