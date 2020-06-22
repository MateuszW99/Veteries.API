﻿using System.Threading;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.Domain;

namespace Animal.Handlers
{
    public class CreateAnimalHandler : IRequestHandler<CreateAnimalCommand, CreateAnimalResult>
    {
        public class Validator : AbstractValidator<CreateAnimalCommand>
        {
            public Validator()
            {
                //this.RuleFor(x => x.Pet)
                //    .NotNull()
                //    .NotEmpty();

                //this.RuleFor(x => x.Pet.Name)
                //    .NotNull()
                //    .Length(2, 20);

                //this.RuleFor(x => x.Pet.Age)
                //    .GreaterThan(0);

                //this.RuleFor(x => x.Pet.Species)
                //    .NotNull()
                //    .Length(3, 20);

                this.RuleFor(x => x.Name)
                    .NotNull()
                    .Length(2, 20);

                this.RuleFor(x => x.Species)
                    .NotNull()
                    .Length(2, 20);


                this.RuleFor(x => x.Age)
                    .GreaterThan(0);
            }
        }

        private readonly DomainDbContext _context;

        public CreateAnimalHandler(DomainDbContext context)
        {
            _context = context;
        }


        public async Task<CreateAnimalResult> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new CreateAnimalResult()
                {
                    Success = false,
                    Pet = null
                };
            }

            var pet = new Pet()
            {
                Name = request.Name,
                Species = request.Species,
                Age = request.Age
            };

            await _context.AddAsync(pet);
            await _context.SaveChangesAsync();

            return new CreateAnimalResult
            {
                Success = true,
                Pet =  pet
            };
        }
    }
}