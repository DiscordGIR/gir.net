using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using gir.net.Configurations;
using gir.net.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace gir.net;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.AddSimpleConsole(c => c.SingleLine = true); 

        var envService = new EnvironmentVariablesService();
        var envVars = envService.GetEnvironmentVariables();
        builder.Services.AddSingleton(envVars);

        builder.Services.AddSingleton<ConfigService>();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<ConfigService>().GetConfig());
        
        builder.Services.AddDbContext<Db>((sp, options) =>
        {
            options.UseNpgsql(envVars.DatabaseConnectionString)
                .UseSnakeCaseNamingConvention();
        });

        builder.Services
            .AddDiscordGateway(options => options.Token = envVars.DiscordToken);

        
        var host = builder.Build();
        await using var scope = host.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<Db>();
 
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        
        await host.RunAsync();
    }
}