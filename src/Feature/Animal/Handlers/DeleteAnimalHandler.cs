using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Domain;

namespace Animal.Handlers
{
    public class DeleteAnimalHandler : IRequestHandler<DeleteAnimalCommand, DeleteAnimalResult>
    {
        public class Validator : AbstractValidator<DeleteAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id)
                    .GreaterThanOrEqualTo(0);
            }
        }

        private readonly DomainDbContext _context;

        public DeleteAnimalHandler(DomainDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteAnimalResult> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return DeleteAnimalResult.ReturnNullAnimalResult();
            }

            var AnimalToBeDeleted = await _context.Pets.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (AnimalToBeDeleted == null)
            {
                return new DeleteAnimalResult()
                {
                    Success = false,
                    Message = new String($"No such animal with id: {request.Id}")
                };
            }

            _context.Pets.Remove(AnimalToBeDeleted);
            await _context.SaveChangesAsync();

            return new DeleteAnimalResult()
            {
                Success = true,
                Message = new String($"Deleted animal with id: {request.Id}")
            };
        }

        private DeleteAnimalResult IsRequestNull()
        {

        }
    }
}
