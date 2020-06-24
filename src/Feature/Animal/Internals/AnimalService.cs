using Animal.Abstractions;
using Domain.Entities;

namespace Animal.Internals
{
    public class AnimalService : IAnimalService
    {
        public bool IsAnimalNull(Pet pet)
        {
            return pet == null ? true : false;
        }
    }
}
