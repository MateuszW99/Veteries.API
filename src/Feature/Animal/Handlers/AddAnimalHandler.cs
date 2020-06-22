using System.Threading;
using System.Threading.Tasks;
using Animal.Models.Commands;
using Animal.Models.Results;
using MediatR;
using Persistence.Domain;

namespace Animal.Handlers
{
    public class AddAnimalHandler : IRequestHandler<AddAnimalCommand, AddAnimalResult>
    {
        private readonly DomainDbContext _context;

        public AddAnimalHandler(DomainDbContext context)
        {
            _context = context;
        }


        public async Task<AddAnimalResult> Handle(AddAnimalCommand request, CancellationToken cancellationToken)
        {
            await _context.AddAsync(request.Pet);
            return new AddAnimalResult { Pet =  request.Pet };
        }
    }
}
