using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class UpdateAnimalCommand : AnimalCommand, IRequest<UpdateAnimalResult>
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Species { get; set; }
    }
}
