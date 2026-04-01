using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateApi.Business.Services;
using StargateAPI.Controllers;

public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public required string Name { get; set; } = string.Empty;
    }
 
    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;
        private readonly ILogService _logService;
 
        public GetAstronautDutiesByNameHandler(StargateContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }
 
        public async Task<GetAstronautDutiesByNameResult> Handle(
            GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {
            var result = new GetAstronautDutiesByNameResult();
 
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
 
                await _logService.LogSuccess(
                    $"Person not found for duty lookup: {request.Name}",
                    nameof(GetAstronautDutiesByNameHandler));
 
                return result;
            }
 
            result.Person = person;
 
            var duties = await _context.AstronautDuties
                .AsNoTracking()
                .Where(d => d.PersonId == person.PersonId)
                .OrderByDescending(d => d.DutyStartDate)
                .ToListAsync(cancellationToken);
 
            result.AstronautDuties = duties;
 
            await _logService.LogSuccess(
                $"Retrieved {duties.Count} duties for: {request.Name}",
                nameof(GetAstronautDutiesByNameHandler));
 
            return result;
        }
    }
 
    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new();
    }