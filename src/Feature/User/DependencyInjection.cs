﻿using Autofac;
using FluentValidation;
using MediatR;
using System.Reflection;
using User.Abstractions;
using User.Abstractions.Internals;

namespace User
{
    public class DependencyInjection : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(AbstractValidator<>));

            builder.RegisterType<TokenService>().As<ITokenService>();
        }
    }
}