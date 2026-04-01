using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetPeople : IRequest<GetPeopleResult>
{

}

public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
{
    public readonly StargateContext _context;
    public GetPeopleHandler(StargateContext context)
    {
        _context = context;
    }
    public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
    {
        var people = await _context.People
            .Include(p => p.AstronautDetail)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var result = people.Select(p => new PersonAstronaut
        {
            PersonId = p.Id,
            Name = p.Name,
            CurrentRank = p.AstronautDetail?.CurrentRank,
            CurrentDutyTitle = p.AstronautDetail?.CurrentDutyTitle,
            CareerStartDate = p.AstronautDetail?.CareerStartDate,
            CareerEndDate = p.AstronautDetail?.CareerEndDate
        }).ToList();

        return new GetPeopleResult
        {
            People = result
        };
    }
}

public class GetPeopleResult : BaseResponse
{
    public List<PersonAstronaut> People { get; set; } = new List<PersonAstronaut> { };

}