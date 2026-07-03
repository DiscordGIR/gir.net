using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services;

namespace gir.net.Infra.Permissions.Preconditions;

[AttributeUsage(AttributeTargets.Parameter)]
public class RequireValidInvokeeAttribute<TContext> : ParameterPreconditionAttribute<TContext>
    where TContext : IInteractionContext, IGuildContext
{
    public override ValueTask<PreconditionResult> EnsureCanExecuteAsync(
        object? value,
        TContext context,
        IServiceProvider? serviceProvider)
    {
        if (value is not User target)
            return new(PreconditionResult.Fail("Invalid target user."));

        if (serviceProvider?.GetService(typeof(GuildMemberHierarchyService)) is not GuildMemberHierarchyService hierarchy)
            return new(PreconditionResult.Fail("Hierarchy service is not available."));

        if (serviceProvider.GetService(typeof(GatewayClient)) is not GatewayClient client)
            return new(PreconditionResult.Fail("Client is not available."));

        if (context.Interaction.User is not GuildUser actor)
            return new(PreconditionResult.Fail("This command can only be used in a server."));

        if (context.Guild is not RestGuild guild)
            return new(PreconditionResult.Fail("Guild information is not available yet; try again in a moment."));

        var result = hierarchy.CanActOn(actor, target, client.Id, guild);
        return new(result.IsAllowed
            ? PreconditionResult.Success
            : PreconditionResult.Fail(result.FailureMessage!));
    }
}
