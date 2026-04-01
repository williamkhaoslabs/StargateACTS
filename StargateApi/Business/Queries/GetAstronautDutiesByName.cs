using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
{
    public string Name { get; set; } = string.Empty;
}

public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
{
    private readonly StargateContext _context;

    public GetAstronautDutiesByNameHandler(StargateContext context)
    {
        _context = context;
    }

    public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
    {
        var person = await _context.People
            .Include(p => p.AstronautDetail)
            .Include(p => p.AstronautDuties)
            .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
        
        return new GetAstronautDutiesByNameResult
        {
            Person = new PersonAstronaut
            {
                PersonId = person.Id,
                Name = person.Name,
                CurrentRank = person.AstronautDetail?.CurrentRank,
                CurrentDutyTitle = person.AstronautDetail?.CurrentDutyTitle,
                CareerStartDate = person.AstronautDetail?.CareerStartDate,
                CareerEndDate = person.AstronautDetail?.CareerEndDate
            },
            AstronautDuties = person.AstronautDuties
                .OrderByDescending(d => d.DutyStartDate)
                .ToList()
        };

    }
}

public class GetAstronautDutiesByNameResult : BaseResponse
{
    public PersonAstronaut Person { get; set; }
    public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
}