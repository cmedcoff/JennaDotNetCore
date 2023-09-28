using consoleFromWorker1;
using Microsoft.Extensions.Logging;

using IocExample;

// just for some output
var logger = LoggerFactory.Create(builder =>
{
    builder.ClearProviders();
    builder.AddConsole();
}).CreateLogger<Program>();

// "setup"
var ioc = new Container("ioc");
ioc.Register<IMessageProvider, EmailProvider>();
ioc.Register<IMessageSender, MessageSender>();

// object resolution
var objectWanted = ioc.Resolve<IMessageSender>() as MessageSender;

if (objectWanted is not null)
{
    var message = @$"objectWanted type is {0} and it's _messageProvider is of type {1}";
    logger.LogInformation(message, 
        objectWanted.GetType().Name, 
        objectWanted._messageProvider.GetType().Name);

    objectWanted.Execute();
}
