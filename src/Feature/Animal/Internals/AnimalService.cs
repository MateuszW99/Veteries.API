using Animal.Abstractions;

namespace Animal.Internals
{
    public class AnimalService : IAnimalService
    {
        public bool IsAnimalNull(Domain.Entities.Animal animal)
        {
            return animal == null ? true : false;
        }

        public Domain.Entities.Animal CreateMockPet(string name, int age, string species)
        {
            return new Domain.Entities.Animal()
            {
                Name = name,
                Age = age,
                Species = species
            };
        }

        public void UpdateAnimal(Domain.Entities.Animal destination, Domain.Entities.Animal source)
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
