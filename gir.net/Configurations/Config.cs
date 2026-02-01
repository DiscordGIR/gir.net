namespace gir.net.Configurations;

public class Config
{
    public string ApplicationName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public ulong GuildId { get; set; }
    public ulong OwnerId { get; set; }
    public bool IsDevelopment { get; set; }
    public bool IsProduction { get; set; }
}
