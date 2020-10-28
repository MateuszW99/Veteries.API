using Autofac;
using Services.Abstractions;
using Services.Externals;
using System.Reflection;
using Module = Autofac.Module;

namespace Services
{
    public class DependencyInjection : Module {
        
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DependencyInjection).GetTypeInfo().Assembly)
                .Where(x => x.FullName == nameof(IHttpService))
                .AsImplementedInterfaces();

            builder.RegisterType<HttpService>().As<IHttpService>();
        }
    }
}