namespace MassTransit.EndpointMapping.Test.Foo.Contract.V1.Events
{
    public class FooConsumedV1
    {
        public FooConsumedV1() { }

        public FooConsumedV1(string foo)
        {
            Foo = foo;
        }

        public string Foo { get; private set; }
    }
}