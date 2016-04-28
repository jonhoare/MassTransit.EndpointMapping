namespace MassTransit.EndpointMapping.Test.Foo.Contract.V1.Commands
{
    public class FooCommandV1
    {
        public FooCommandV1(){ }

        public FooCommandV1(string foo)
        {
            Foo = foo;
        }

        public string Foo { get; private set; }
    }
}