namespace FileProcessingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services
                .Configure<FileProcessConfig>(
                builder.Configuration.GetSection("FileProcessConfig"));
            builder.Services.AddHostedService<FileProcessor>();

            var host = builder.Build();
            host.Run();
        }
    }
}