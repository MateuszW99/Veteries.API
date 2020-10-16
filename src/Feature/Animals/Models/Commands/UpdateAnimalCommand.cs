using Animals.Models.Results;
using Domain.Entities;
using MediatR;

namespace Animals.Models.Commands
{
    public class UpdateAnimalCommand : AnimalCommand, IRequest<UpdateAnimalResult>
    {
        public Animal Animal { get; set; }
    }
}
