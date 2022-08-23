using AutoMapper;
using BusinessRule.Domain;
using BusinessRule.Interfaces;
using Microsoft.Extensions.Logging;
using Repository.DataAccessLayer;
using Repository.RepositoryInterfaces;

namespace BusinessRule.Services
{
    public class MessageService : IMessage
    {
        private ILogger<MessageService> logger;
        private IMapper mapper;
        private IMessageRepository messageRepository;

        public MessageService(ILogger<MessageService> _logger,
            IMapper mapper,
            IMessageRepository messageRepository
            )
        {
            logger = _logger;
            this.mapper = mapper;
            this.messageRepository = messageRepository;
        }

        public List<MessageBR> GetMessages(int chatId)
        {
            FilterMessageDAL filterMessage = new FilterMessageDAL { ChatRoom = chatId };
            List<MessageDAL>? messagesDAL = this.messageRepository.Query(filterMessage);
            var messagesBR = mapper.Map<List<MessageBR>>(messagesDAL);
            return messagesBR;
        }

        public MessageBR SendMessage(MessageBR message)
        {
            var messageDal = mapper.Map<MessageDAL>(message);
            var messageDalCreated = messageRepository.Create(messageDal);
            var messageBRCreated = mapper.Map<MessageBR>(messageDalCreated);
            return messageBRCreated;
        }
    }
}
