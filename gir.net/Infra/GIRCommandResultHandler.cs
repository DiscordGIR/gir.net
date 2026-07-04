using gir.net.Views;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Rest;
using NetCord.Services;

namespace gir.net.Infra;

public class GIRCommandResultHandler<TContext>(MessageFlags? messageFlags = null)
    : IApplicationCommandResultHandler<TContext>
    where TContext : GIRContext
{
    private static readonly ErrorView _errorView = new();
    private const int MESSAGE_CONTENT_LIMIT = 4000;

    public async ValueTask HandleResultAsync(
        IExecutionResult result,
        TContext context,
        GatewayClient? client,
        ILogger logger,
        IServiceProvider services)
    {
        if (result is not IFailResult failResult)
            return;

        var resultMessage = failResult.Message;
        var interaction = context.Interaction;

        if (failResult is IExceptionResult exceptionResult)
        {
            var exceptionMessage =
                $"Execution of an application command `/{interaction.Data.Name}` failed with an exception";
            logger.LogError(exceptionResult.Exception, exceptionMessage);

            resultMessage = $"{exceptionMessage}\n\n```{exceptionResult.Exception}\n{exceptionResult.Exception.StackTrace}```";
        }
        else
        {
            logger.LogDebug(
                "Execution of an application command of name '{Name}' failed with '{Message}'",
                interaction.Data.Name,
                resultMessage);
        }

        if (resultMessage.Length >= MESSAGE_CONTENT_LIMIT)
            resultMessage = resultMessage[..(MESSAGE_CONTENT_LIMIT - 500)] + "...\n```";

        var errorView = _errorView.CreateFrom(resultMessage);
        var message = new InteractionMessageProperties()
            .WithComponents([errorView])
            .WithFlags(
                MessageFlags.IsComponentsV2 |
                MessageFlags.Ephemeral);

        await SendErrorResponseAsync(interaction, context.InteractionResponded, message);
    }

    private static async Task SendErrorResponseAsync(
        ApplicationCommandInteraction interaction,
        bool interactionResponded,
        InteractionMessageProperties message)
    {
        if (interactionResponded)
        {
            await interaction.ModifyResponseAsync(m => InteractionMessages.Apply(m, message));
            return;
        }

        try
        {
            await interaction.SendResponseAsync(InteractionCallback.Message(message));
        }
        catch (RestException ex) when (IsAlreadyAcknowledged(ex))
        {
            await interaction.ModifyResponseAsync(m => InteractionMessages.Apply(m, message));
        }
    }

    private static bool IsAlreadyAcknowledged(RestException ex) =>
        ex.Message.Contains("already been acknowledged", StringComparison.OrdinalIgnoreCase);
}
