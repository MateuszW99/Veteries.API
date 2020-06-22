using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    public class GetAnimalHandler : IRequestHandler<GetAnimalCommand, GetAnimalResult>
    {
        public class Validator : AbstractValidator<GetAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Id)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
            }
        }

        private readonly DomainDbContext _context;

        public GetAnimalHandler(DomainDbContext context)
        {
            _context = context;
        }

        public async Task<GetAnimalResult> Handle(GetAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return new GetAnimalResult()
                {
                    Success = false,
                    Pet = null
                };
            }

            var pet = await _context.Pets.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (pet == null)
            {
                return new GetAnimalResult()
                {
                    Success = false,
                    Pet = null
                };
            }

            return new GetAnimalResult()
            {
                Success = true,
                Pet = pet
            };
        }
    }
}
