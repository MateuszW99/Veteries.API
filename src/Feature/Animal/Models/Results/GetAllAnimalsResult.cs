using System.Collections.Generic;
using Domain.Entities;

namespace Animal.Models.Results
{
    public class GetAllAnimalsResult : IAnimalResult
    {
        public IEnumerable<Pet> Pets;
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
