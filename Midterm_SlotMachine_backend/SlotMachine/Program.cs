using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO; 
using MIDTERM_A1_SECTION_LASTNAME_FIRSTNAME.Controllers;

namespace MIDTERM_A1_SECTION_LASTNAME_FIRSTNAME
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            
            var env = app.Environment;

       
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseStaticFiles();

            // Map controllers
            app.MapControllers();

            // Game frontend route
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/game"), gameApp =>
            {
                gameApp.UseStaticFiles();
                gameApp.Run(async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "game/index.html"));
                });
            });

            // Admin frontend route
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/admin"), adminApp =>
            {
                adminApp.UseStaticFiles();
                adminApp.Run(async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "admin/index.html"));
                });
            });


            SlotMachineController.AddTestData();

            app.Run();
        }
    }
}