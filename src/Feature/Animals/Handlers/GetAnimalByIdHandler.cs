using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using FluentValidation;
using MediatR;

namespace Animals.Handlers
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

            if (request.UserId != animal.UserId)
            {
                return GetAnimalResult.AccessDeniedResult();
            }

            if (_animalService.IsAnimalNull(animal))
            {
                return GetAnimalResult.AnimalNotFoundResult(request.Id);
            }

            return GetAnimalResult.AnimalFoundResult(animal);
        }
    }
}
