namespace MassTransit.EndpointMapping.Test.Bar.Contract.V1.Commands
{
    public class BarCommandV1
    {
        public BarCommandV1(){ }

        public BarCommandV1(string bar)
        {
            Bar = bar;
        }

        public string Bar { get; private set; }
    }
}