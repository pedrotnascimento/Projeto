using Application.Authorization;
using AutoMapper;
using BusinessRule.Domain;
using BusinessRule.Interfaces;
using Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Repository.Models;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly ILogger<MessageController> _logger;
        private readonly IMessage messageService;
        private readonly IMapper mapper;
        private readonly IAuthentication authentication;

        public MessageController(ILogger<MessageController> logger,
            IMapper mapper,
            IMessage messageService,
            IAuthentication authentication
            )
        {
            _logger = logger;
            this.messageService = messageService;
            this.mapper = mapper;
            this.authentication = authentication;
        }

        [Authorize("Bearer")]
        [HttpPost("create")]
        public ObjectResult CreateMessage([FromBody] MessageCreateDTO message)
        {
            try
            {
                if (message != null && String.IsNullOrWhiteSpace(message.Payload))
                {
                    throw new Exception("Can't send empty message");
                }

                MessageBR data = mapper.Map<MessageCreateDTO, MessageBR>(message);
                string authHeader = HttpContext.Request.Headers["Authorization"];

                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                data.UserId = userId;
                MessageBR res = messageService.SendMessage(data);
                return new OkObjectResult(res) { StatusCode = 201 };
            }
            catch (Exception e)
            {
                return ControllerHelpers.ReturnError(e, 400, _logger, StatusCode);
            }
        }

        [Authorize("Bearer")]
        [HttpGet("list/{chatId}")]
        public ObjectResult ListMessage([FromRoute] int chatId)
        {
            try
            {
                List<MessageBR> result = messageService.GetMessages(chatId);
                return new OkObjectResult(result) { StatusCode = 201 };
            }
            catch (Exception e)
            {
                return ControllerHelpers.ReturnError(e, 400, _logger, StatusCode);
            }
        }
    }
}