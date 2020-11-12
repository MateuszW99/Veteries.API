﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Module = Autofac.Module;

namespace Mapper
{
    public class DependencyInjection : Module
    {
        private readonly IEnumerable<Assembly> _assembliesToScan;

        public DependencyInjection(IEnumerable<Assembly> assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        public DependencyInjection(params Assembly[] assembliesToScan) 
            : this((IEnumerable<Assembly>)assembliesToScan) {}

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var assembliesToScan = _assembliesToScan as Assembly[] ?? _assembliesToScan.ToArray();

            var allTypes = assembliesToScan
                .Where(a => !a.IsDynamic && a.GetName().Name != nameof(AutoMapper))
                .Distinct()
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

            var openTypes = new[]
            {
                typeof(IValueResolver<,,>),
                typeof(IMemberValueResolver<,,,>),
                typeof(IValueConverter<,>),
                typeof(ITypeConverter<,>),
                typeof(IMappingAction<,>)
            };

            foreach (var type in openTypes
                .SelectMany(openType => allTypes
                    .Where(t => t.IsClass & !t.IsAbstract && ImplementsGenericInterface(t.AsType(), openType))))
            {
                builder.RegisterType(type.AsType()).InstancePerDependency();
            }

            builder.Register<IConfigurationProvider>(ctx
                => new MapperConfiguration(cfg 
                    => cfg.AddMaps(assembliesToScan)))
                        .SingleInstance();

            builder.Register<IMapper>(ctx 
                => new AutoMapper.Mapper(ctx.Resolve<IConfigurationProvider>(), ctx.Resolve))
                    .InstancePerDependency();
        }

        private static bool ImplementsGenericInterface(Type type, Type interfaceType)
            => IsGenericType(type, interfaceType)
               || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => IsGenericType(@interface, interfaceType));

        private static bool IsGenericType(Type type, Type genericType)
            => type.GetType().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}