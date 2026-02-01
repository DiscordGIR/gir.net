using Microsoft.Extensions.Configuration;

namespace gir.net.Configurations;

public class ConfigService
{
    private readonly Config _config;

    public ConfigService(IConfiguration configuration)
    {
        _config = new Config();
        configuration.Bind(_config);

        var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        _config.IsDevelopment = environment == "Development";
        _config.IsProduction = environment == "Production";
    }

    public Config GetConfig() => _config;

    public Config Configuration => _config;
}
