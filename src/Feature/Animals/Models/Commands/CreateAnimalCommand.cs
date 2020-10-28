using Animals.Models.Results;
using MediatR;

namespace Animals.Models.Commands
{
    public class CreateAnimalCommand : AnimalCommand, IRequest<CreateAnimalResult>
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
    }
}
