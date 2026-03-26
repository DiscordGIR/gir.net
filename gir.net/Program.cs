using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using NetCord.Hosting.Gateway;
using gir.net.Configurations;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.Commands;
using Microsoft.Extensions.Logging;
using NetCord.Gateway;

namespace gir.net;

class Program
{
    static async Task Main(string[] args)
    {
        // Load .env file
        DotNetEnv.Env.TraversePath().Load();

        var builder = Host.CreateApplicationBuilder(args);
        
        // Add environment variables to Configuration
        builder.Configuration.AddEnvironmentVariables();

        builder.Logging.AddSimpleConsole(c => c.SingleLine = true); 

        // Configure strongly typed settings
        builder.Services.Configure<Config>(builder.Configuration);

        var connectionString = builder.Configuration["DATABASE_CONNECTION_STRING"] ?? string.Empty;
        var discordToken = builder.Configuration["DISCORD_TOKEN"] ?? string.Empty;

        builder.Services.AddDbContext<Db>((sp, options) =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        builder.Services.AddScoped<gir.net.Domain.Interfaces.Repositories.IUserRepository, gir.net.Infrastructure.Repositories.UserRepository>();
        builder.Services.AddScoped<gir.net.Domain.Interfaces.Repositories.ITagRepository, gir.net.Infrastructure.Repositories.TagRepository>();
        builder.Services.AddScoped<gir.net.Application.Interfaces.Services.ITagService, gir.net.Application.Services.TagService>();

        builder.Services
            .AddDiscordGateway(options =>
            {
                options.Token = discordToken;
                options.Intents = GatewayIntents.GuildMessages | GatewayIntents.DirectMessages | GatewayIntents.MessageContent;
            })
            .AddApplicationCommands()
            .AddCommands();
        
        var host = builder.Build();
        
        host.AddModules(typeof(Program).Assembly);
        await using var scope = host.Services.CreateAsyncScope();
 
        await host.RunAsync();
    }
}
