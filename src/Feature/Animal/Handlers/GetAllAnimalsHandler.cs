using System.Threading;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var pets = await _context.Pets.ToListAsync();
            return new GetAllAnimalsResult
            {
                Success = true,
                Pets = pets
            };
        }
    }
}
