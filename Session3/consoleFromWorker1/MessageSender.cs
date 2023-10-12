using Microsoft.Extensions.Options;

namespace consoleFromWorker1;

public interface IMessageSender
{
    void Execute();
}

public class MessageSender : IMessageSender
{
    public readonly IMessageProvider _messageProvider;

    public MessageSender(IMessageProvider messageProvider)
    {
        _messageProvider = messageProvider;
    }

    public void Execute()
    {
        _messageProvider.Send(   );
    }
}