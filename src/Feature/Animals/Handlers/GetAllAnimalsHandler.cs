using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Animals.Abstractions;
using Animals.Models.Commands;
using Animals.Models.Results;
using AutoMapper;
using MediatR;
using Models.ResponseModels;

namespace Animals.Handlers
{
    public class GetAllAnimalsHandler : IRequestHandler<GetAllAnimalsCommand, GetAllAnimalsResult>
    {
        private readonly IAnimalService _animalService;
        private readonly IMapper _mapper;

        public GetAllAnimalsHandler(IAnimalService animalService, IMapper mapper)
        {
            _animalService = animalService;
            _mapper = mapper;
        }

        public async Task<GetAllAnimalsResult> Handle(GetAllAnimalsCommand request, CancellationToken cancellationToken)
        {
            var animals = await _animalService.GetAnimalsAsync();

            return GetAllAnimalsResult.SuccessfulResult(_mapper.Map<List<AnimalResponse>>(animals));
        }
    }
}
