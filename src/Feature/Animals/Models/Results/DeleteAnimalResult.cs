using Models;

namespace Animals.Models.Results
{
    public class DeleteAnimalResult : IAnimalResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }


        public static DeleteAnimalResult RequestEmptyResult()
        {
            return new DeleteAnimalResult()
            {
                Success = false,
                Message = ResultMessages.EmptyRequest
            };
        }

        public static DeleteAnimalResult BadRequestResult()
        {
            return new DeleteAnimalResult()
            {
                Success = false,
                Message = ResultMessages.DeletionFailed
            };
        }

        public static DeleteAnimalResult SuccessfulResult()
        {
            return new DeleteAnimalResult()
            {
                Success = true,
                Message = ResultMessages.DeletionSuccessful
            };
        }

        public static DeleteAnimalResult AccessDeniedResult()
        {
            return new DeleteAnimalResult()
            {
                Success = false,
                Message = ResultMessages.AccessDenied
            };
        }
    }
}
