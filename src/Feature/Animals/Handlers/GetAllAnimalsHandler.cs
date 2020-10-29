using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace Animals.Handlers
{
    public class GetAllAnimalsHandler : IRequestHandler<GetAllAnimalsCommand, GetAllAnimalsResult>
    {
        private readonly IAnimalService _animalService;

        public GetAllAnimalsHandler(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public async Task<GetAllAnimalsResult> Handle(GetAllAnimalsCommand request, CancellationToken cancellationToken)
        {
            var animals = await _animalService.GetAnimalsAsync();

            if (!animals.Any())
            {
                return GetAllAnimalsResult.NoAnimalFoundResult();
            }

            return GetAllAnimalsResult.SuccessfulResult(animals);
        }
    }
}
