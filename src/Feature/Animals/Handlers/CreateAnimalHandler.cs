using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using FluentValidation;
using MediatR;

namespace Animals.Handlers
{
    public class CreateAnimalHandler : IRequestHandler<CreateAnimalCommand, CreateAnimalResult>
    {
        public class Validator : AbstractValidator<CreateAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Animal.Name)
                    .NotNull()
                    .Length(2, 20);

                this.RuleFor(x => x.Animal.Species)
                    .NotNull()
                    .Length(2, 20);


                this.RuleFor(x => x.Animal.Age)
                    .GreaterThan(0);
            }
        }

        private readonly IAnimalService _service;

        public CreateAnimalHandler(IAnimalService service)
        {
            _service = service;
        }


        public async Task<CreateAnimalResult> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return CreateAnimalResult.RequestEmptyResult();
            }

            var createdAnimal = await _service.CreateAnimalAsync(request);

            return CreateAnimalResult.SuccessfulResult(createdAnimal);
        }
    }
}
