using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord.Gateway;

namespace gir.net.Infra.Permissions;

public class PermissionRoleValidationHostedService(
    GatewayClient gatewayClient,
    PermissionService permissionService,
    ILogger<PermissionRoleValidationHostedService> logger) : IHostedService
{
    private readonly GatewayClient _gatewayClient = gatewayClient;
    private readonly PermissionService _permissionService = permissionService;
    private readonly ILogger<PermissionRoleValidationHostedService> _logger = logger;
    private Func<ReadyEventArgs, ValueTask>? _onReady;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _onReady = OnReadyAsync;
        _gatewayClient.Ready += _onReady;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_onReady is not null)
            _gatewayClient.Ready -= _onReady;
        return Task.CompletedTask;
    }

    private async ValueTask OnReadyAsync(ReadyEventArgs _)
    {
        if (_onReady is not null)
            _gatewayClient.Ready -= _onReady;

        try
        {
            await _permissionService.CheckConfiguredRolesExistAsync(_gatewayClient.Rest).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Permission role validation failed after gateway ready");
        }
    }
}
