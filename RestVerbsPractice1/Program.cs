using RestVerbsPractice1;
using RestVerbsPractice1.Data;

using Microsoft.AspNetCore.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddControllers()
            .AddNewtonsoftJson();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<SqliteContext>();
        builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

        builder.Services.AddApiVersioning(config =>
        {
          config.DefaultApiVersion = new ApiVersion(1, 0);
          config.AssumeDefaultVersionWhenUnspecified = true;
          config.ReportApiVersions = true;
        });
 

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
