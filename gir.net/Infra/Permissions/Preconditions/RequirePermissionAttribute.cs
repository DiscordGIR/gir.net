using gir.net.Configurations;
using NetCord;
using NetCord.Rest;
using NetCord.Services;

namespace gir.net.Infra.Permissions.Preconditions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePermissionAttribute<TContext>(PermissionLevel requiredLevel) : PreconditionAttribute<TContext>
    where TContext : IInteractionContext, IGuildContext
{
    private readonly PermissionLevel _requiredLevel = requiredLevel;

    public override ValueTask<PreconditionResult> EnsureCanExecuteAsync(TContext context, IServiceProvider? serviceProvider)
    {
        if (serviceProvider?.GetService(typeof(PermissionService)) is not PermissionService permissions)
            return new(PreconditionResult.Fail("Permission service is not available."));

        if (context.Interaction.User is not GuildUser guildUser)
            return new(PreconditionResult.Fail("This command can only be used in a server."));

        if (context.Guild is not RestGuild guild)
            return new(PreconditionResult.Fail("Guild information is not available yet; try again in a moment."));

        if (!permissions.Has(guildUser, guild, _requiredLevel))
            return new(PreconditionResult.Fail($"You need to be at least {_requiredLevel} to use this command."));

        return new(PreconditionResult.Success);
    }
}
