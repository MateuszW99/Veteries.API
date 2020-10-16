using Autofac;
using MediatR;
using System.Reflection;
using Animal.Abstractions;
using Animal.Internals;
using FluentValidation;

namespace Animal
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
