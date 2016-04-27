using System.Threading.Tasks;

namespace MassTransit.EndpointMapping
{
    public interface IEndpointSender
    {
        Task Send(object model);
    }
}