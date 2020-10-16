using Animals.Models.Results;
using Domain.Entities;
using MediatR;

namespace Animals.Models.Commands
{
    public class CreateAnimalCommand : AnimalCommand, IRequest<CreateAnimalResult>
    {
        public Animal Animal { get; set; }
    }
}
