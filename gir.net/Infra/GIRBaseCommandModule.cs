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
    private InteractionResponder? _responder;

    protected GIRBaseCommandModule(ILogger logger)
    {
        _logger = logger;
    }

    protected InteractionResponder Responder => _responder ??= new InteractionResponder(Context, _logger);

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

        return InteractionMessages.FromContainer(
            container,
            MessageFlags.Ephemeral | MessageFlags.IsComponentsV2);
    }

    protected InteractionMessageProperties SuccessResponse(string message,
        ComponentContainerProperties? extraContainer = null, AllowedMentionsProperties? allowedMentionsProperties = null)
    {
        var container = SuccessView.CreateFrom(message);

        var responseComponents = new List<ComponentContainerProperties> { container };

        if (extraContainer is not null)
            responseComponents.Add(extraContainer);

        var response = new InteractionMessageProperties()
            .WithComponents(responseComponents)
            .WithFlags(MessageFlags.IsComponentsV2)
            .WithAllowedMentions(allowedMentionsProperties ?? AllowedMentionsProperties.None);

        return response;
    }

    protected InteractionMessageProperties SuccessResponse(string title, string message)
    {
        var container = SuccessView.CreateFrom(title, message);

        return InteractionMessages.FromContainer(container);
    }

    protected InteractionMessageProperties ContainerResponse(
        ComponentContainerProperties container,
        bool ephemralIfNoob = true,
        AllowedMentionsProperties? allowedMentions = null)
    {
        var visibility = ephemralIfNoob ? ReplyVisibility.Auto : ReplyVisibility.Public;
        var flags = MessageFlags.IsComponentsV2;

        if (visibility == ReplyVisibility.Ephemeral ||
            visibility == ReplyVisibility.Auto && Context.ShouldWhisperIfNoPermissions())
            flags |= MessageFlags.Ephemeral;

        return InteractionMessages.FromContainer(container, flags, allowedMentions);
    }
}
