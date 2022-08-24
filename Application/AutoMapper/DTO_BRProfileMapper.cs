using AutoMapper;
using BusinessRule.Domain;
using Common;
using Application.DTO;

namespace Application.AutoMapper
{
    public class DTO_BRProfileMapper : Profile
    {

        public DTO_BRProfileMapper()
        {
            CreateMap<MessageCreateDTO, MessageBR>();
            CreateMap<ChatRoomCreateDTO, ChatRoomBR>();
            CreateMap<ChatRoomBR, ChatRoomResponseDTO>();
            CreateMap<MessageBR, MessageResponseDTO>();
            CreateMap<UserBR, UserDTO>().ReverseMap();
        }

    }
}
