using System.Collections.Generic;
using Models.ResponseModels;

namespace Animals.Models.Results
{
    public class GetAllAnimalsResult : IAnimalResult
    {
        public IEnumerable<AnimalResponse> Animals { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static GetAllAnimalsResult SuccessfulResult(IEnumerable<AnimalResponse> animals)
        {
            return new GetAllAnimalsResult()
            {
                Success = true,
                Animals = animals
            };
        }
    }
}
