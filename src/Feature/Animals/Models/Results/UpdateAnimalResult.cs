using Domain.Entities;
using Models;

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
                Message = ResultMessages.EmptyRequest
            };
            
        }

        public static UpdateAnimalResult BadRequestResult()
        {
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = ResultMessages.BadRequest
            };
        }

        public static UpdateAnimalResult SuccessfulResult()
        {
            return new UpdateAnimalResult()
            {
                Success = true,
                Message = ResultMessages.UpdateSuccessful
            };
        }

        public static UpdateAnimalResult AccessDeniedResult()
        {
            return new UpdateAnimalResult()
            {
                Success = false,
                Message = ResultMessages.AccessDenied
            };
        }
    }
}
