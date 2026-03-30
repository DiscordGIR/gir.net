namespace gir.net.Configurations;

public class DiscordPermissionOptions
{
    public const string SectionName = "Discord:Permissions";

    public ulong? Everyone { get; set; }
    public ulong? MemberPlus { get; set; }
    public ulong? MemberPro { get; set; }
    public ulong? MemberEdition { get; set; }
    public ulong? Genius { get; set; }
    public ulong? Moderator { get; set; }
    public ulong? Administrator { get; set; }
}
