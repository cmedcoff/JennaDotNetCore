using consoleFromWorker1;

IHost host = Host.CreateDefaultBuilder(args)
	 .ConfigureAppConfiguration(configurationBuilder =>
	 {

		 int i = 0;

		 // despite what docs say about default configuration providers for Host.CreatedDefaultHostBuilder(),
		 // https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host,
		 // I find that I must explicitly add support for command line args
		 configurationBuilder.AddCommandLine(args);

		 configurationBuilder.Build();
	 })
	.ConfigureServices((hostBuilder, services) =>
	{
		services.AddSingleton<MessageSender>();

		// command line driven provider (not configuration driven)
		// e.g. "email"
		//services.AddTransient<IMessageProvider>((ioc) =>
		//{
		//	bool useEmailProvider = args.Any(arg => arg.Contains("email"));
		//	return useEmailProvider ? new EmailProvider() : new SmsProvider();
		//});

		// configuration driven driven provider, configuration source
		// command line or appsettings{env}.json
		services.AddTransient<IMessageProvider>((ioc) =>
			bool.Parse(hostBuilder.Configuration["email"] ?? "false")
			? new EmailProvider() : new SmsProvider());

		//services.Configure<MessageOptions>(hostBuilder.Configuration.GetSection("Message"));

	})
    .Build();

// host.Run();

var doIt = host.Services.GetRequiredService<MessageSender>();
doIt.Execute();