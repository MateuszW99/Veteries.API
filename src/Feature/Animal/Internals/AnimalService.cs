using System.Collections.Generic;
using System.Threading.Tasks;
using Animal.Abstractions;
using Animal.Models.Commands;
using Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Domain;

namespace Animal.Internals
{
    public class AnimalService : IAnimalService
    {
        private readonly DomainDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnimalService(DomainDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAnimalNull(Domain.Entities.Animal animal)
        {
            return animal == null ? true : false;
        }

        public async Task<Domain.Entities.Animal> ReadAnimalAsync(int animalId)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == animalId);

            return animal;
        }

        public async Task<bool> UpdateAnimalAsync(UpdateAnimalCommand request)
        {
            var animalToUpdate = await _context.Animals.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (IsAnimalNull(animalToUpdate))
            {
                return false;
            }

            UpdateAnimalCredentials(request, animalToUpdate);

            _context.Animals.Update(animalToUpdate);
            await _context.SaveChangesAsync();

            return true;
        }

        private static void UpdateAnimalCredentials(UpdateAnimalCommand request, Domain.Entities.Animal animalToUpdate)
        {
            animalToUpdate.Name = request.Name ?? animalToUpdate.Name;
            animalToUpdate.Species = request.Name ?? animalToUpdate.Species;
            animalToUpdate.Age = request.Age >= 0 ? request.Age : animalToUpdate.Age;
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

        public async Task<IEnumerable<Domain.Entities.Animal>> ReadAllAnimals()
        {
            var animals = await _context.Animals.ToListAsync();

            return animals;
        }

        public async Task<Domain.Entities.Animal> CreateAnimalAsync(CreateAnimalCommand request)
        {
            var AnimalToCreate = new Domain.Entities.Animal()
            {
                Name = request.Name,
                Age = request.Age,
                Species = request.Species,
                OwnerId = _httpContextAccessor.HttpContext.GetUserId()
            };

            await _context.Animals.AddAsync(AnimalToCreate);
            await _context.SaveChangesAsync();

            return AnimalToCreate;
        }
    }
}
