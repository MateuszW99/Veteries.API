using Domain.Entities;
using Models;
using Models.ResponseModels;

namespace Animals.Models.Results
{
    public class CreateAnimalResult : IAnimalResult
    {
        public AnimalResponse Animal { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static CreateAnimalResult RequestEmptyResult()
        {
            return new CreateAnimalResult()
            {
                Success = false,
                Message = ResultMessages.BadRequest
            };
        }

        public static CreateAnimalResult SuccessfulResult(AnimalResponse animal)
        {
            return new CreateAnimalResult
            {
                Success = true,
                Message = ResultMessages.CreationSuccessful,
                Animal = animal
            };
        }
    }
}
