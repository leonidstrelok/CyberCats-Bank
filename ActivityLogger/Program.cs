using System.Threading.Tasks;
using ActivityLogger.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
using Serilog;

namespace ActivityLogger
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await appDbContext.Database.MigrateAsync();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddEnvironmentVariables();

                if (!context.HostingEnvironment.IsProduction())
                {
                    builder.AddJsonFile($"serilogconfig.{context.HostingEnvironment.EnvironmentName}.json",
                            optional: true);
                }
            })
            .UseSerilog((context, configuration) =>
                {
                    configuration.Enrich.FromLogContext().ReadFrom.Configuration(context.Configuration);
                })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}