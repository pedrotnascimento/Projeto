using AutoMapper;
using BusinessRule.Domain;
using Common;
using Application.DTO;
using Repository.DataAccessLayer;

namespace Application.AutoMapper
{
    public class BRtoDALProfileMapper : Profile
    {

        public BRtoDALProfileMapper()
        {
            CreateMap<MessageBR, MessageDAL>().ReverseMap();
            CreateMap<ChatRoomBR, ChatRoomDAL>().ReverseMap();
            CreateMap<UserBR, UserDAL>().ReverseMap();
        }

    }
}
