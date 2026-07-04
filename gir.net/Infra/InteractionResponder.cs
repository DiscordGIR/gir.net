using gir.net.Views;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;

namespace gir.net.Infra;

public enum ReplyVisibility
{
    Auto,
    Public,
    Ephemeral,
}

public sealed record ReplyOptions(
    ReplyVisibility Visibility = ReplyVisibility.Auto,
    AllowedMentionsProperties? AllowedMentions = null,
    string? Content = null)
{
    public static ReplyOptions Public { get; } = new(ReplyVisibility.Public);
    public static ReplyOptions Ephemeral { get; } = new(ReplyVisibility.Ephemeral);
}

public sealed class InteractionResponder(GIRContext context, ILogger logger)
{
    private static readonly ErrorView ErrorView = new();
    private static readonly TimeSpan MinOriginalResponseDeleteDelay = TimeSpan.FromMilliseconds(500);

    public async Task DeferAsync(MessageFlags flags = default, CancellationToken cancellationToken = default)
    {
        await context.Interaction
            .SendResponseAsync(InteractionCallback.DeferredMessage(flags), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        context.MarkInteractionResponded();
    }

    public Task ReplyAsync(
        ComponentContainerProperties container,
        ReplyOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var opts = options ?? new ReplyOptions();
        var flags = ResolveFlags(opts.Visibility);
        var properties = InteractionMessages.FromContainer(
            container, flags, opts.AllowedMentions, opts.Content);
        return ReplyAsync(properties, cancellationToken);
    }

    public Task ReplyErrorAsync(string message, CancellationToken cancellationToken = default) =>
        ReplyAsync(ErrorView.CreateFrom(message), ReplyOptions.Ephemeral, cancellationToken);

    public async Task ReplyAsync(
        InteractionMessageProperties properties,
        CancellationToken cancellationToken = default)
    {
        if (context.InteractionResponded)
        {
            await context.Interaction
                .ModifyResponseAsync(m => InteractionMessages.Apply(m, properties), cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return;
        }

        await context.Interaction
            .SendResponseAsync(InteractionCallback.Message(properties), cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        context.MarkInteractionResponded();
    }

    public void ScheduleDeleteAfter(TimeSpan deleteAfter, CancellationToken cancellationToken = default)
    {
        var interaction = context.Interaction;
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
                logger.LogWarning(ex, "Failed to auto-delete the original interaction response.");
            }
        }
    }

    private MessageFlags ResolveFlags(ReplyVisibility visibility)
    {
        var flags = MessageFlags.IsComponentsV2;

        if (visibility == ReplyVisibility.Ephemeral ||
            visibility == ReplyVisibility.Auto && context.ShouldWhisperIfNoPermissions())
            flags |= MessageFlags.Ephemeral;

        return flags;
    }
}
