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

namespace FolhaDePontoTest
{
    public class FolhaDePontoServiceTest
    {
        private readonly DateTime defaultDateTime = new DateTime(2022, 1, 3, 0, 0, 0);
        IChatRoom folhaDePonto;
        UserBR testUser;
        AppDatabaseContext context;
        public FolhaDePontoServiceTest()
        {
            context = CreateContext();
            folhaDePonto = FolhaDePontoServiceArranje(context);
            CreateTestUser(context);
        }

        [Theory]
        [InlineData("2022-01-03T12:00:00")]
        [InlineData("2022-01-03T00:00:00")]
        [InlineData("2022-01-20T23:59:59")]
        public void ShouldRegisterATimeMoment(string dateTimeStr)
        {
            MessageBR timeMoment = TimeMomentArranje(dateTimeStr);
            var hourPart = timeMoment.Timestamp.ToLongTimeString();

            IEnumerable<MessageBR> result = folhaDePonto.ClockIn(timeMoment);

            Assert.NotNull(result.FirstOrDefault(x => x.Timestamp.ToLongTimeString() == hourPart));
        }


        [Theory]
        [InlineData("2022-01-03T09:00:00", "2022-01-03T12:00:00", "2022-01-03T13:00:00", "2022-01-03T17:00:00")]
        [InlineData("2022-01-03T00:00:00", "2022-01-03T00:01:00", "2022-01-03T23:58:00", "2022-01-03T23:59:00")]
        [InlineData("2022-01-03T00:00:00", "2022-01-03T22:58:00", "2022-01-03T23:58:00", "2022-01-03T23:59:00")]
        public void ShouldRegisterAllTimeMoments(string start, string lunchStart, string lunchEnd, string end)
        {
            MessageBR startMoment = TimeMomentArranje(start);
            folhaDePonto.ClockIn(startMoment);
            int userId = startMoment.UserId;

            MessageBR startLunchMoment = CreateMomenWithUser(lunchStart, userId);
            folhaDePonto.ClockIn(startLunchMoment);

            MessageBR endLunchMoment = CreateMomenWithUser(lunchEnd, userId);
            folhaDePonto.ClockIn(endLunchMoment);

            MessageBR endMoment = CreateMomenWithUser(end, userId);
            IEnumerable<MessageBR>? result = folhaDePonto.ClockIn(endMoment);

            Assert.True(result.Count() == 4);
        }

        [Theory]
        [InlineData("2022-01-03T12:00:00")]
        [InlineData("2022-01-03T00:00:00")]
        [InlineData("2022-01-03T23:00:00")]
        public void ShouldFailWhenRegisterATimeMomentThatAlreadyExists(string dateTimeStr)
        {
            MessageBR timeMoment = TimeMomentArranje(dateTimeStr);
            folhaDePonto.ClockIn(timeMoment);

            var exceptionCall = () => folhaDePonto.ClockIn(timeMoment);

            Assert.Throws<HourAlreadyExistsException>(exceptionCall);
        }

        [Theory]
        [InlineData("2022-01-03T12:00:00")]
        [InlineData("2022-01-03T00:00:00")]
        [InlineData("2022-01-03T16:59:59")]
        public void ShouldFailWhenExceedsTimeMomentRegister(string dateTimeStr)
        {
            MessageBR timeMomentExtra = SeveralTimeMomentArranje(dateTimeStr, folhaDePonto);

            var exceptionCall = () => folhaDePonto.ClockIn(timeMomentExtra);

            Assert.Throws<HoursLimitExceptions>(exceptionCall);
        }

        [Theory]
        [InlineData("2022-01-01T12:00:00")]
        [InlineData("2022-01-02T00:00:00")]
        public void ShouldFailWhenWeekend(string dateTimeStr)
        {
            MessageBR timeMoment = TimeMomentArranje(dateTimeStr);

            var exceptionCall = () => folhaDePonto.ClockIn(timeMoment);

            Assert.Throws<WeekendExceptions>(exceptionCall);
        }

        [Theory]
        [InlineData("2022-01-03T08:00:00", "2022-01-03T12:00:00", "2022-01-03T12:01:00")]
        [InlineData("2022-01-03T13:11:00", "2022-01-03T13:12:00", "2022-01-03T14:11:00")]
        public void ShouldFailWhenLunchLessThan1Hour(string startJourney, string lunchStart, string lunchEnd)
        {

            MessageBR startJourneyTimeMoment = TimeMomentArranje(startJourney);
            folhaDePonto.ClockIn(startJourneyTimeMoment);
            MessageBR lunchStartTimeMoment = new MessageBR
            {
                UserId = startJourneyTimeMoment.UserId,
                Timestamp = DateTime.Parse(lunchStart)
            };
            folhaDePonto.ClockIn(lunchStartTimeMoment);

            MessageBR lunchEndTimeMoment = new MessageBR
            {
                UserId = lunchStartTimeMoment.UserId,
                Timestamp = DateTime.Parse(lunchEnd)
            };
            var exceptionCall = () => folhaDePonto.ClockIn(lunchEndTimeMoment);

            Assert.Throws<LunchTimeLimitExceptions>(exceptionCall);
        }

        [Theory]
        [InlineData(0.1, 4)]
        [InlineData(4, 4)]
        [InlineData(8, 8)]
        [InlineData(20, 20)]
        public void ShouldCreateAllocationHoursInProject(double hoursToAllocate, double hoursWorked)
        {
            BuildTimeMomentFullJourney(hoursWorked);

            var timeDuration = new DateTime(1, 1, 1, 0, 0, 0).AddHours(hoursToAllocate);
            var timeAllocation = new ChatRoomBR
            {
                Date = defaultDateTime,
                TimeDuration = timeDuration,
                ProjectName = "Any Project Name",
                UserId = testUser.Id
            };
            var result = folhaDePonto.AllocateHoursInProject(timeAllocation);
            Assert.True(result.TimeDuration == timeDuration);
        }

        [Theory]
        [InlineData(5, 4)]
        [InlineData(4.1, 4)]
        [InlineData(22, 20)]
        public void ShouldFailWhenHoursToAllocateGreaterThanHoursWorked(double hoursToAllocate, double hoursWorked)
        {
            BuildTimeMomentFullJourney(hoursWorked);

            var timeDuration = new DateTime(1, 1, 1, 0, 0, 0).AddHours(hoursToAllocate);
            var timeAllocation = new ChatRoomBR
            {
                Date = defaultDateTime,
                TimeDuration = timeDuration,
                ProjectName = "Any Project Name",
                UserId = testUser.Id
            };

            var exceptionCall = () => folhaDePonto.AllocateHoursInProject(timeAllocation);
            Assert.Throws<TimeAllocationLimitException>(exceptionCall);
        }

        [Theory]
        [InlineData(8, 8, 0, 0)]
        public void ShouldReturnReport(double hoursToAllocate, double hoursWorkedInDay, double exceedHours, double debtHours)
        {
            int HOURS_WORKED_IN_DAY = 8;
            int CLOCK_IN_IN_DAY = 2;
            BuildTimeMomentEntireMonthGivenAnHoursByDay(hoursWorkedInDay);
            BuildTimeAllocation(hoursToAllocate);

            var report = new ReportBR
            {
                Month = defaultDateTime.Date,
                User = new UserBR { Id = testUser.Id, Name = testUser.Name }
            };

            var workDays = DateHelper.WorkDaysInAMonth(defaultDateTime);
            ReportDataBR? reportData = folhaDePonto.GetReport(report);

            var timeSpanWorkedHours = new TimeSpan(reportData.WorkedTime.Ticks);
            var timeSpanDebtHours = new TimeSpan(reportData.DebtTime.Ticks);
            var timeSpanExceedHours = new TimeSpan(reportData.ExceededWorkedTime.Ticks);
            double totalWorkedHours = workDays * hoursWorkedInDay;

            Assert.NotNull(reportData);
            Assert.True(reportData.TimeAllocations.FirstOrDefault().TimeDuration.Hour == hoursToAllocate);
            Assert.True(reportData.TimeMoments.Count() == workDays * CLOCK_IN_IN_DAY);
            Assert.True(timeSpanWorkedHours.TotalHours == totalWorkedHours);
            Assert.True(timeSpanDebtHours.TotalHours == debtHours);

            Assert.True(timeSpanExceedHours.TotalHours == exceedHours);

        }

        #region Arranje Auxiliar methods

        private AppDatabaseContext CreateContext()
        {
            SharedDatabaseFixture sharedDatabaseFixture = new SharedDatabaseFixture();
            var context = sharedDatabaseFixture.CreateContext();
            return context;
        }

        private void CreateTestUser(AppDatabaseContext context)
        {
            testUser = new UserBR { Name = "teste" };
        }

        private MessageBR CreateMomenWithUser(string dateTimeStr, int userId)
        {
            return new MessageBR
            {
                UserId = userId,
                Timestamp = DateTime.Parse(dateTimeStr),
            };
        }

        private MessageBR SeveralTimeMomentArranje(string dateTimeStr, IChatRoom folhaDePonto)
        {
            MessageBR timeMoment = TimeMomentArranje(dateTimeStr);

            folhaDePonto.ClockIn(timeMoment);
            for (var i = 1; i < ChatRoomService.LIMIT_OF_MOMENT_PER_DAY; i++)
            {
                MessageBR timeMomentRepeated = new MessageBR
                {
                    Timestamp = timeMoment.Timestamp.AddHours(i),
                    UserId = timeMoment.UserId
                };
                folhaDePonto.ClockIn(timeMomentRepeated);
            }

            MessageBR timeMomentExtra = new MessageBR
            {
                Timestamp = timeMoment.Timestamp.AddHours(ChatRoomService.LIMIT_OF_MOMENT_PER_DAY),
                UserId = timeMoment.UserId
            };
            timeMomentExtra.UserId = timeMoment.UserId;
            return timeMomentExtra;
        }

        private static IChatRoom FolhaDePontoServiceArranje(AppDatabaseContext context)
        {
            Mock<ILogger<ChatRoomService>> mockLogger = new Mock<ILogger<ChatRoomService>>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DTOtoBRProfileMapper));
                cfg.AddProfile(typeof(BRtoDALProfileMapper));
                cfg.AddProfile(typeof(DALtoTableProfileMapper));
            });

            var mapper = mapperConfiguration.CreateMapper();
            var timeMomentRepository = new MessageRepository(context, mapper);
            var timeAllocationRepository = new ChatRoomRepository(context, mapper);
            IChatRoom folhaDePonto = new ChatRoomService(mockLogger.Object, mapper, timeMomentRepository, timeAllocationRepository);
            return folhaDePonto;
        }

        private MessageBR TimeMomentArranje(string dateTimeStr)
        {
            MessageBR timeMoment = new MessageBR();
            DateTime dateTime = DateTime.Parse(dateTimeStr);
            timeMoment.Timestamp = dateTime;
            timeMoment.UserId = this.testUser.Id;
            return timeMoment;
        }

        private void BuildTimeMomentFullJourney(double hoursWorked)
        {
            var list = new List<Message>();
            Message timeMomentStart = new Message { Timestamp = defaultDateTime, UserId = testUser.Id };
            list.Add(timeMomentStart);
            list.Add(new Message { Timestamp = defaultDateTime.AddHours(hoursWorked), UserId = testUser.Id });

            context.TimeMoments.AddRange(list);
            context.SaveChanges();
        }

        private void BuildTimeMomentEntireMonthGivenAnHoursByDay(double hoursWorkedByDay)
        {
            var list = new List<Message>();
            var workDaysInAMonth = DateHelper.WorkDaysInAMonth(defaultDateTime.Date);
            for (int i = 0; i < workDaysInAMonth; i++)
            {
                Message timeMomentStart = new Message { Timestamp = defaultDateTime.AddDays(i), UserId = testUser.Id };
                list.Add(timeMomentStart);
                list.Add(new Message { Timestamp = defaultDateTime.AddDays(i).AddHours(hoursWorkedByDay), UserId = testUser.Id });
            }

            context.TimeMoments.AddRange(list);
            context.SaveChanges();
        }

        private void BuildTimeAllocation(double hoursToAllocate)
        {
            ChatRoom timeAllocation = new ChatRoom
            {
                Date = defaultDateTime,
                ProjectName = "Any project",
                TimeDuration = new DateTime(1, 1, 1).AddHours(hoursToAllocate),
                Users = testUser.Id
            };

            context.TimeAllocations.Add(timeAllocation);
            context.SaveChanges();
        }

        #endregion
    }
}