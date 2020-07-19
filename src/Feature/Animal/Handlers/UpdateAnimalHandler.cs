using System.Threading;
using System.Threading.Tasks;
using Animal.Abstractions;
using Animal.Models.Commands;
using Animal.Models.Results;
using Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Animal.Handlers
{
    public class UpdateAnimalHandler : IRequestHandler<UpdateAnimalCommand, UpdateAnimalResult>
    {
        public class Validator : AbstractValidator<UpdateAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id)
                    .GreaterThan(0);

                this.RuleFor(x => x.Age)
                    .GreaterThan(0);

                this.RuleFor(x => x.Species)
                    .NotNull()
                    .Length(2, 30);

                this.RuleFor(x => x.Name)
                    .NotNull()
                    .Length(2, 30);
            }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAnimalService _animalService;

        public UpdateAnimalHandler(IAnimalService animalService, IHttpContextAccessor httpContextAccessor)
        {
            _animalService = animalService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<UpdateAnimalResult> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return UpdateAnimalResult.RequestEmptyResult();
            }

            var userCanUpdateAnimal = _animalService.UserOwnsAnimal(request.Id, _httpContextAccessor.HttpContext.GetUserId());

            if (!userCanUpdateAnimal.Result)
            {
                return UpdateAnimalResult.AccessDeniedResult();
            }

            var updated = await _animalService.UpdateAnimalAsync(request);
            
            return !updated ? UpdateAnimalResult.BadRequestResult() : UpdateAnimalResult.SuccessfulResult();
        }


    }
}
