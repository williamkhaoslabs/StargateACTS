using System.Net;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;

namespace StargateAPI.Tests;

public class CreateAstronautDutyHandlerTests
    {
        private readonly MockLogService _logService = new();
        
        [Fact]
        public async Task Handle_FirstDuty_CreatesAstronautDetail()
        {
            using var context = TestDbContextFactory.Create();
            var person = TestDbContextFactory.SeedPerson(context, "Neil Armstrong");
            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Neil Armstrong",
                Rank = "Colonel",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2024, 3, 1)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Id);

            var detail = await context.AstronautDetails
                .FirstOrDefaultAsync(d => d.PersonId == person.Id);

            Assert.NotNull(detail);
            Assert.Equal("Colonel", detail.CurrentRank);
            Assert.Equal("Commander", detail.CurrentDutyTitle);
            Assert.Equal(new DateTime(2024, 3, 1), detail.CareerStartDate);
            Assert.Null(detail.CareerEndDate);
        }
        
        [Fact]
        public async Task Handle_SecondDuty_ClosesPreviousDutyEndDate()
        {
            using var context = TestDbContextFactory.Create();
            var (person, _, firstDuty) = TestDbContextFactory.SeedPersonWithDuty(
                context, "Buzz Aldrin", "Major", "Pilot",
                new DateTime(2024, 1, 1));

            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Buzz Aldrin",
                Rank = "Colonel",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2024, 6, 15)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.True(result.Success);
            
            var closedDuty = await context.AstronautDuties
                .FirstAsync(d => d.Id == firstDuty.Id);

            Assert.NotNull(closedDuty.DutyEndDate);
            Assert.Equal(new DateTime(2024, 6, 14), closedDuty.DutyEndDate);
        }
        
        [Fact]
        public async Task Handle_RetiredDuty_SetsCareerEndDate()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPersonWithDuty(
                context, "Sally Ride", "Captain", "Mission Specialist",
                new DateTime(2024, 1, 1));

            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Sally Ride",
                Rank = "Captain",
                DutyTitle = "RETIRED",
                DutyStartDate = new DateTime(2025, 1, 1)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.True(result.Success);

            var detail = await context.AstronautDetails
                .FirstAsync(d => d.Person.Name == "Sally Ride");

            Assert.Equal("RETIRED", detail.CurrentDutyTitle);
            Assert.NotNull(detail.CareerEndDate);
            Assert.Equal(new DateTime(2024, 12, 31), detail.CareerEndDate);
        }
        
        [Fact]
        public async Task Handle_RetiredAsFirstDuty_SetsCareerEndDate()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Retiree");
            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Retiree",
                Rank = "General",
                DutyTitle = "RETIRED",
                DutyStartDate = new DateTime(2025, 3, 1)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.True(result.Success);

            var detail = await context.AstronautDetails
                .FirstAsync(d => d.Person.Name == "Retiree");

            Assert.Equal(new DateTime(2025, 2, 28), detail.CareerEndDate);
        }
        
        [Fact]
        public async Task Handle_NewDuty_UpdatesAstronautDetailRankAndTitle()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPersonWithDuty(
                context, "Chris Hadfield", "Major", "Pilot",
                new DateTime(2024, 1, 1));

            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Chris Hadfield",
                Rank = "Colonel",
                DutyTitle = "Station Commander",
                DutyStartDate = new DateTime(2024, 7, 1)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.True(result.Success);

            var detail = await context.AstronautDetails
                .FirstAsync(d => d.Person.Name == "Chris Hadfield");

            Assert.Equal("Colonel", detail.CurrentRank);
            Assert.Equal("Station Commander", detail.CurrentDutyTitle);
        }
        
        [Fact]
        public async Task Handle_PersonNotFound_ReturnsFailure()
        {
            using var context = TestDbContextFactory.Create();
            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Nobody",
                Rank = "Private",
                DutyTitle = "Guard",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.ResponseCode);
            Assert.Contains("Nobody", result.Message);
        }
        
		[Fact]
        public async Task Handle_NewDuty_HasNullEndDate()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Test Person");
            var handler = new CreateAstronautDutyHandler(context, _logService);

            var request = new CreateAstronautDuty
            {
                Name = "Test Person",
                Rank = "Lieutenant",
                DutyTitle = "Pilot",
                DutyStartDate = new DateTime(2024, 5, 1)
            };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            var duty = await context.AstronautDuties
                .OrderByDescending(d => d.Id)
                .FirstAsync();

            Assert.Null(duty.DutyEndDate);
        }
        
        [Fact]
        public async Task Handle_Success_LogsToLogService()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Log Test");
            var logService = new MockLogService();
            var handler = new CreateAstronautDutyHandler(context, logService);

            var request = new CreateAstronautDuty
            {
                Name = "Log Test",
                Rank = "Captain",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            await handler.Handle(request, CancellationToken.None);
            
            Assert.Single(logService.SuccessLogs);
            Assert.Contains("Log Test", logService.SuccessLogs[0].Message);
            Assert.Equal("CreateAstronautDutyHandler", logService.SuccessLogs[0].Source);
        }
    }