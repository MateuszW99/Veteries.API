using Autofac;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace User
{
    class DependencyInjection : Module
    {
        protected override Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(AbstractValidator<>));

            builder.RegisterType<TokenService>().As<ITokenService>();
        }
    }
}
