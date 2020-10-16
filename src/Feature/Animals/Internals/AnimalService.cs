using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Domain;
using Services.Abstractions;

namespace Animals.Internals
{
    public class AnimalService : IAnimalService
    {
        private readonly DomainDbContext _context;
        private readonly IHttpService _httpService;
        
        public AnimalService(DomainDbContext context, IHttpService httpService)
        {
            _context = context;
            _httpService = httpService;
        }

        public bool IsAnimalNull(Animal animal)
        {
            return animal == null ? true : false;
        }

        public async Task<Animal> GetAnimalAsync(int animalId)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == animalId);

            if (animal == null)
            {
                return null;
            }

            if (!await UserOwnsAnimalAsync(animal.Id))
            {
                return null;
            }

            return animal;
        }

        public async Task<bool> UpdateAnimalAsync(UpdateAnimalCommand request)
        {
            try
            {
                var animalToUpdate = await _context.Animals.FirstOrDefaultAsync(x => x.Id == request.Animal.Id);

                if (IsAnimalNull(animalToUpdate))
                {
                    return false;
                }

                UpdateAnimalCredentials(request, animalToUpdate);

                _context.Animals.Update(animalToUpdate);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void UpdateAnimalCredentials(UpdateAnimalCommand request, Animal animalToUpdate)
        {
            animalToUpdate.Name = request.Animal.Name ?? animalToUpdate.Name;
            animalToUpdate.Species = request.Animal.Species ?? animalToUpdate.Species;
            animalToUpdate.Age = request.Animal.Age >= 0 ? request.Animal.Age : animalToUpdate.Age;
        }

        public async Task<bool> DeleteAnimalAsync(int animalId)
        {
            var animalToDelete = await _context.Animals.FirstOrDefaultAsync(x => x.Id == animalId);

            if (IsAnimalNull(animalToDelete))
            {
                return false;
            }

            _context.Animals.Remove(animalToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            return await _context.Animals.ToListAsync();
        }
        
        public async Task<Animal> CreateAnimalAsync(CreateAnimalCommand request)
        {
            var userId = _httpService.GetUserId();

            if (userId == string.Empty)
            {
                return null;
            }
            
            var animalToCreate = new Animal()
            {
                Name = request.Animal.Name,
                Age = request.Animal.Age,
                Species = request.Animal.Species,
                UserId = userId
            };

            await _context.Animals.AddAsync(animalToCreate);
            await _context.SaveChangesAsync();

            return animalToCreate;
        }

        public async Task<bool> UserOwnsAnimalAsync(int animalId)
        {
            var userId = _httpService.GetUserId();
            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == animalId);
            return animal != null && userId != string.Empty && animal.UserId == userId;
        }
    }
}
