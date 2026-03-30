using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using Color = System.Drawing.Color;

namespace gir.net.Infra;

public abstract class GIRCommandModule : ApplicationCommandModule<ApplicationCommandContext>
{
    protected InteractionMessageProperties ErrorResponse(string message)
    {
        var container = new ComponentContainerProperties()
            .WithComponents([
                new TextDisplayProperties("### :("),
                new TextDisplayProperties("### Your command ran into a problem"),
                new TextDisplayProperties(message),
            ])
            .WithAccentColor(new(Color.OrangeRed.ToArgb()));

        var response = new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.Ephemeral | MessageFlags.IsComponentsV2);

        return response;
    }

    protected InteractionMessageProperties SuccessResponse(string message,
        ComponentContainerProperties? extraContainer = null)
    {
        var container = new ComponentContainerProperties()
            .WithComponents([
                new TextDisplayProperties(message),
            ])
            .WithAccentColor(new(Color.Green.ToArgb()));

        var responseComponents = new List<ComponentContainerProperties>()
        {
            container
        };
        
        if  (extraContainer != null)
            responseComponents.Add(extraContainer);

        var response = new InteractionMessageProperties()
            .WithComponents(responseComponents)
            .WithFlags(MessageFlags.IsComponentsV2);

        return response;
    }

    protected InteractionMessageProperties ContainerResponse(ComponentContainerProperties container)
    {
        var response = new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.IsComponentsV2);

        return response;
    }
}