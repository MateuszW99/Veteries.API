using Domain.Entities;

namespace Animal.Models.Results
{
    public class UpdateAnimalResult : IAnimalResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Domain.Entities.Animal Animal { get; set; }

        public static UpdateAnimalResult RequestEmptyResult()
        { 
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = new string("Empty request"),
                Animal = null
            };
            
        }

        public static UpdateAnimalResult BadRequestResult()
        {
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = new string($"Error when updating animal"),
                Animal = null
            };
        }

        public static UpdateAnimalResult SuccessfulResult(Domain.Entities.Animal animal)
        {
            return new UpdateAnimalResult()
            {
                Success = true,
                Message = new string("Successfully updated animal"),
                Animal = animal
            };
        }
    }
}
