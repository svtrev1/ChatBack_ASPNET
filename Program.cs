using Serilog.Events;
using Serilog;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;



namespace WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {     
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            var services = new Services.ChatService();
            builder.Services.AddSingleton<Services.IChatService>(services);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            var appName = typeof(Program).Assembly.GetName().Name;
            var loggerConfiguration = new LoggerConfiguration();
            Log.Logger = loggerConfiguration.MinimumLevel.Debug()
                                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                            .Enrich.FromLogContext()
                                            .WriteTo.Console()
                                            .CreateLogger();
            builder.Host.UseSerilog();
            var app = builder.Build();
            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();
            }
            app.UseAuthorization();
            app.UseCors();
            app.MapControllers();
            app.Run();
        }
    }
}
