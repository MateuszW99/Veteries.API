using System.Threading;
using System.Threading.Tasks;
using Animal.Abstractions;
using Animal.Models.Commands;
using Animal.Models.Results;
using FluentValidation;
using MediatR;

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

        private readonly IAnimalService _animalService;

        public GetAnimalByIdHandler(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public async Task<GetAnimalResult> Handle(GetAnimalByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return GetAnimalResult.RequestEmptyResult();
            }

            var animal = await _animalService.ReadAnimalAsync(request.Id);

            if (_animalService.IsAnimalNull(animal))
            {
                return GetAnimalResult.AnimalNotFoundResult(request.Id);
            }

            return GetAnimalResult.AnimalFoundResult(animal);
        }
    }
}
