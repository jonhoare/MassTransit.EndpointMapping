using System;
using System.Threading.Tasks;
using MassTransit.EndpointMapping.Test.Bar.Contract.V1.Commands;
using MassTransit.EndpointMapping.Test.Bar.Contract.V1.Events;
using MassTransit.EndpointMapping.Test.Foo.Contract.V1.Commands;
using MassTransit.EndpointMapping.Test.Foo.Contract.V1.Events;
using MassTransit.EndpointMapping.Test.Framework;
using NUnit.Framework;

namespace MassTransit.EndpointMapping.Test
{
    [TestFixture]
    public class BusExtenstionTests : EventTesting
    {
        private IBusControl _busControl;
        private IBus _bus { get { return _busControl; } }

        [OneTimeSetUp]
        public void Init()
        {
            ConfigureMappings();

            _busControl = Bus.Factory.CreateUsingInMemory(cfg =>
            {
                cfg.ReceiveEndpoint("test_queue", ep =>
                {
                    ep.Handler<FooConsumedV1>(context =>
                    {
                        Console.WriteLine("FooConsumedV1Consumer event received.");
                        ReportEvent(context.Message);
                        return context.CompleteTask;
                    });
                    ep.Handler<BarConsumedV1>(context =>
                    {
                        Console.WriteLine("BarConsumedV1Consumer event received.");
                        ReportEvent(context.Message);
                        return context.CompleteTask;
                    });
                });

                cfg.ReceiveEndpoint("foo_queue", ep =>
                {
                    ep.Handler<FooCommandV1>(async context =>
                    {
                        Console.WriteLine("FooCommandV1 successfully consumed");
                        await context.Publish(new FooConsumedV1(context.Message.Foo));
                    });
                });

                cfg.ReceiveEndpoint("bar_queue", ep =>
                {
                    ep.Handler<BarCommandV1>(async context =>
                    {
                        Console.WriteLine("BarCommandV1 successfully consumed");
                        await context.Publish(new BarConsumedV1(context.Message.Bar));
                    });
                });
            });

            _busControl.Start();
        }

        private static void ConfigureMappings()
        {
            EndpointMapping.AddMapping("MassTransit.EndpointMapping.Test.Foo.Contract.V1.Commands", "foo_queue");
            EndpointMapping.AddMapping("MassTransit.EndpointMapping.Test.Bar.Contract.V1.Commands", "bar_queue");
        }

        [Test]
        public async Task CanSendFooCommandFooQueueAndReceiveFooConsumedEvent()
        {
            var foo = "Test Command";
            SetEventExpectation<FooConsumedV1>(x => x.Foo == foo);

            await _bus.Send(new FooCommandV1(foo));

            VerifyEventExpectations();
        }

        [Test]
        public async Task CanSendBarCommandBarQueueAndReceiveBarConsumedEvent()
        {
            var bar = "Test Command";
            SetEventExpectation<BarConsumedV1>(x => x.Bar == bar);

            await _bus.Send(new BarCommandV1(bar));

            VerifyEventExpectations();
        }
    }
}
