using Models;

namespace Animal.Models.Results
{
    public class GetAnimalResult : IAnimalResult
    {
        public Domain.Entities.Animal Animal { get; set; }
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

        public static GetAnimalResult AnimalNotFoundResult(int id)
        {
            return new GetAnimalResult()
            {
                Success = false,
                Message = ResultMessages.AnimalNotFound
            };
        }

        public static GetAnimalResult AnimalFoundResult(Domain.Entities.Animal animal)
        {
            return new GetAnimalResult()
            {
                Success = true,
                Animal = animal
            };
        }

        public static GetAnimalResult AccessDeniedResult()
        {
            return new GetAnimalResult()
            {
                Success = true,
                Message = ResultMessages.AccessDenied
            };
        }
    }
}
