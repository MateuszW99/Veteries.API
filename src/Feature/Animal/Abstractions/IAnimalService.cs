using Domain.Entities;

namespace Animal.Abstractions
{
    public interface IAnimalService
    {
        bool IsAnimalNull(Pet pet);
        Pet CreateMockPet(string name, int age, string species);
        void UpdateAnimal(Pet destination, Pet source);
    }
}
