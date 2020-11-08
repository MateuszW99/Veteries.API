using Models;
using Models.ResponseModels;

namespace Animals.Models.Results
{
    public class GetAnimalResult : IAnimalResult
    {
        public AnimalResponse Animal { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static GetAnimalResult RequestEmptyResult()
        {
            return new GetAnimalResult()
            {
                Success = false,
                Message = ResultMessages.BadRequest
            };
        }

        public static GetAnimalResult AnimalNotFoundResult()
        {
            return new GetAnimalResult()
            {
                Success = false,
                Message = ResultMessages.AnimalNotFound
            };
        }

        public static GetAnimalResult AnimalFoundResult(AnimalResponse animal)
        {
            return new GetAnimalResult()
            {
                Success = true,
                Animal = animal
            };
        }
    }
}
