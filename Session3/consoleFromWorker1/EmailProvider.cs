namespace consoleFromWorker1;

public class EmailProvider : IMessageProvider
{
    public void Send()
    {
        Console.WriteLine("mail");
    }
}