using System.Net;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests;

public class GetAstronautDutiesByNameHandlerTests
    {
        private readonly MockLogService _logService = new();
        
        [Fact]
        public async Task Handle_PersonWithDuties_ReturnsSortedDuties()
        {
            using var context = TestDbContextFactory.Create();
            var person = TestDbContextFactory.SeedPerson(context, "Multi Duty");

            var detail = new AstronautDetail
            {
                PersonId = person.Id,
                CurrentRank = "Colonel",
                CurrentDutyTitle = "Commander",
                CareerStartDate = new DateTime(2020, 1, 1)
            };
            context.AstronautDetails.Add(detail);

            context.AstronautDuties.AddRange(
                new AstronautDuty
                {
                    PersonId = person.Id,
                    Rank = "Lieutenant",
                    DutyTitle = "Pilot",
                    DutyStartDate = new DateTime(2020, 1, 1),
                    DutyEndDate = new DateTime(2021, 12, 31)
                },
                new AstronautDuty
                {
                    PersonId = person.Id,
                    Rank = "Major",
                    DutyTitle = "Mission Specialist",
                    DutyStartDate = new DateTime(2022, 1, 1),
                    DutyEndDate = new DateTime(2023, 12, 31)
                },
                new AstronautDuty
                {
                    PersonId = person.Id,
                    Rank = "Colonel",
                    DutyTitle = "Commander",
                    DutyStartDate = new DateTime(2024, 1, 1),
                    DutyEndDate = null
                }
            );
            context.SaveChanges();

            var handler = new GetAstronautDutiesByNameHandler(context, _logService);
            var request = new GetAstronautDutiesByName { Name = "Multi Duty" };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.True(result.Success);
            Assert.NotNull(result.Person);
            Assert.Equal("Multi Duty", result.Person.Name);
            Assert.Equal(3, result.AstronautDuties.Count);
            
            Assert.Equal(new DateTime(2024, 1, 1), result.AstronautDuties[0].DutyStartDate);
            Assert.Equal(new DateTime(2022, 1, 1), result.AstronautDuties[1].DutyStartDate);
            Assert.Equal(new DateTime(2020, 1, 1), result.AstronautDuties[2].DutyStartDate);
        }
        
        [Fact]
        public async Task Handle_UnknownName_Returns404()
        {
            using var context = TestDbContextFactory.Create();
            var handler = new GetAstronautDutiesByNameHandler(context, _logService);
            var request = new GetAstronautDutiesByName { Name = "Unknown" };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.False(result.Success);
            Assert.Equal((int)HttpStatusCode.NotFound, result.ResponseCode);
            Assert.Contains("Unknown", result.Message);
        }
        
        [Fact]
        public async Task Handle_PersonWithNoDuties_ReturnsEmptyList()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Civilian");
            var handler = new GetAstronautDutiesByNameHandler(context, _logService);
            var request = new GetAstronautDutiesByName { Name = "Civilian" };
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.True(result.Success);
            Assert.NotNull(result.Person);
            Assert.Empty(result.AstronautDuties);
        }
    }