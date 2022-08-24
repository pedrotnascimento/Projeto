using AutoMapper;
using Repository.DataAccessLayer;
using Repository.Models;

namespace Application.AutoMapper
{
    public class DAL_ModelProfileMapper : Profile
    {

        public DAL_ModelProfileMapper()
        {
            CreateMap<MessageDAL, Message>().ReverseMap();
            CreateMap<ChatRoomDAL, ChatRoom>().ReverseMap();
            CreateMap<UserDAL, ApplicationUser>().ReverseMap();
        }

    }
}
