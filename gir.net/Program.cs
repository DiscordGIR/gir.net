using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using NetCord.Hosting.Gateway;
using gir.net.Configurations;
using gir.net.Infra.Permissions;
using gir.net.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using gir.net.Infra;
using NetCord.Services.ApplicationCommands;

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
        builder.Services.Configure<DiscordPermissionOptions>(
            builder.Configuration.GetSection(DiscordPermissionOptions.SectionName));

        builder.Services.AddSingleton<PermissionService>();
        builder.Services.AddHostedService<PermissionRoleValidationHostedService>();

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
        builder.Services.AddSingleton<gir.net.Application.Interfaces.Services.IImageStorageService, gir.net.Infrastructure.Services.CloudflareR2StorageService>();

        builder.Services
            .AddDiscordGateway(options =>
            {
                options.Token = discordToken;
                options.Intents = GatewayIntents.Guilds | GatewayIntents.GuildUsers |
                                  GatewayIntents.GuildMessages | GatewayIntents.DirectMessages |
                                  GatewayIntents.MessageContent;
            })
            .AddApplicationCommands<ApplicationCommandInteraction, GIRContext, AutocompleteInteractionContext>(options =>
            {
                options.DefaultContexts = [InteractionContextType.Guild];
                options.CreateContext = (interaction, client, services) =>
                    new GIRContext(interaction, client!, services.GetRequiredService<PermissionService>());
            });
        
        var host = builder.Build();
        
        host.AddModules(typeof(Program).Assembly);
        await using var scope = host.Services.CreateAsyncScope();
 
        await host.RunAsync();
    }
}
