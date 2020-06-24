using Domain.Entities;

namespace Animal.Models.Results
{
    public class CreateAnimalResult : IAnimalResult
    {
        public Pet Pet { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
