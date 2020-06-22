using Animal.Models.Results;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Animal.Models.Commands
{
    public class CreateAnimalCommand : IRequest<CreateAnimalResult>
    {
        //public Pet Pet { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Species { get; set; }
    }

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
}
