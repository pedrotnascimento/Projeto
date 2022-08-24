using AutoMapper;
using Repository.DataAccessLayer;
using Repository.RepositoryInterfaces;
using Repository.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public MessageDAL Create(MessageDAL message)
        {
            var instance = mapper.Map<MessageDAL, Message>(message);
            this.context.Messages.Add(instance);
            context.SaveChanges();
            var ret = mapper.Map<MessageDAL>(instance);
            return ret;
        }

        public List<MessageDAL> Query(FilterMessageDAL filter)
        {
            var sortingsProperties = new Dictionary<string, string>()
            {
                {"timestamp", "Timestamp" },
            };
            IQueryable<Message>? messages = this.context.Messages
                .Where(x => x.ChatRoomId == filter.ChatRoom);    
            messages = messages.OrderByDescending(x => x.Timestamp);
            messages = messages.Take(filter.Quantity);
            messages = messages.Include(x => x.User);

            var messagesDAL = mapper.Map<List<MessageDAL>>(messages);
            return messagesDAL;
        }
    }
}
