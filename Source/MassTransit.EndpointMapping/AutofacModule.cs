using Autofac;

namespace MassTransit.EndpointMapping
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EndpointSender>().As<IEndpointSender>();
        }
    }
}