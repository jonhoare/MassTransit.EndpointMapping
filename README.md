# MassTransit.EndpointMapping
Having previously worked with NServiceBus, I found their code to be very good at extracting away the mapping of Commands to Endpoint Queues.

With MassTransit out of the box, whenever you want to Send a Command to a specific endpoint, you have to request an IBus, then call the method GetSendEndpoint, and pass in as a parameter the full path of the endpoint queue.

I found this quite inconvient and made my code quite dirty. I ended up with lots of code in lots of my classes having to know about and ask for a SendEndpoint and each page had to know the full path of my endpoints.

So I have written an extension helper class to work with MassTransit that will allow me to register all my commands once and map them to specific queues.

Now I only need to call my new "Send" ExtensionMethod on an instance of an IBus, to send my command without worrying about knowing where it needs to be sent.

When you call Send, the extension method will extract the namespace that the command belongs to and lookup the namespace in its dictionary to work out the relative queue path of where the command should be sent. This is combined with the BaseUri, derived from the current IBus Address, and then sent using the traditional MassTransit Send.

If you have worked with NServiceBus before, the concept is similar, whereby you map a namespace to a specific Queue Uri.

This should work with AzureServiceBus, RabbitMQ and the In-Memory transports.

## Usage:

*Example Command*
```
namespace MyCompany.Service1.Contracts.V1.Commands
{
  public class FooCommandV1
  {
    public FooCommandV1() { }
    public FooCommandV1(string foo)
    {
      Foo = foo;
    }
    
    public string Foo { get; private set; }
  }
}
```

First of all, at application Startup you will want to add a mapping for each of your Commands Namespaces, and the QueueNames of where commands in the namespace are to be sent.

```
public virtual void Configuration(IAppBuilder app)
{
    var config = new HttpConfiguration();
    config.MapHttpAttributeRoutes();
    app.UseWebApi(config);
    
    ConfigureEndpointMappings();
}

public static void ConfigureEndpointMappings()
{
    EndpointMapping.AddMapping("MyCompany.Service1.Contracts.V1.Commands", "service1_queue");
    EndpointMapping.AddMapping("MyCompany.Service2.Contracts.V1.Commands", "service2_queue");
    EndpointMapping.AddMapping("MyCompany.Service2.Contracts.V2.Commands", "service2_queue");
}
```
Finally, when you need to send a command, call the Send ExtensionMethod now available on an instance of an IBus.

```
public async void DoSomeWork()
{
  await bus.Send(new FooCommandV1("FooBar"));
}
```
