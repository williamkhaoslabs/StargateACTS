using System.Net;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateApi.Business.Services;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands;

public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        public required string Name { get; set; }
        public required string Rank { get; set; }
        public required string DutyTitle { get; set; }
        public DateTime DutyStartDate { get; set; }
    }
 
    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;
 
        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }
 
        public Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadHttpRequestException("Name is required.");
 
            if (string.IsNullOrWhiteSpace(request.Rank))
                throw new BadHttpRequestException("Rank is required.");
 
            if (string.IsNullOrWhiteSpace(request.DutyTitle))
                throw new BadHttpRequestException("Duty Title is required.");
 
            if (request.DutyStartDate == default)
                throw new BadHttpRequestException("A valid Duty Start Date is required.");
            
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);
 
            if (person is null)
                throw new BadHttpRequestException($"Person '{request.Name}' not found.");
            
            var verifyNoPreviousDuty = _context.AstronautDuties.AsNoTracking().FirstOrDefault(
                z => z.PersonId == person.Id
                  && z.DutyTitle == request.DutyTitle
                  && z.DutyStartDate == request.DutyStartDate);
 
            if (verifyNoPreviousDuty is not null)
                throw new BadHttpRequestException(
                    $"A duty with title '{request.DutyTitle}' starting on " +
                    $"{request.DutyStartDate:yyyy-MM-dd} already exists for '{request.Name}'.");
 
            return Task.CompletedTask;
        }
    }
 
    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;
        private readonly ILogService _logService;
 
        public CreateAstronautDutyHandler(StargateContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }
 
        public async Task<CreateAstronautDutyResult> Handle(
            CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var person = await _context.People
                .FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken);
 
            if (person == null)
            {
                return new CreateAstronautDutyResult
                {
                    Success = false,
                    Message = $"Person '{request.Name}' not found.",
                    ResponseCode = (int)HttpStatusCode.BadRequest
                };
            }
            
            var astronautDetail = await _context.AstronautDetails
                .FirstOrDefaultAsync(d => d.PersonId == person.Id, cancellationToken);
 
            if (astronautDetail == null)
            {
                astronautDetail = new AstronautDetail
                {
                    PersonId = person.Id,
                    CurrentDutyTitle = request.DutyTitle,
                    CurrentRank = request.Rank,
                    CareerStartDate = request.DutyStartDate.Date
                };
                
                if (request.DutyTitle == "RETIRED")
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
 
                await _context.AstronautDetails.AddAsync(astronautDetail, cancellationToken);
            }
            else
            {
                astronautDetail.CurrentDutyTitle = request.DutyTitle;
                astronautDetail.CurrentRank = request.Rank;
 
                if (request.DutyTitle == "RETIRED")
                {
                    astronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
 
                _context.AstronautDetails.Update(astronautDetail);
            }
            
            var previousDuty = await _context.AstronautDuties
                .Where(d => d.PersonId == person.Id)
                .OrderByDescending(d => d.DutyStartDate)
                .FirstOrDefaultAsync(cancellationToken);
 
            if (previousDuty != null)
            {
                previousDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
                _context.AstronautDuties.Update(previousDuty);
            }
            
            var newAstronautDuty = new AstronautDuty
            {
                PersonId = person.Id,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = null
            };
 
            await _context.AstronautDuties.AddAsync(newAstronautDuty, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
 
            await _logService.LogSuccess(
                $"Astronaut duty assigned: {request.Name} — {request.Rank} / {request.DutyTitle}",
                nameof(CreateAstronautDutyHandler));
 
            return new CreateAstronautDutyResult
            {
                Id = newAstronautDuty.Id
            };
        }
    }
 
    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }