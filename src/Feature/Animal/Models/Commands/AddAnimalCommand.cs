using Animal.Models.Results;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Animal.Models.Commands
{
    public class AddAnimalCommand : IRequest<AddAnimalResult>
    {
        public Pet Pet { get; set; }
    }

    public class Validator : AbstractValidator<AddAnimalCommand>
    {
        public Validator()
        {
            this.RuleFor(x => x.Pet)
                .NotNull()
                .NotEmpty();

            this.RuleFor(x => x.Pet.Name)
                .NotNull()
                .Length(2, 20);

            this.RuleFor(x => x.Pet.Age)
                .GreaterThan(0);

            this.RuleFor(x => x.Pet.Species)
                .NotNull()
                .Length(3, 20);
        }
    }
}
