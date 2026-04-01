using gir.net.Views;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;

namespace gir.net.Infra;

public abstract class GIRBaseCommandModule : ApplicationCommandModule<GIRContext>
{
    private static readonly ErrorView _errorView = new();
    private static readonly SuccessView _successView = new();

    protected InteractionMessageProperties ErrorResponse(string message)
    {
        var container = _errorView.CreateFrom(message);
        
        var response = new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.Ephemeral | MessageFlags.IsComponentsV2);
        
        return response;
    }

    protected InteractionMessageProperties SuccessResponse(string message,
        ComponentContainerProperties? extraContainer = null)
    {
        var container = _successView.CreateFrom(message);

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

    protected InteractionMessageProperties SuccessResponse(string title, string message)
    {
        var container = _successView.CreateFrom(message);

        var responseComponents = new List<ComponentContainerProperties>()
        {
            container
        };
        
        var response = new InteractionMessageProperties()
            .WithComponents(responseComponents)
            .WithFlags(MessageFlags.IsComponentsV2);

        return response;
    }

    protected InteractionMessageProperties ContainerResponse(ComponentContainerProperties container, bool ephemralIfNoob = true)
    {
        var response = new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.IsComponentsV2 | (ephemralIfNoob ? Context.WhisperFlagIfNoPermissions() : 0));

        return response;
    }
}