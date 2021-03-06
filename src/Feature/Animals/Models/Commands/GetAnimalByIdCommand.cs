﻿using Animals.Models.Results;
using MediatR;

namespace Animals.Models.Commands
{
    public class GetAnimalByIdCommand : AnimalCommand, IRequest<GetAnimalResult>
    {
        public int AnimalId { get; set; }
    }
}
