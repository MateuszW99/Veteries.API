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
                Message = new string("Request empty"),
                Animal = null
            };
        }

        public static GetAnimalResult AnimalNotFoundResult(int id)
        {
            return new GetAnimalResult()
            {
                Success = false,
                Message = new string($"Error when deleting animal with id: {id}"),
                Animal = null
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
    }
}
