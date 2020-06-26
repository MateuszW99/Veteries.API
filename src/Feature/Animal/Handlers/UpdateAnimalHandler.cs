﻿using System.Threading;
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
            }
        }

        private readonly DomainDbContext _context;
        private readonly IAnimalService _animalService;

        public UpdateAnimalHandler(DomainDbContext context, IAnimalService animalService)
        {
            _context = context;
            _animalService = animalService;
        }


        public async Task<UpdateAnimalResult> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return UpdateAnimalResult.RequestEmptyResult();
            }

            var animalToUpdate = await _context.Pets.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (animalToUpdate == null)
            {
                return UpdateAnimalResult.BadRequestResult();
            }

            var mockAnimal = _animalService.CreateMockPet(request.Name, request.Age, request.Species);

            _animalService.UpdateAnimal(animalToUpdate, mockAnimal);

            _context.Pets.Update(animalToUpdate);
            await _context.SaveChangesAsync();

            return UpdateAnimalResult.SuccessfulResult(animalToUpdate);

        }
    }
}
