﻿using System;
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

        public async Task<Animal> GetAnimalAsync(int animalId)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == animalId);

            if (animal == null || !await UserOwnsAnimalAsync(animal.Id))
            {
                return null;
            }

            return animal;
        }

        public async Task<bool> UpdateAnimalAsync(UpdateAnimalCommand request)
        {
            try
            {
                var animalToUpdate = await _context.Animals.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (animalToUpdate == null)
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
            animalToUpdate.Name = request.Name ?? animalToUpdate.Name;
            animalToUpdate.Species = request.Species ?? animalToUpdate.Species;
            animalToUpdate.Age = request.Age >= 0 ? request.Age : animalToUpdate.Age;
        }

        public async Task<bool> DeleteAnimalAsync(int animalId)
        {
            var animalToDelete = await _context.Animals.FirstOrDefaultAsync(x => x.Id == animalId);

            if (animalToDelete == null)
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
                Name = request.Name,
                Age = request.Age,
                Species = request.Species,
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
