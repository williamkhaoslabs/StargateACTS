using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateApi.Business.Services;
using StargateAPI.Controllers;

public class GetPeople : IRequest<GetPeopleResult> { }
 
public class GetPeopleHandler : IRequestHandler<GetPeople, GetPeopleResult>
{
    private readonly StargateContext _context;
    private readonly ILogService _logService;
 
    public GetPeopleHandler(StargateContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }
 
    public async Task<GetPeopleResult> Handle(GetPeople request, CancellationToken cancellationToken)
    {
        var result = new GetPeopleResult();
 
        var people = await _context.People
            .AsNoTracking()
            .Include(p => p.AstronautDetail)
            .Select(p => new PersonAstronaut
            {
                PersonId = p.Id,
                Name = p.Name,
                CurrentRank = p.AstronautDetail != null ? p.AstronautDetail.CurrentRank : null,
                CurrentDutyTitle = p.AstronautDetail != null ? p.AstronautDetail.CurrentDutyTitle : null,
                CareerStartDate = p.AstronautDetail != null ? p.AstronautDetail.CareerStartDate : null,
                CareerEndDate = p.AstronautDetail != null ? p.AstronautDetail.CareerEndDate : null
            })
            .ToListAsync(cancellationToken);
 
        result.People = people;
 
        await _logService.LogSuccess(
            $"Retrieved {people.Count} people",
            nameof(GetPeopleHandler));
 
        return result;
    }
}
 
public class GetPeopleResult : BaseResponse
{
    public List<PersonAstronaut> People { get; set; } = new();
}