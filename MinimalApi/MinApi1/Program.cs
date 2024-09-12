using Microsoft.AspNetCore.Http;

namespace MinApi1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddSingleton<MyInjection>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/message1", (HttpContext httpContext, MyInjection my) =>
        {
            //return Results.Ok(my.Get());
            //return  TypedResults.Ok<string>(my.Get());
            return my.Get(httpContext);
        })
        .WithName("message1")
        .WithOpenApi();

        app.Run();
    }
}

public class MyInjection
{
    // using HttpContext and Results.OK here couples this class
    // which likely has business logic to the web layer,
    // it's probabl better to unpack parameters in the endpoing handler, pass
    // them to the business logic, then return a business result,
    // then have the hanlder convert the business result to web "speak"
    public IResult Get(HttpContext context)
    {
        return Results.Ok("Hello from MyInjection");
    }
}
