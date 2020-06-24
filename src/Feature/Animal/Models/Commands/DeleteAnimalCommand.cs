﻿using Animal.Models.Results;
using MediatR;

namespace Animal.Models.Commands
{
    public class DeleteAnimalCommand : IRequest<DeleteAnimalResult>
    {
        public int Id { get; set; }
    }

    public static class DeleteAnimalCommandExtension
    {
        public static bool IsNull(this DeleteAnimalCommand command)
        {
            return command == null ? true : false;
        }
    }
}
