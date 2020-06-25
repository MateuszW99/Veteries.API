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
                Message = new string("Request empty")
            };
        }

        public static DeleteAnimalResult BadRequestResult(int id)
        {
            return new DeleteAnimalResult()
            {
                Success = false,
                Message = new string($"Error when deleting animal with id: {id}")
            };
        }

        public static DeleteAnimalResult SuccessfulResult(int id)
        {
            return new DeleteAnimalResult()
            {
                Success = true,
                Message = new string($"Deleted animal with id: {id}")
            };
        }
    }
}
