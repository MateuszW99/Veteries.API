﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Animals.Models.Commands;

namespace Animals.Abstractions
{
    public interface IAnimalService
    {
        bool IsAnimalNull(Domain.Entities.Animal animal);

        Task<Domain.Entities.Animal> CreateAnimalAsync(CreateAnimalCommand request);
        Task<Domain.Entities.Animal> GetAnimalAsync(int animalId);
        Task<bool> UpdateAnimalAsync(UpdateAnimalCommand request);
        Task<bool> DeleteAnimalAsync(int animalId);
        Task<IEnumerable<Domain.Entities.Animal>> GetAnimalsAsync();
        Task<bool> UserOwnsAnimalAsync(int animalId);
    }
}
