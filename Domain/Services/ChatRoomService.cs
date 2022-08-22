using AutoMapper;
using BusinessRule.Domain;
using BusinessRule.Interfaces;
using Microsoft.Extensions.Logging;
using Repository.DataAccessLayer;
using Repository.RepositoryInterfaces;

namespace BusinessRule.Services
{
    public class ChatRoomService : IChatRoom
    {
        public readonly static int LIMIT_OF_MOMENT_PER_DAY = 4;
        public readonly static int HOURS_PER_DAY = 8;
        private ILogger<ChatRoomService> logger;
        private IMapper mapper;
        private IChatRoomRepository chatRoomRepository;

        public ChatRoomService(ILogger<ChatRoomService> _logger,
            IMapper mapper,
            IChatRoomRepository chatRoomRepository
            )
        {
            logger = _logger;
            this.mapper = mapper;
            this.chatRoomRepository = chatRoomRepository;
        }

        public ChatRoomBR CreateChatRoom(ChatRoomBR chatRoom)
        {
            ChatRoomDAL chatRoomDal = mapper.Map<ChatRoomDAL>(chatRoom);
            var chatRoomCreatedDal = this.chatRoomRepository.CreateChatRoom(chatRoomDal);
            var chatRoomCreated = mapper.Map<ChatRoomBR>(chatRoomCreatedDal);
            return chatRoomCreated;
        }

        public List<ChatRoomBR> GetChatRooms()
        {
            var chatRooms = this.chatRoomRepository.GetChatRooms();
            var chatRoomsBR = mapper.Map<List<ChatRoomBR>>(chatRooms);
            return chatRoomsBR;
        }
    }
}
