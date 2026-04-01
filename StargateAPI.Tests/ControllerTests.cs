using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Controllers;
using StargateAPI.Controllers;

namespace StargateAPI.Tests;

public class PersonControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetPeople_ReturnsOkWithResult()
    {
        var expected = new GetPeopleResult { Success = true };
        _mediator.Setup(m => m.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new PersonController(_mediator.Object);
        var result = await controller.GetPeople();

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, obj.StatusCode);
    }

    [Fact]
    public async Task GetPersonByName_ReturnsOkWithResult()
    {
        var expected = new GetPersonByNameResult { Success = true };
        _mediator.Setup(m => m.Send(It.IsAny<GetPersonByName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new PersonController(_mediator.Object);
        var result = await controller.GetPersonByName("John");

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, obj.StatusCode);
    }

    [Fact]
    public async Task GetPersonByName_NotFound_Returns404()
    {
        var expected = new GetPersonByNameResult
        {
            Success = false,
            ResponseCode = (int)HttpStatusCode.NotFound,
            Message = "Not found"
        };
        _mediator.Setup(m => m.Send(It.IsAny<GetPersonByName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new PersonController(_mediator.Object);
        var result = await controller.GetPersonByName("Nobody");

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, obj.StatusCode);
    }

    [Fact]
    public async Task CreatePerson_ReturnsOkWithId()
    {
        var expected = new CreatePersonResult { Success = true, Id = 1 };
        _mediator.Setup(m => m.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new PersonController(_mediator.Object);
        var result = await controller.CreatePerson("New Person");

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, obj.StatusCode);
    }
}

public class AstronautDutyControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetAstronautDutiesByName_ReturnsOkWithResult()
    {
        var expected = new GetAstronautDutiesByNameResult { Success = true };
        _mediator.Setup(m => m.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new AstronautDutyController(_mediator.Object);
        var result = await controller.GetAstronautDutiesByName("John");

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, obj.StatusCode);
    }

    [Fact]
    public async Task CreateAstronautDuty_ReturnsOkWithId()
    {
        var expected = new CreateAstronautDutyResult { Success = true, Id = 1 };
        _mediator.Setup(m => m.Send(It.IsAny<CreateAstronautDuty>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new AstronautDutyController(_mediator.Object);
        var request = new CreateAstronautDuty
        {
            Name = "Test",
            Rank = "Captain",
            DutyTitle = "Commander",
            DutyStartDate = new DateTime(2024, 1, 1)
        };

        var result = await controller.CreateAstronautDuty(request);

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(200, obj.StatusCode);
    }
}