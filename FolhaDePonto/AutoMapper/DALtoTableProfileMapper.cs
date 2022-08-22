using AutoMapper;
using Repository.DataAccessLayer;
using Repository.Models;

namespace Application.AutoMapper
{
    public class DALtoTableProfileMapper : Profile
    {

        public DALtoTableProfileMapper()
        {
            CreateMap<MessageDAL, Message>().ReverseMap();
            CreateMap<ChatRoomDAL, ChatRoom>().ReverseMap();
        }

    }
}
