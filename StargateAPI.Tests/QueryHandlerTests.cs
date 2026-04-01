using System.Net;
using StargateAPI.Business.Queries;

namespace StargateAPI.Tests;

public class GetPersonByNameHandlerTests
{
    private readonly MockLogService _logService = new();

    [Fact]
    public async Task Handle_ExistingPerson_ReturnsPersonWithDetail()
    {
        using var context = TestDbContextFactory.Create();
        var (person, _, _) = TestDbContextFactory.SeedPersonWithDuty(
            context, "John Glenn", "Colonel", "Pilot",
            new DateTime(2024, 1, 1));

        var handler = new GetPersonByNameHandler(context, _logService);
        var request = new GetPersonByName { Name = "John Glenn" };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.Success);
        Assert.NotNull(result.Person);
        Assert.Equal("John Glenn", result.Person.Name);
        Assert.Equal("Colonel", result.Person.CurrentRank);
        Assert.Equal("Pilot", result.Person.CurrentDutyTitle);
    }

    [Fact]
    public async Task Handle_UnknownName_Returns404()
    {
        using var context = TestDbContextFactory.Create();
        var handler = new GetPersonByNameHandler(context, _logService);
        var request = new GetPersonByName { Name = "No One" };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal((int)HttpStatusCode.NotFound, result.ResponseCode);
        Assert.Null(result.Person);
    }

    [Fact]
    public async Task Handle_PersonWithoutDetail_ReturnsNullDetailFields()
    {
        using var context = TestDbContextFactory.Create();
        TestDbContextFactory.SeedPerson(context, "Civilian");
        var handler = new GetPersonByNameHandler(context, _logService);
        var request = new GetPersonByName { Name = "Civilian" };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.Success);
        Assert.NotNull(result.Person);
        Assert.Equal("Civilian", result.Person.Name);
        Assert.Null(result.Person.CurrentRank);
        Assert.Null(result.Person.CurrentDutyTitle);
        Assert.Null(result.Person.CareerStartDate);
    }
}

public class GetPeopleHandlerTests
{
    private readonly MockLogService _logService = new();

    [Fact]
    public async Task Handle_MultiplePeople_ReturnsAll()
    {
        using var context = TestDbContextFactory.Create();
        
        var baseline = context.People.Count();

        TestDbContextFactory.SeedPersonWithDuty(
            context, "Person One", "Captain", "Pilot",
            new DateTime(2024, 1, 1));
        TestDbContextFactory.SeedPerson(context, "Person Two");

        var handler = new GetPeopleHandler(context, _logService);
        var request = new GetPeople();

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(baseline + 2, result.People.Count);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessEvenIfEmpty()
    {
        using var context = TestDbContextFactory.Create();
        var handler = new GetPeopleHandler(context, _logService);
        var request = new GetPeople();

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.True(result.Success);
        Assert.NotNull(result.People);
    }

    [Fact]
    public async Task Handle_MixedPeople_CorrectDetailFields()
    {
        using var context = TestDbContextFactory.Create();
        TestDbContextFactory.SeedPersonWithDuty(
            context, "Astronaut", "Major", "Commander",
            new DateTime(2024, 1, 1));
        TestDbContextFactory.SeedPerson(context, "Civilian");

        var handler = new GetPeopleHandler(context, _logService);
        var request = new GetPeople();

        var result = await handler.Handle(request, CancellationToken.None);

        var astronaut = result.People.First(p => p.Name == "Astronaut");
        var civilian = result.People.First(p => p.Name == "Civilian");

        Assert.Equal("Major", astronaut.CurrentRank);
        Assert.Equal("Commander", astronaut.CurrentDutyTitle);
        Assert.Null(civilian.CurrentRank);
        Assert.Null(civilian.CurrentDutyTitle);
    }
}