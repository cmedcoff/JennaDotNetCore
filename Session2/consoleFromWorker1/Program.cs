
bool useEmail = true;


IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(ioc =>
	{
		ioc.AddSingleton<DoIt>();
		ioc.AddTransient<IMessageProvider>((x) =>
		{
			if(useEmail)
			{
				return new EmailProvider();
			}
		});
	})
    .Build();

// host.Run();

var doIt = host.Services.GetRequiredService<DoIt>();
doIt.Execute();

public class DoIt
{
	public void Execute()
	{
		System.Console.WriteLine("doing it!");
	}
}

public interface IMessageProvider
{
	public void Send();
}

public class EmailProvider : IMessageProvider
{
    public void Send()
    {
    
    }
}


