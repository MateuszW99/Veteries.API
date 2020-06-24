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

    public static class CreateAnimalCommandExtension
    {
        public static bool IsNull(this CreateAnimalCommand command)
        {
            return command == null ? true : false;
        }
    }
}
