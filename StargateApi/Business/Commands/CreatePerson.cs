using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateApi.Business.Services;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands;

public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }
 
    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;
 
        public CreatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
 
        public Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new BadHttpRequestException("Name is required and cannot be empty.");
 
            request.Name = request.Name.Trim();
 
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);
 
            if (person is not null)
                throw new BadHttpRequestException($"A person with the name '{request.Name}' already exists.");
 
            return Task.CompletedTask;
        }
    }
 
    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly ILogService _logService;
 
        public CreatePersonHandler(StargateContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }
 
        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {
            var newPerson = new Person()
            {
                Name = request.Name
            };
 
            await _context.People.AddAsync(newPerson, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
 
            await _logService.LogSuccess(
                $"Person created: {newPerson.Name} (Id: {newPerson.Id})",
                nameof(CreatePersonHandler));
 
            return new CreatePersonResult()
            {
                Id = newPerson.Id
            };
        }
    }
 
    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }