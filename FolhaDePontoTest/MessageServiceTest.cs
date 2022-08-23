using Moq;
using Microsoft.Extensions.Logging;
using Repository.Repositories;
using Repository;
using BusinessRule.Interfaces;
using BusinessRule.Services;
using BusinessRule.Domain;
using Repository.Models;
using AutoMapper;
using Application.AutoMapper;
using Common;
using Microsoft.AspNetCore.Identity;

namespace FolhaDePontoTest
{
    public class MessageServiceTest
    {
        private readonly DateTime defaultDateTime = new DateTime(2022, 1, 3, 0, 0, 0);
        private ChatRoom chatRoomInstance;
        private ApplicationUser userInstance;
        IMessage messageService;
        AppDatabaseContext context;
        public MessageServiceTest()
        {
            context = CreateContext();
            messageService = MessageServiceArranje(context);
        }

        [Fact]
        public void ShouldCreateAMessage()
        {
            string text = "Message 1";
            MessageBR message = MessageArranje(text, chatRoomInstance.Id, userInstance.Id);

            MessageBR messageCreated = messageService.SendMessage(message);

            Assert.True(messageCreated.Payload == message.Payload);
            Assert.True(messageCreated.UserId == message.UserId);
            Assert.True(messageCreated.ChatRoomId == message.ChatRoomId);
            Assert.True(messageCreated.Id != 0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void ShouldGetAllMessages(int size)
        {
            ArranjeSeveralsMessages(size);
            IEnumerable<MessageBR>? result = messageService.GetMessages(chatRoomInstance.Id);
            Assert.True(result.Count() == size);
        }


        #region Arranje Auxiliar methods

        private AppDatabaseContext CreateContext()
        {
            SharedDatabaseFixture sharedDatabaseFixture = new SharedDatabaseFixture();
            var context = sharedDatabaseFixture.CreateContext();
            chatRoomInstance = new ChatRoom { Name = "ChatRoom1" };
            userInstance = new ApplicationUser { UserName = "Test User" };
            context.ChatRooms.Add(chatRoomInstance);
            context.Users.Add(userInstance);
            context.SaveChanges();
            return context;
        }

        private static IMessage MessageServiceArranje(AppDatabaseContext context)
        {
            Mock<ILogger<MessageService>> mockLogger = new Mock<ILogger<MessageService>>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DTO_BRProfileMapper));
                cfg.AddProfile(typeof(BR_DALProfileMapper));
                cfg.AddProfile(typeof(DAL_ModelProfileMapper));
            });

            var mapper = mapperConfiguration.CreateMapper();
            var messageRepository = new MessageRepository(context, mapper);
            IMessage folhaDePonto = new MessageService(mockLogger.Object, mapper, messageRepository);
            return folhaDePonto;
        }

        private MessageBR MessageArranje(string text, int chatId, string userId)
        {
            return new MessageBR
            {
                Payload = text,
                ChatRoomId = chatId,
                UserId = userId,
                Timestamp = DateTime.Now,
            };
        }

        private void ArranjeSeveralsMessages(int tam)
        {
            for (int i = 0; i < tam; i++)
            {
                Message cr1 = new Message { ChatRoomId = chatRoomInstance.Id, UserId = userInstance.Id, Payload = $"Message{i}", Timestamp = DateTime.Now };
                context.Messages.Add(cr1);
            }
            context.SaveChanges();
        }

        #endregion
    }
}