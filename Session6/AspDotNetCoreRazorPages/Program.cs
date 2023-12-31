﻿using AspDotNetCoreRazorPages.Data;

using Microsoft.EntityFrameworkCore;


namespace AspDotNetCoreRazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // new ...
            builder.Services.AddDbContext<CarDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CarDbContext") ?? throw new InvalidOperationException("Connection string 'CarDbContext' not found.")));

            builder.Services.AddTransient<ICarData, CarData>();

            // ... end new

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}