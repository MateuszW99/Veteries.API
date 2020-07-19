using Domain.Entities;

namespace Animal.Models.Results
{
    public class UpdateAnimalResult : AnimalResult
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

        public new static UpdateAnimalResult SuccessfulResult()
        {
            return new UpdateAnimalResult()
            {
                Success = true,
                Message = new string("Successfully updated animal")
            };
        }

        public static UpdateAnimalResult AccessDeniedResult()
        {
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = new string("You can't update this animal data")
            };
        }
    }
}
