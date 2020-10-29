using Models;

namespace Animals.Models.Results
{
    public class CreateAnimalResult : IAnimalResult
    {
        public Domain.Entities.Animal Animal { get; set; }
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

        public static CreateAnimalResult SuccessfulResult(Domain.Entities.Animal animal)
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
