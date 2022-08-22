using AutoMapper;
using Repository.DataAccessLayer;
using Repository.RepositoryInterfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly AppDatabaseContext context;
        private readonly IMapper mapper;

        public ChatRoomRepository(AppDatabaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public ChatRoomDAL CreateChatRoom(ChatRoomDAL timeAlocation)
        {
            var instance = mapper.Map<ChatRoomDAL, ChatRoom>(timeAlocation);
            this.context.ChatRooms.Add(instance);
            context.SaveChanges();
            var instanceDal = mapper.Map<ChatRoomDAL>(instance);
            return instanceDal;
        }

        public List<ChatRoomDAL> GetChatRooms()
        {
            var chatRooms = this.context.ChatRooms.ToList();
            var chatRoomsDal = mapper.Map<List<ChatRoomDAL>>(chatRooms);
            return chatRoomsDal;
        }
    }
}
