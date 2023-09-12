using consoleFromWorker1;
using Microsoft.Extensions.Logging;
using IocExample;

// just for some output
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.ClearProviders();
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<Program>();

var ioc = new Container("ioc");
ioc.Register<IMessageProvider, EmailProvider>();
ioc.Register<IMessageSender, MessageSender>();
var objectWanted = ioc.Resolve<IMessageSender>() as MessageSender;
if (objectWanted is not null)
{
    var message = @$"objectWanted type is {objectWanted.GetType().Name} 
        and it's _messageProvider is of type {objectWanted._messageProvider.GetType().Name}";
    logger.LogInformation(message);
}
