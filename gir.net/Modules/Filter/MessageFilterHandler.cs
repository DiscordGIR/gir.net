using Microsoft.Extensions.Logging;

using NetCord.Gateway;
using NetCord.Hosting.Gateway;

namespace gir.net.Modules.Filter;

public class MessageFilterHandler(ILogger<MessageFilterHandler> logger) : IMessageCreateGatewayHandler
{
    public ValueTask HandleAsync(Message message)
    {
        if (message.Author.IsBot) return default;
        
        logger.LogInformation("{}", message.Content);
        return default;
    }
}