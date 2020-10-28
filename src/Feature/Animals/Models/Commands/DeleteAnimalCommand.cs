using Animals.Models.Results;
using MediatR;

namespace Animals.Models.Commands
{
    public class DeleteAnimalCommand : AnimalCommand, IRequest<DeleteAnimalResult>
    {
        public int AnimalId { get; set; }
    }
}
