using System.Collections.Generic;
using Models;

namespace Animals.Models.Results
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
                Message = ResultMessages.DatabaseEmpty
            };
        }

        public static GetAllAnimalsResult AccessDeniedResult()
        {
            return new GetAllAnimalsResult()
            {
                Success = false,
                Message = ResultMessages.AccessDenied
            };
        }

        public static GetAllAnimalsResult SuccessfulResult(IEnumerable<Domain.Entities.Animal> animals)
        {
            return new GetAllAnimalsResult()
            {
                Success = true,
                Animals = animals
            };
        }
    }
}
