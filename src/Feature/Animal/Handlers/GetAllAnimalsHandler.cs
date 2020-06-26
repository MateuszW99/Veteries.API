using System.Threading;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Persistence.Domain;

namespace Animal.Handlers
{
    public class GetAllAnimalsHandler : IRequestHandler<GetAllAnimalsCommand, GetAllAnimalsResult>
    {
        private readonly DomainDbContext _context;

        public GetAllAnimalsHandler(DomainDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllAnimalsResult> Handle(GetAllAnimalsCommand request, CancellationToken cancellationToken)
        {
            var animals = await _context.Animals.ToListAsync();

            if (!animals.Any())
            {
                return GetAllAnimalsResult.NoAnimalFoundResult();
            }

            return new GetAllAnimalsResult
            {
                Success = true,
                Animals = animals
            };
        }
    }
}
