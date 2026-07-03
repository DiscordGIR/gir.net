using NetCord.Rest;

namespace gir.net.Infra.Services;

public interface ICaseDeliveryService
{
    Task<bool> TryDeliverDirectMessageAsync(
        ulong targetUserId,
        ComponentContainerProperties container,
        CancellationToken cancellationToken = default);

    Task<bool> TryDeliverPublicModLogAsync(
        ComponentContainerProperties container,
        ulong? userToPing = null,
        CancellationToken cancellationToken = default);
}
