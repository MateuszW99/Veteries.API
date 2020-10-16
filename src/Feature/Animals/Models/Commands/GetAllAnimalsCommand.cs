using Animals.Models.Results;
using MediatR;

namespace Animals.Models.Commands
{
    public class GetAllAnimalsCommand : AnimalCommand, IRequest<GetAllAnimalsResult>
    {
    }
}
