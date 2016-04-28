using System;
using System.Threading.Tasks;

namespace MassTransit.EndpointMapping
{
    public static class BusExtensions
    {
        /// <summary>
        /// Sends a command to the Queue defined for its namespace in the EndpointMappings
        /// </summary>
        /// <param name="bus">This bus instance</param>
        /// <param name="command">The command to send</param>
        public static async Task Send(this IBus bus, object command)
        {
            var address = bus.Address;

            var baseUriBuilder = new UriBuilder(address.Scheme, address.Host);

            var endpoint = await bus.GetSendEndpoint(EndpointMapping.GetEndpointUri(baseUriBuilder.Uri, command));
            await endpoint.Send(command);
        }
    }
}