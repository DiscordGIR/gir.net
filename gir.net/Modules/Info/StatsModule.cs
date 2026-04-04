using gir.net.Infra;
using gir.net.Views;
using Microsoft.Extensions.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules.Info;

public class StatsModule(ILogger<StatsModule> logger) : GIRBaseCommandModule(logger) 
{
    [SlashCommand("ping", "Test server latency")]
    public async Task Ping()
    {
        var firstTime = DateTime.UtcNow;
        var view = new PingView(Context).CreateFrom($"**Latency**: testing...");
        await SendResponse(ContainerResponse(view));
        
        var secondTime = DateTime.UtcNow;
        var delta =  secondTime - firstTime;
        var formattedDelta = $"{Math.Ceiling(delta.TotalMilliseconds)}ms";
        
        var editedView =  new PingView(Context).CreateFrom($"**Latency**: {formattedDelta}");
        await SendEditResponse(ContainerResponse(editedView));
    }

    [SlashCommand("serverinfo", "xd")]
    public async Task<InteractionMessageProperties> ServerInfo()
    {
        var view = new ServerInfoView(Context).CreateFrom("");
        return ContainerResponse(view);
    }
}