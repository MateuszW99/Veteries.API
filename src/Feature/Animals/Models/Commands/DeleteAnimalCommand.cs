using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class DeleteAnimalCommand : AnimalCommand, IRequest<DeleteAnimalResult>
    {
        public int Id { get; set; }
    }
}
