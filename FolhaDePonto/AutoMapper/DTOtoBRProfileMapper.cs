using AutoMapper;
using BusinessRule.Domain;
using Common;
using Application.DTO;

namespace Application.AutoMapper
{
    public class DTOtoBRProfileMapper : Profile
    {

        public DTOtoBRProfileMapper()
        {
            CreateMap<MessageCreateDTO, MessageBR>();
            CreateMap<ChatRoomCreateDTO, ChatRoomBR>();
            CreateMap<ChatRoomBR, ChatRoomResponseDTO>();
        }

    }
}
