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
        public readonly static int LIMIT_OF_MOMENT_PER_DAY = 4;
        public readonly static int HOURS_PER_DAY = 8;
        private ILogger<MessageService> logger;
        private IMapper mapper;
        private IMessageRepository messageRepository;
        private IChatRoomRepository timeAllocationRepository;

        public MessageService(ILogger<MessageService> _logger,
            IMapper mapper,
            IMessageRepository messageRepository,
            IChatRoomRepository timeAllocationRepository
            )
        {
            logger = _logger;
            this.mapper = mapper;
            this.messageRepository = messageRepository;
            this.timeAllocationRepository = timeAllocationRepository;
        }

        public List<MessageBR> GetMessages(int chatId)
        {
            FilterMessageDAL filterMessage = new FilterMessageDAL { ChatRoom = chatId };
            var messagesDAL = this.messageRepository.Query(filterMessage);
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
