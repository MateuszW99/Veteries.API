using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class GetAllAnimalsCommand : IRequest<GetAllAnimalsResult>
    {
    }
}
