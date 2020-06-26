using System.Collections.Generic;
using System.Threading.Tasks;
using Animal.Models.Commands;

namespace Animal.Abstractions
{
    public interface IAnimalService
    {
        bool IsAnimalNull(Domain.Entities.Animal animal);

        Task<Domain.Entities.Animal> CreateAnimalAsync(CreateAnimalCommand request);
        Task<Domain.Entities.Animal> ReadAnimalAsync(int animalId);
        Task<bool> UpdateAnimalAsync(UpdateAnimalCommand request);
        Task<bool> DeleteAnimalAsync(int animalId);
        Task<IEnumerable<Domain.Entities.Animal>> ReadAllAnimals();
    }
}
