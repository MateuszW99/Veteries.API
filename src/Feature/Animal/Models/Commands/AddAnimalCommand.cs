using System.Security.Cryptography.X509Certificates;
using Domain.Entities;
using FluentValidation;

namespace Animal.Models.Commands
{
    public class AddAnimalCommand
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
        }
    }
}
