using System.Threading;
using System.Threading.Tasks;
using Animal.Abstractions;
using Animal.Models.Commands;
using Animal.Models.Results;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Domain;

namespace Animal.Handlers
{
    public class DeleteAnimalHandler : IRequestHandler<DeleteAnimalCommand, DeleteAnimalResult>
    {
        public class Validator : AbstractValidator<DeleteAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id)
                    .GreaterThanOrEqualTo(0);
            }
        }

        private readonly DomainDbContext _context;
        private readonly IAnimalService _animalService;

        public DeleteAnimalHandler(DomainDbContext context, IAnimalService animalService)
        {
            _context = context;
            _animalService = animalService;
        }

        public async Task<DeleteAnimalResult> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return DeleteAnimalResult.RequestEmptyResult();
            }

            var animalToBeRemoved = await _context.Animals.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (_animalService.IsAnimalNull(animalToBeRemoved))
            {
                return DeleteAnimalResult.BadRequestResult(request.Id);
            }

            _context.Animals.Remove(animalToBeRemoved);
            await _context.SaveChangesAsync();

            return DeleteAnimalResult.SuccessfulResult(request.Id);
        }
    }
}
