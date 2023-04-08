using Application.Authorization;
using BusinessRule.Domain;
using BusinessRule.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public class MessageHub:Hub
    {

        private IHubContext<MessageHub> context;
        private IMessage messageService;
        private IIdentityManager identityManager;

        public MessageHub(
            IHubContext<MessageHub> context,
            IMessage message,
            IIdentityManager identityManager) : base()
        {
            this.context = context;
            this.messageService = message;
            this.identityManager = identityManager;
        }

        public async Task MessageSent(MessageBR message)
        {
            var messageObjResult = messageService.SendMessage(message);
            var user = identityManager.GetUser(messageObjResult.UserId);
            messageObjResult.User = new UserBR {  Id = user.Id, UserName = user.UserName};

            context.Clients.All.SendAsync("messageReceived", messageObjResult);
        }
    }
}
