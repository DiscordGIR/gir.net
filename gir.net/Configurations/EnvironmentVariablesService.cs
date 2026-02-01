using DotNetEnv;

namespace gir.net.Configurations;

public class EnvironmentVariablesService
{
    private readonly EnvironmentVariables _environmentVariables;

    public EnvironmentVariablesService()
    {
        // Automatically traverse parent directories to find .env file
        Env.TraversePath().Load();

        // Use DotNetEnv's GetString method which reads directly from the loaded .env file
        _environmentVariables = new EnvironmentVariables
        {
            DiscordToken = Env.GetString("DISCORD_TOKEN", string.Empty),
            DatabaseConnectionString = Env.GetString("DATABASE_CONNECTION_STRING", string.Empty)
        };
    }

    public EnvironmentVariables GetEnvironmentVariables() => _environmentVariables;

    // Convenience property for direct access
    public EnvironmentVariables Variables => _environmentVariables;
}
