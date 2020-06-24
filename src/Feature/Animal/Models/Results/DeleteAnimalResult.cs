using System;

namespace Animal.Models.Results
{
    public class DeleteAnimalResult : IAnimalResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }


        public static DeleteAnimalResult ReturnNullAnimalResult()
        {
            return new DeleteAnimalResult()
            {
                Success = false,
                Message = new String("Request empty")
            };
        }
    }
}
