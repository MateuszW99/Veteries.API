using Domain.Entities;

namespace Animal.Abstractions
{
    public interface IAnimalService
    {
        bool IsAnimalNull(Pet pet);
    }
}
