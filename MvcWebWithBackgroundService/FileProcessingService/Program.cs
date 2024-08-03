namespace FileProcessingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var fileStorageConfigSection = builder.Configuration.GetSection("FileStorageConfig");
            builder.Services.Configure<FileStorageConfig>(fileStorageConfigSection);
            if( fileStorageConfigSection.GetValue<bool>("UseBlobStorage") )
            {
                builder.Services.AddSingleton<IFileStorage, BlobStorage>();
            }
            else
            {
                builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();
            }

            builder.Services.AddHostedService<FileProcessor>();

            var host = builder.Build();
            host.Run();
        }
    }
}
