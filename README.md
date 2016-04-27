# MassTransit.EndpointMapping
Having previously worked with NServiceBus, I found their code to be very good at extracting away the mapping of Commands to Endpoint Queues.

With MassTransit out of the box, whenever you want to Send a Command to a specific endpoint, you have to request an IBus, then call the method GetSendEndpoint, and pass in as a parameter the full path of the endpoint queue.

I found this quite inconvient and made my code quite dirty. I ended up with lots of code in lots of my classes having to know about and ask for a SendEndpoint and each page had to know the full path of my endpoints.

So I have written an extension helper class to work with MassTransit and Autofac, that will allow me to once, at application start, create a dictionary of namespaces and relative queue paths.

Now the only thing I need to ask for from Autofac is an IEndpointSender and call Send, passing in my command.

The underlying code will first ask for your IBus and IBusControl to get the Bus' baseUri. Then the code will extract the Commands namespace and lookup the namespace in its dictionary to work out the relative queue path of where the command should be sent. This is combined with the BaseUri and then sent using the traditional MassTransit Send.

If you have worked with NServiceBus before, the concept is similar, whereby you map a namespace to a specific Queue Uri.

This has so far only been tested to work with Azure Service Bus queues and uses Autofac to register an IEndpointSender and Resolves your IBus and IBusControl instances.

## Usage:

*Example Command*
```
namespace MyCompany.Service1.Contracts.V1.Commands
{
  public class MyCommandV1
  {
    public MyCommandV1() { }
    public MyCommandV1(string bar)
    {
      Bar = bar;
    }
    
    public string Bar { get; private set; }
  }
}
```

First of all, you will need to have your application's Autofac ContainerBuilder register the Module. This is so that you can request an IEndpointSender and the EndpointSender can resolve your registered IBus and IBusControl.

```
var builder = new ContainerBuilder();
builder.RegisterModule<MassTransit.EndpointMapping.AutofacModule>();
var container = builder.Build();
```
Then at your application Startup you will want to add a mapping for each of your Commands Namespaces, and their relative QueueName where they are to be sent to.

```
public virtual void Configuration(IAppBuilder app)
{
    var config = ConfigureWebApi();
    GlobalLifetimeScope = ConfigureAutofac();
    config.DependencyResolver = new AutofacWebApiDependencyResolver(GlobalLifetimeScope);
    app.UseAutofacMiddleware(GlobalLifetimeScope);
    app.UseAutofacWebApi(config);
    app.UseWebApi(config);
    ConfigureEndpointMappings();
}

public static void ConfigureEndpointMappings()
{
    EndpointMapping.AddMapping("MyCompany.Service1.Contracts.V1.Commands", "service1queue");
    EndpointMapping.AddMapping("MyCompany.Service2.Contracts.V1.Commands", "service2queue");
    EndpointMapping.AddMapping("MyCompany.Service2.Contracts.V2.Commands", "service2queue");
}
```
Finally, when you need to send a command, request an IEndpointSender from autofac and call send, passing in your command.

```
public class Foo
{
    private IEndpointSender _endpointSender;
    
    public Foo(IEndpointSender endpointSender)
    {
      _endpointSender = endpointSender;
    }
    
    public async void DoSomeWork()
    {
      _endpointSender.Send(new MyCommandV1("FooBar"));
    }
}
```
