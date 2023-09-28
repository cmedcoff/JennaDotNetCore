using LoggingConsole2FromWorkerServiceTemplate;

IHost host = Host.CreateDefaultBuilder(args)
    //.ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .ConfigureLogging((hostBuilderContext, loggingBuilder) =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddDebug();
    })
    .ConfigureServices(hostbuilder =>
    {
        hostbuilder.AddTransient<NeedsGenericLogger>();
        
        // hack to get a non-generic logger
        // there seems to be debate about using a non-generic
        // lots of points of views can be found at this thread
        https://stackoverflow.com/questions/51345161/should-i-take-ilogger-iloggert-iloggerfactory-or-iloggerprovider-for-a-libra
        hostbuilder.AddSingleton<ILogger>(serviceProvider
            => serviceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(""));
        
        hostbuilder.AddTransient<NeedsLogging>();
        
        /*
         * I feel that both IOptions<T> and ILogger<T> are somewhat 'leaky abstractions'
         * ( google search 'leaky abstraction' )
         *
         * For IOptions<T> there is a way to just inject the T, but certain features
         * such as reloading configuration values at runtime are then lost.
         * I need to do more research on the details.
         *
         * We've seen the non-generic solution for ILoggger.
         *
         * In either case there is another way to avoid the possibly undesired coupling
         * to these interfaces (if you want code that works across both full framework and code)
         * by using the standard "Adapter Pattern".  E.g Define your own configuration and logging
         * interfaces and hide the details of .NET Core IOptions<T> and ILoggger inside
         * the .NET Core implementation and delegate to those objects.
         *
         * This is what we do in our own applications to decouple 3rd party Nuget packages
         * from our domain code/objects.  Thus we can do the same to decouple our code
         * from .NET Core 'leaky abstractions'.  As usual, it's all a set of trade off's.
         *
         * I do recall doing exactly these on some piece of BH code that I wrote and had defined
         * my own ILogger.  It made porting less work because lots of classes ahd a dependency
         * on my ILogger.  Had I gone the Microsoft route, I would have had to go update
         * each of those classes from mhy ILogger to MS ILogger<T>.  In the end I just wrote
         * a single new implementation of my ILogger and the ILoggerImplementation (or whatever
         * I called it), just accepted an MS ILogger as a dependency and delegated to it. A great example
         * of Open/Closed because instead of modifying a bunch of existing classes, I only added
         * and then updated the injection/setup.  Didn't have to touch the rest of the code, rebuild it
         * or regression test it.
         *
         * If what I've said is unclear at all, the take the time to do an exercise to implement this.
         * E.g. Write code that has some domain class/object, that requires logging and/or config,
         * and ensure it is decoupled from ILogger<T> and IOptions<T> via 2 adapters.
         * 
         */

    })
    .Build();

//await host.RunAsync();

//
// the built in ioc provides no means to get a non-generic I Logger
//
// the following code will raise an exception
//var logger = host.Services.GetRequiredService<ILogger>();
//logger.LogInformation("Some logging");
//

// 
// instead you must have the generic version as a dependency in order
// for the ioc to inject it
var o = host.Services.GetRequiredService<NeedsGenericLogger>();
o.LogSomething();

var o2 = host.Services.GetRequiredService<NeedsLogging>();
o2.LogSomething();