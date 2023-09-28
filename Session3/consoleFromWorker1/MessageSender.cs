using Microsoft.Extensions.Options;

namespace consoleFromWorker1;

public interface IMessageSender
{
    void Execute();
}

public class MessageSender : IMessageSender
{
    public readonly IMessageProvider _messageProvider;
    private readonly IOptions<MessageOptions> _options;

    //public MessageSender(IMessageProvider messageProvider)
    //{
    //    _messageProvider = messageProvider;
    //}

    public MessageSender(IMessageProvider messageProvider, IOptions<MessageOptions> options)
    {
        _messageProvider = messageProvider;
        _options = options;
    }

    public void Execute()
    {
        _messageProvider.Send(   );
    }
}