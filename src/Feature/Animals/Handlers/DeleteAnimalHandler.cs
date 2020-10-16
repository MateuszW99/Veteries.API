using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using FluentValidation;
using MediatR;

namespace Animals.Handlers
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

        private readonly IAnimalService _animalService;

        public DeleteAnimalHandler(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public async Task<DeleteAnimalResult> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return DeleteAnimalResult.RequestEmptyResult();
            }

            var userCanDeleteAnimal = await _animalService.UserOwnsAnimalAsync(request.Id, request.UserId);

            if (!userCanDeleteAnimal)
            {
                return DeleteAnimalResult.AccessDeniedResult();
            }

            var isDeleted = await _animalService.DeleteAnimalAsync(request.Id);

            return !isDeleted ? DeleteAnimalResult.BadRequestResult(request.Id) : DeleteAnimalResult.SuccessfulResult(request.Id);
        }

    }
}
