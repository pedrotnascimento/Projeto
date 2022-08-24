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

namespace ApplicationTest
{
    public class ChatRoomServiceTest
    {
        IChatRoom chatRoomService;
        AppDatabaseContext context;
        public ChatRoomServiceTest()
        {
            context = CreateContext();
            chatRoomService = ChatRoomServiceArranje(context);
        }

        [Fact]
        public void ShouldCreateAChatRoom()
        {
            string chatName = "ChatRoom 1";
            ChatRoomBR chatRoom = ChatRoomArranje(chatName);

            ChatRoomBR chatRoomCreated = chatRoomService.CreateChatRoom(chatRoom);

            Assert.True(chatRoomCreated.Name == chatRoom.Name);
            Assert.True(chatRoomCreated.Id != 0);
        }

        private ChatRoomBR ChatRoomArranje(string chatName)
        {
            return new ChatRoomBR
            {
                Name = chatName,
            };
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void ShouldGetAllChatRooms(int size)
        {
            ArranjeSeveralsChatRooms(size);
            IEnumerable<ChatRoomBR>? result = chatRoomService.GetChatRooms();
            Assert.True(result.Count() == size);
        }


        #region Arranje Auxiliar methods

        private AppDatabaseContext CreateContext()
        {
            SharedDatabaseFixture sharedDatabaseFixture = new SharedDatabaseFixture();
            var context = sharedDatabaseFixture.CreateContext();
            return context;
        }

        private static IChatRoom ChatRoomServiceArranje(AppDatabaseContext context)
        {
            Mock<ILogger<ChatRoomService>> mockLogger = new Mock<ILogger<ChatRoomService>>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DTO_BRProfileMapper));
                cfg.AddProfile(typeof(BR_DALProfileMapper));
                cfg.AddProfile(typeof(DAL_ModelProfileMapper));
            });

            var mapper = mapperConfiguration.CreateMapper();
            var chatRoomRepository = new ChatRoomRepository(context, mapper);
            IChatRoom Application = new ChatRoomService(mockLogger.Object, mapper, chatRoomRepository);
            return Application;
        }

        private void ArranjeSeveralsChatRooms(int tam)
        {

            for (int i = 0; i < tam; i++)
            {
                ChatRoom cr1 = new ChatRoom { Name = $"CR{i}" };
                context.ChatRooms.Add(cr1);
            }
            context.SaveChanges();
        }

        #endregion
    }
}