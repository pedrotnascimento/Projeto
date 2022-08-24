using Application.Authorization;
using AutoMapper;
using BusinessRule.Domain;
using BusinessRule.Interfaces;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomController : ControllerBase
    {

        private readonly ILogger<ChatRoomController> _logger;
        private readonly IChatRoom chatRoomService;
        private readonly IMapper mapper;
        private readonly IAuthentication authentication;

        public ChatRoomController(ILogger<ChatRoomController> logger,
            IMapper mapper,
            IChatRoom chatRoomService,
            IAuthentication authentication
            )
        {
            _logger = logger;
            this.chatRoomService = chatRoomService;
            this.mapper = mapper;
            this.authentication = authentication;
        }

        [Authorize("Bearer")]
        [HttpPost("create")]
        public ObjectResult CreateChatRoom([FromBody] ChatRoomCreateDTO chatRoom)
        {
            try
            {
                ChatRoomBR data = mapper.Map<ChatRoomCreateDTO, ChatRoomBR>(chatRoom);
                ChatRoomBR res = chatRoomService.CreateChatRoom(data);
                return new OkObjectResult(res) { StatusCode = 201 };
            }
            catch (Exception e)
            {
                return ControllerHelpers.ReturnError(e, 400, _logger, StatusCode);
            }
        }

        [Authorize("Bearer")]
        [HttpGet("list")]
        public ObjectResult ListChatRoom()
        {
            try
            {
                List<ChatRoomBR> result = chatRoomService.GetChatRooms();
                return new OkObjectResult(result) { StatusCode = 201 };
            }
            catch (Exception e)
            {
                return ControllerHelpers.ReturnError(e, 400, _logger, StatusCode);
            }
        }
    }
}