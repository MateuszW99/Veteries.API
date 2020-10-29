﻿using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using FluentValidation;
using MediatR;

namespace Animals.Handlers
{
    public class UpdateAnimalHandler : IRequestHandler<UpdateAnimalCommand, UpdateAnimalResult>
    {
        public class Validator : AbstractValidator<UpdateAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Animal.Id)
                    .GreaterThan(0);

                this.RuleFor(x => x.Animal.Age)
                    .GreaterThan(0);

                this.RuleFor(x => x.Animal.Species)
                    .NotNull()
                    .Length(2, 30);

                this.RuleFor(x => x.Animal.Name)
                    .NotNull()
                    .Length(2, 30);
            }
        }

        private readonly IAnimalService _animalService;

        public UpdateAnimalHandler(IAnimalService animalService)
        {
            _animalService = animalService;
        }


        public async Task<UpdateAnimalResult> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return UpdateAnimalResult.RequestEmptyResult();
            }

            if (!await _animalService.UserOwnsAnimalAsync(request.Animal.Id))
            {
                return UpdateAnimalResult.AccessDeniedResult();
            }

            var updated = await _animalService.UpdateAnimalAsync(request);
            
            return !updated ? UpdateAnimalResult.BadRequestResult() : UpdateAnimalResult.SuccessfulResult();
        }


    }
}
