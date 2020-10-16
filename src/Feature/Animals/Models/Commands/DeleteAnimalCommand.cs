using Animals.Models.Results;
using MediatR;

namespace Animals.Models.Commands
{
    public class DeleteAnimalCommand : AnimalCommand, IRequest<DeleteAnimalResult>
    {
        public int Id { get; set; }
    }
}
