using Serilog.Events;
using Serilog;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using WebApiDemo.Controllers;

namespace WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {     
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = "yourIssuer",
                       ValidAudience = "yourAudience",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
                   };
               });

            // Добавляем политику авторизации
            builder.Services.AddAuthorization();


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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.MapControllers();
            app.MapHub<ChatHub>("/hub");
            app.Run();
        }
    }
}
