namespace Animal.Abstractions
{
    public interface IAnimalService
    {
        bool IsAnimalNull(Domain.Entities.Animal animal);
        Domain.Entities.Animal CreateMockPet(string name, int age, string species);
        void UpdateAnimal(Domain.Entities.Animal destination, Domain.Entities.Animal source);
    }
}
