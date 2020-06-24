using Domain.Entities;

namespace Animal.Models.Results
{
    public class GetAnimalResult : IAnimalResult
    {
        public Pet Pet { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
