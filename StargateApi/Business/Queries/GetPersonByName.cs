using System.Net;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries;

public class GetPersonByName : IRequest<GetPersonByNameResult>
{
    public required string Name { get; set; } = string.Empty;
}

public class GetPersonByNameHandler : IRequestHandler<GetPersonByName, GetPersonByNameResult>
{
    private readonly StargateContext _context;

    public GetPersonByNameHandler(StargateContext context)
    {
        _context = context;
    }

    public async Task<GetPersonByNameResult> Handle(GetPersonByName request, CancellationToken cancellationToken)
    {
        var result = new GetPersonByNameResult();

        var person = await _context.People
            .AsNoTracking()
            .Include(p => p.AstronautDetail)
            .Where(p => p.Name == request.Name)
            .Select(p => new PersonAstronaut
            {
                PersonId = p.Id,
                Name = p.Name,
                CurrentRank = p.AstronautDetail != null ? p.AstronautDetail.CurrentRank : null,
                CurrentDutyTitle = p.AstronautDetail != null ? p.AstronautDetail.CurrentDutyTitle : null,
                CareerStartDate = p.AstronautDetail != null ? p.AstronautDetail.CareerStartDate : null,
                CareerEndDate = p.AstronautDetail != null ? p.AstronautDetail.CareerEndDate : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (person == null)
        {
            result.Success = false;
            result.Message = $"Person '{request.Name}' not found.";
            result.ResponseCode = (int)HttpStatusCode.NotFound;
            return result;
        }

        result.Person = person;
        return result;
    }
}

public class GetPersonByNameResult : BaseResponse
{
    public PersonAstronaut? Person { get; set; }
}