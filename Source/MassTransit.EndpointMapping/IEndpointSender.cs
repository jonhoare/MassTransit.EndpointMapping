using System.Threading.Tasks;

namespace MassTransit.EndpointMapping
{
    public interface IEndpointSender
    {
        /// <summary>
        /// Sends a command using your registered IBus, by looking up the commands Namespace in the EndpointMappings Collection.
        /// </summary>
        /// <param name="command">Command to send</param>
        /// <returns></returns>
        Task Send(object command);
    }
}