using Domain.Entities;

namespace Animal.Models.Results
{
    public class AddAnimalResult : IAnimalResult
    {
        public Pet Pet { get; set; }
        public bool Success { get; set; }
    }
}
