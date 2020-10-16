using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class CreateAnimalCommand : AnimalCommand, IRequest<CreateAnimalResult>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Species { get; set; }
    }
}
