using AutoMapper;
using Repository.DataAccessLayer;
using Repository.RepositoryInterfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDatabaseContext context;
        private readonly IMapper mapper;

        public MessageRepository(AppDatabaseContext context, AutoMapper.IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Create(MessageDAL timeMoment)
        {
            var instance = mapper.Map<MessageDAL, Message>(timeMoment);
            this.context.Messages.Add(instance);
            context.SaveChanges();
        }

        public void Query(int chatRoomId)
        {
            throw new NotImplementedException();
        }
    }
}
