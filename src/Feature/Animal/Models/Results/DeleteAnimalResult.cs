using System;

namespace Animal.Models.Results
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
                Message = new String("Request empty")
            };
        }

        public static DeleteAnimalResult BadRequestResult(int id)
        {
            return new DeleteAnimalResult()
            {
                Success = false,
                Message = new String($"Error when deleting animal with id: {id}")
            };
        }

        public static DeleteAnimalResult SuccessfulResult(int id)
        {
            return new DeleteAnimalResult()
            {
                Success = true,
                Message = new String($"Deleted animal with id: {id}")
            };
        }
    }
}
