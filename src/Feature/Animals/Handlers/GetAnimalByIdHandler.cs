using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using AutoMapper;
using FluentValidation;
using MediatR;
using Models.ResponseModels;

namespace Animals.Handlers
{
    public class GetAnimalByIdHandler : IRequestHandler<GetAnimalByIdCommand, GetAnimalResult>
    {
        public class Validator : AbstractValidator<GetAnimalByIdCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.AnimalId)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
            }
        }

        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;

        public GetAnimalByIdHandler(IAnimalService animalService, IMapper mapper)
        {
            _animalService = animalService;
            _mapper = mapper;
        }

        public async Task<GetAnimalResult> Handle(GetAnimalByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return GetAnimalResult.RequestEmptyResult();
            }

            var animal = await _animalService.GetAnimalAsync(request.AnimalId);

            return animal is null ? GetAnimalResult.AnimalNotFoundResult() : GetAnimalResult.AnimalFoundResult(_mapper.Map<AnimalResponse>(animal));
        }
    }
}
