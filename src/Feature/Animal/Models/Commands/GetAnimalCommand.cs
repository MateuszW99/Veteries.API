using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class GetAnimalCommand : AnimalCommand, IRequest<GetAnimalResult>
    {
        public int Id { get; set; }
    }
}
