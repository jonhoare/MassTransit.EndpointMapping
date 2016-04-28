using System;
using MassTransit.EndpointMapping.Test.Bar.Contract.V1.Commands;
using MassTransit.EndpointMapping.Test.Foo.Contract.V1.Commands;
using NUnit.Framework;

namespace MassTransit.EndpointMapping.Test
{
    [TestFixture]
    public class EndpointMappingTests
    {
        [OneTimeSetUp]
        public void Init()
        {
            EndpointMapping.AddMapping("MassTransit.EndpointMapping.Test.Foo.Contract.V1.Commands", "foo_queue");
            EndpointMapping.AddMapping("MassTransit.EndpointMapping.Test.Bar.Contract.V1.Commands", "bar_queue");
        }
        
        [Test]
        public void FooCommandDoesReturnFooQueue()
        {
            var command = new FooCommandV1("Test Command");
            var baseUri = new Uri(string.Format("{0}://{1}", "scheme", "host"));

            var endpointUri = EndpointMapping.GetEndpointUri(baseUri, command);

            var expectedUri = new Uri(string.Format("{0}/foo_queue", baseUri));

            Assert.AreEqual(expectedUri, endpointUri);
        }

        [Test]
        public void BarCommandDoesReturnBarQueue()
        {
            var command = new BarCommandV1("Test Command");
            var baseUri = new Uri(string.Format("{0}://{1}", "scheme", "host"));

            var endpointUri = EndpointMapping.GetEndpointUri(baseUri, command);

            var expectedUri = new Uri(string.Format("{0}/bar_queue", baseUri));

            Assert.AreEqual(expectedUri, endpointUri);
        }
    }
}