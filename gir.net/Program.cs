using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using gir.net.Configurations;

namespace gir.net;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var envService = new EnvironmentVariablesService();
        var envVars = envService.GetEnvironmentVariables();
        builder.Services.AddSingleton(envVars);

        builder.Services.AddSingleton<ConfigService>();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<ConfigService>().GetConfig());

        builder.Services
            .AddDiscordGateway(options => options.Token = envVars.DiscordToken);

        var host = builder.Build();
        await host.RunAsync();
    }
}