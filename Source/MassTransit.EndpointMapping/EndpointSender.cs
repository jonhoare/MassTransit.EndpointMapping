using System;
using System.Threading.Tasks;

namespace MassTransit.EndpointMapping
{
    public class EndpointSender : IEndpointSender
    {
        private readonly IBus _bus;
        private readonly IBusControl _busControl;

        public EndpointSender(IBus bus, IBusControl busControl)
        {
            _busControl = busControl;
            _bus = bus;
        }

        public async Task Send(object command)
        {
            var address = _busControl.Address;

            var baseUriBuilder = new UriBuilder(address.Scheme, address.Host);

            var endpoint = await _bus.GetSendEndpoint(EndpointMapping.GetEndpointUri(baseUriBuilder.Uri, command));
            await endpoint.Send(command);
        }
    }
}