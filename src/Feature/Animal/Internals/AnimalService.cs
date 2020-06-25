using Animal.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Http.Features;

namespace Animal.Internals
{
    public class AnimalService : IAnimalService
    {
        public bool IsAnimalNull(Pet pet)
        {
            return pet == null ? true : false;
        }

        public Pet CreateMockPet(string name, int age, string species)
        {
            return new Pet()
            {
                Name = name,
                Age = age,
                Species = species
            };
        }

        public void UpdateAnimal(Pet destination, Pet source)
        {
            if (source.Name != null)
            {
                destination.Name = source.Name;
            }

            if (source.Species != null)
            {
                destination.Species = source.Species;
            }

            if (source.Age >= 0)
            {
                destination.Age = source.Age;
            }
        }

    }
}
