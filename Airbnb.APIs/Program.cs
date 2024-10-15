using Airbnb.APIs.Extensions;
using Airbnb.APIs.Utility;
using Airbnb.Application.Chatting;
using Airbnb.Domain.Interfaces.Interface;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Airbnb.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfigurations();

            await builder.Services.JWTConfigurations(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddSignalR();
            var app = builder.Build();

            // Apply Pending Migrations on Database
            await ExtensionMethods.ApplyMigrations(app);

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) =>
                       {
                           await context.Clients.All.ReceiveMessage(message);
                           return Results.NoContent();
                       });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization(); 

            app.MapHub<ChatHub>("chat-hub");

            app.MapControllers();



            app.Run();
        }
    }
}
