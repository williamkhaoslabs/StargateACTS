using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;

namespace StargateAPI.Tests;

public class CreatePersonPreProcessorTests
{
    [Fact]
    public async Task Process_DuplicateName_ThrowsBadRequest()
    {
        using var context = TestDbContextFactory.Create();
        TestDbContextFactory.SeedPerson(context, "John Doe");
        var preprocessor = new CreatePersonPreProcessor(context);

        var request = new CreatePerson { Name = "John Doe" };
        
        var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
            () => preprocessor.Process(request, CancellationToken.None));

        Assert.Contains("already exists", ex.Message);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Process_EmptyOrWhitespaceName_ThrowsBadRequest(string? name)
    {
        using var context = TestDbContextFactory.Create();
        var preprocessor = new CreatePersonPreProcessor(context);

        var request = new CreatePerson { Name = name! };
        
        var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
            () => preprocessor.Process(request, CancellationToken.None));

        Assert.Contains("Name", ex.Message);
    }
    
    [Fact]
    public async Task Process_TrimsWhitespace()
    {
        using var context = TestDbContextFactory.Create();
        TestDbContextFactory.SeedPerson(context, "John Doe");
        var preprocessor = new CreatePersonPreProcessor(context);

        var request = new CreatePerson { Name = "  John Doe  " };
        
        var ex = await Assert.ThrowsAsync<BadHttpRequestException>(
            () => preprocessor.Process(request, CancellationToken.None));

        Assert.Contains("already exists", ex.Message);
    }
    
    [Fact]
    public async Task Process_UniqueName_Succeeds()
    {
        using var context = TestDbContextFactory.Create();
        var preprocessor = new CreatePersonPreProcessor(context);

        var request = new CreatePerson { Name = "Brand New Person" };
        
        await preprocessor.Process(request, CancellationToken.None);
    }
}

public class CreatePersonHandlerTests
{
    private readonly MockLogService _logService = new();
    
    [Fact]
    public async Task Handle_ValidName_CreatesPersonAndReturnsId()
    {
        using var context = TestDbContextFactory.Create();
        var handler = new CreatePersonHandler(context, _logService);

        var request = new CreatePerson { Name = "New Astronaut" };
        
        var result = await handler.Handle(request, CancellationToken.None);
        
        Assert.True(result.Success);
        Assert.True(result.Id > 0);

        var person = await context.People.FirstOrDefaultAsync(p => p.Name == "New Astronaut");
        Assert.NotNull(person);
        Assert.Equal(result.Id, person.Id);
    }
    
    [Fact]
    public async Task Handle_Success_LogsToLogService()
    {
        using var context = TestDbContextFactory.Create();
        var logService = new MockLogService();
        var handler = new CreatePersonHandler(context, logService);

        var request = new CreatePerson { Name = "Log Person" };
        
        await handler.Handle(request, CancellationToken.None);
        
        Assert.Single(logService.SuccessLogs);
        Assert.Contains("Log Person", logService.SuccessLogs[0].Message);
        Assert.Equal("CreatePersonHandler", logService.SuccessLogs[0].Source);
    }
    
    [Fact]
    public async Task Handle_NewPerson_HasNoAstronautDetail()
    {
        using var context = TestDbContextFactory.Create();
        var handler = new CreatePersonHandler(context, _logService);

        var request = new CreatePerson { Name = "Civilian" };
        
        var result = await handler.Handle(request, CancellationToken.None);
        
        var detail = await context.AstronautDetails
            .FirstOrDefaultAsync(d => d.PersonId == result.Id);

        Assert.Null(detail);
    }
}

