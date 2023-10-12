using Microsoft.Extensions.Options;

namespace consoleFromWorker1;

public class EmailProvider : IMessageProvider
{

    private readonly MessageConfig _messageConfig;

    public EmailProvider(IOptions<MessageConfig> messageConfig)
    {
        _messageConfig = messageConfig.Value;
    }

    public void Send()
    {
        var emailAddress = _messageConfig.EmailAddress;

        Console.WriteLine("mail");
    }
}