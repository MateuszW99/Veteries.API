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
    public class CreateAnimalHandler : IRequestHandler<CreateAnimalCommand, CreateAnimalResult>
    {
        public class Validator : AbstractValidator<CreateAnimalCommand>
        {
            public Validator()
            {
                this.RuleFor(x => x.Name)
                    .NotNull()
                    .Length(2, 20);

                this.RuleFor(x => x.Species)
                    .NotNull()
                    .Length(2, 20);


                this.RuleFor(x => x.Age)
                    .GreaterThan(0);
            }
        }

        private readonly IAnimalService _service;
        private readonly IMapper _mapper;

        public CreateAnimalHandler(IAnimalService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        public async Task<CreateAnimalResult> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            if (request.IsNull())
            {
                return CreateAnimalResult.RequestEmptyResult();
            }

            var animal = await _service.CreateAnimalAsync(request);

            return CreateAnimalResult.SuccessfulResult(_mapper.Map<AnimalResponse>(animal));
        }
    }
}
