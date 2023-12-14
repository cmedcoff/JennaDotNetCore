using Microsoft.AspNetCore.Mvc;

namespace GetAccountBalance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

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

            app.MapGet("/balance", async Task<IResult> (HttpContext httpContext) =>
            {
                var accountNumber = httpContext.Request.Query["accountNumber"];

                if (int.TryParse(accountNumber, out int accountNumberInt))
                {
                    var result = await Task.FromResult(
                        new AccountBalance
                        {
                            AccountNumber = accountNumber,
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            Balance = accountNumberInt / 100.0m
                        });

                    Thread.Sleep(6000);
                    return TypedResults.Ok(result);
                }
                else
                {
                    return TypedResults.BadRequest();
                }
            })
                .WithName("GetAccountBalanceRestApi")
                .WithOpenApi();

            app.Run();
        }
    }
}
