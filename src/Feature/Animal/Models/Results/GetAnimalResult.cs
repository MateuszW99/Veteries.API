using Domain.Entities;

namespace Animal.Models.Results
{
    public class GetAnimalResult : IAnimalResult
    {
        public Pet Pet { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static GetAnimalResult RequestEmptyResult()
        {
            return new GetAnimalResult()
            {
                Success = false,
                Message = new string("Request empty"),
                Pet = null
            };
        }

        public static GetAnimalResult AnimalNotFoundResult(int id)
        {
            return new GetAnimalResult()
            {
                Success = false,
                Message = new string($"Error when deleting animal with id: {id}"),
                Pet = null
            };
        }
    }
}
