namespace MassTransit.EndpointMapping.Test.Bar.Contract.V1.Events
{
    public class BarConsumedV1
    {
        public BarConsumedV1() { }

        public BarConsumedV1(string bar)
        {
            Bar = bar;
        }

        public string Bar { get; private set; }
    }
}