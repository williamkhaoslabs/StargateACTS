using Microsoft.AspNetCore.Http;
using StargateAPI.Business.Commands;

namespace StargateAPI.Tests;

public class CreateAstronautDutyPreProcessorTests
    {
        [Fact]
        public async Task Process_MissingPerson_ThrowsBadRequest()
        {
            using var context = TestDbContextFactory.Create();
            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = "Ghost Person",
                Rank = "Captain",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
                () => preprocessor.Process(request, CancellationToken.None));

            Assert.Contains("Ghost Person", ex.Message);
            Assert.Contains("not found", ex.Message);
        }
        
        [Fact]
        public async Task Process_DuplicateDuty_ThrowsBadRequest()
        {
            using var context = TestDbContextFactory.Create();
            var (person, _, _) = TestDbContextFactory.SeedPersonWithDuty(
                context, "John Doe", "Major", "Pilot",
                new DateTime(2024, 1, 1));

            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = "John Doe",
                Rank = "Major",
                DutyTitle = "Pilot",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
                () => preprocessor.Process(request, CancellationToken.None));

            Assert.Contains("already exists", ex.Message);
        }
        
        [Fact]
        public async Task Process_SameDutyDifferentPerson_Succeeds()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPersonWithDuty(
                context, "Person A", "Major", "Pilot",
                new DateTime(2024, 1, 1));

            TestDbContextFactory.SeedPerson(context, "Person B");

            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = "Person B",
                Rank = "Major",
                DutyTitle = "Pilot",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            await preprocessor.Process(request, CancellationToken.None);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task Process_EmptyName_ThrowsBadRequest(string? name)
        {
            using var context = TestDbContextFactory.Create();
            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = name!,
                Rank = "Captain",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
                () => preprocessor.Process(request, CancellationToken.None));

            Assert.Contains("Name", ex.Message);
        }
        
        [Fact]
        public async Task Process_EmptyRank_ThrowsBadRequest()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Rank Test");
            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = "Rank Test",
                Rank = "",
                DutyTitle = "Commander",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
                () => preprocessor.Process(request, CancellationToken.None));

            Assert.Contains("Rank", ex.Message);
        }
        
        [Fact]
        public async Task Process_EmptyDutyTitle_ThrowsBadRequest()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Title Test");
            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = "Title Test",
                Rank = "Captain",
                DutyTitle = "",
                DutyStartDate = new DateTime(2024, 1, 1)
            };
            
            var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
                () => preprocessor.Process(request, CancellationToken.None));

            Assert.Contains("Duty Title", ex.Message);
        }
        
        [Fact]
        public async Task Process_DefaultStartDate_ThrowsBadRequest()
        {
            using var context = TestDbContextFactory.Create();
            TestDbContextFactory.SeedPerson(context, "Date Test");
            var preprocessor = new CreateAstronautDutyPreProcessor(context);

            var request = new CreateAstronautDuty
            {
                Name = "Date Test",
                Rank = "Captain",
                DutyTitle = "Commander",
                DutyStartDate = default 
            };
            
            var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
                () => preprocessor.Process(request, CancellationToken.None));

            Assert.Contains("Duty Start Date", ex.Message);
        }
    }