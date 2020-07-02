using System.Collections.Generic;

namespace Animal.Models.Results
{
    public class GetAllAnimalsResult : IAnimalResult
    {
        public IEnumerable<Domain.Entities.Animal> Animals;
        public bool Success { get; set; }
        public string Message { get; set; }

        public static GetAllAnimalsResult NoAnimalFoundResult()
        {
            return new GetAllAnimalsResult()
            {
                Success = false,
                Message = new string("No animal found"),
                Animals = null
            };
        }

        public static GetAllAnimalsResult AnimalListResult(IEnumerable<Domain.Entities.Animal> animals)
        {
            return new GetAllAnimalsResult()
            {
                Success = true,
                Animals = animals
            };
        }
    }
}
