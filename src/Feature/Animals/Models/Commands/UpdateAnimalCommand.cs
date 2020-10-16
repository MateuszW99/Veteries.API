using Animals.Models.Results;
using MediatR;

namespace Animals.Models.Commands
{
    public class UpdateAnimalCommand : AnimalCommand, IRequest<UpdateAnimalResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Species { get; set; }
    }
}
