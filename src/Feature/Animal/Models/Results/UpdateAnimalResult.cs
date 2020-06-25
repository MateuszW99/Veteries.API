using Domain.Entities;

namespace Animal.Models.Results
{
    public class UpdateAnimalResult : IAnimalResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Pet Pet { get; set; }

        public static UpdateAnimalResult RequestEmptyResult()
        { 
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = new string("Empty request"),
                Pet = null
            };
            
        }

        public static UpdateAnimalResult BadRequestResult()
        {
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = ($"Error when updating animal"),
                Pet = null
            };
        }

        public static UpdateAnimalResult SuccessfulResult(Pet pet)
        {
            return new UpdateAnimalResult()
            {
                Success = true,
                Message = new string("Successfully updated animal"),
                Pet = pet
            };
        }
    }
}
