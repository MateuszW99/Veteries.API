using System;
using Domain.Entities;

namespace Animal.Models.Results
{
    public class CreateAnimalResult : IAnimalResult
    {
        public Pet Pet { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static CreateAnimalResult RequestEmptyResult()
        {
            return new CreateAnimalResult()
            {
                Success = false,
                Message = new String("Empty request"),
                Pet = null
            };
        }

        public static CreateAnimalResult SuccessfulResult(Pet pet)
        {
            return new CreateAnimalResult
            {
                Success = true,
                Message = new String("Successfully added a new animal"),
                Pet = pet
            };
        }
    }

}
