using System.Threading;
using System.Threading.Tasks;
using Animal.Abstractions;
using Animal.Models.Commands;
using Animal.Models.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace Animal.Handlers
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
            var animals = await _animalService.ReadAllAnimals();

            if (!animals.Any())
            {
                return GetAllAnimalsResult.NoAnimalFoundResult();
            }

            return GetAllAnimalsResult.AnimalListResult(animals);
        }
    }
}
