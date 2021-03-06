﻿using Autofac;
using System.Reflection;
using Animals.Abstractions;
using Animals.Internals;
using FluentValidation;
using MediatR;

namespace Animals
{
    public class DependencyInjection : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(AbstractValidator<>));

            builder.RegisterType<AnimalService>().As<IAnimalService>();
        }
    }
}
