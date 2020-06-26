using System.Threading;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Domain;

namespace Animal.Handlers
{
    public class GetAnimalByIdHandler : IRequestHandler<GetAnimalByIdCommand, GetAnimalResult>
    {
        public class Validator : AbstractValidator<GetAnimalByIdCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
            }
        }

        private readonly DomainDbContext _context;

        public GetAnimalByIdHandler(DomainDbContext context)
        {
            _context = context;
        }

        public async Task<GetAnimalResult> Handle(GetAnimalByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return GetAnimalResult.RequestEmptyResult();
            }

            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (animal == null)
            {
                return GetAnimalResult.AnimalNotFoundResult(request.Id);
            }

            return new GetAnimalResult()
            {
                Success = true,
                Animal = animal
            };
        }
    }
}
