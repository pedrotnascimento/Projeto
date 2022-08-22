using AutoMapper;
using BusinessRule.Domain;
using BusinessRule.Interfaces;
using Microsoft.Extensions.Logging;
using Repository.RepositoryInterfaces;

namespace BusinessRule.Services
{
    public class MessageService : IMessage
    {
        public readonly static int LIMIT_OF_MOMENT_PER_DAY = 4;
        public readonly static int HOURS_PER_DAY = 8;
        private ILogger<MessageService> logger;
        private IMapper mapper;
        private IMessageRepository timeMomentRepository;
        private IChatRoomRepository timeAllocationRepository;

        public MessageService(ILogger<MessageService> _logger,
            IMapper mapper,
            IMessageRepository timeMomentRepository,
            IChatRoomRepository timeAllocationRepository
            )
        {
            logger = _logger;
            this.mapper = mapper;
            this.timeMomentRepository = timeMomentRepository;
            this.timeAllocationRepository = timeAllocationRepository;
        }

        public List<MessageBR> GetMessages(int chatRoomId)
        {
            throw new NotImplementedException();
        }

        public MessageBR SendMessage(MessageBR dayMoment)
        {
            throw new NotImplementedException();
        }
    }
}
