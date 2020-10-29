using Application.Pipeline;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;

namespace Application
{
    public class DependencyInjection : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AddMediatr(builder);
        }

        private static void AddMediatr(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            var mediatrOpenTpes = new[]
            {
                typeof(IRequestHandler<,>)
            };

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ModelValidationPreProcessor<>)).As(typeof(IRequestPreProcessor<>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
        }
    }
}