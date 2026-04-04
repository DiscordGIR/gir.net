using gir.net.Views;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Infra;

public abstract class GIRBaseCommandModule : ApplicationCommandModule<GIRContext>
{
    private static readonly TimeSpan MinOriginalResponseDeleteDelay = TimeSpan.FromMilliseconds(500);

    private static readonly ErrorView ErrorView = new();
    private static readonly SuccessView SuccessView = new();

    private readonly ILogger _logger;

    protected GIRBaseCommandModule(ILogger logger)
    {
        _logger = logger;
    }

    protected void ScheduleResponseDeleteAfter(TimeSpan deleteAfter,
        CancellationToken cancellationToken = default)
    {
        var interaction = Context.Interaction;
        var delay = deleteAfter < MinOriginalResponseDeleteDelay ? MinOriginalResponseDeleteDelay : deleteAfter;

        _ = DeleteAfterAsync();

        async Task DeleteAfterAsync()
        {
            try
            {
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                await interaction.DeleteResponseAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to auto-delete the original interaction response.");
            }
        }
    }

    protected InteractionMessageProperties ErrorResponse(string message)
    {
        var container = ErrorView.CreateFrom(message);
        
        var response = new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.Ephemeral | MessageFlags.IsComponentsV2);
        
        return response;
    }

    protected InteractionMessageProperties SuccessResponse(string message,
        ComponentContainerProperties? extraContainer = null)
    {
        var container = SuccessView.CreateFrom(message);

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
        var container = SuccessView.CreateFrom(title, message);

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
            .WithFlags(MessageFlags.IsComponentsV2 | (ephemralIfNoob && Context.ShouldWhisperIfNoPermissions() ? MessageFlags.Ephemeral : 0));

        return response;
    }

    protected async Task<InteractionCallbackResponse?> SendResponse(InteractionMessageProperties properties)
    {
        var callback = InteractionCallback.Message(properties);
        return await RespondAsync(callback);
    }
    
    protected async Task<RestMessage> SendEditResponse(InteractionMessageProperties properties)
    {
        return await ModifyResponseAsync(m =>
        {
            m.Components = properties.Components;
            m.Flags = properties.Flags;
        });
    }
}