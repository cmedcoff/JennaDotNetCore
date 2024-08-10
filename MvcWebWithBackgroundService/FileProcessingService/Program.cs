
namespace FileProcessingService;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddFileProcessorService(builder.Configuration);
            var host = builder.Build();
            host.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}


