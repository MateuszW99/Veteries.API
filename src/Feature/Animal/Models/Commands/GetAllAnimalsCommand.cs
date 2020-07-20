using System.Runtime.CompilerServices;
using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class GetAllAnimalsCommand : AnimalCommand, IRequest<GetAllAnimalsResult>
    {
    }
}
