using AutoMapper;
using BusinessRule.Domain;
using Common;
using Application.DTO;
using Repository.DataAccessLayer;

namespace Application.AutoMapper
{
    public class BR_DALProfileMapper : Profile
    {

        public BR_DALProfileMapper()
        {
            CreateMap<MessageBR, MessageDAL>().ReverseMap();
            CreateMap<ChatRoomBR, ChatRoomDAL>().ReverseMap();
            CreateMap<UserBR, UserDAL>().ReverseMap();
        }

    }
}
