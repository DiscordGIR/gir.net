using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Domain.Exceptions;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using gir.net.Views;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

[SlashCommand("tags", "Manage tags")]
public class TagAdminCommandModule(ITagService tagService, ILogger<TagAdminCommandModule> logger)
    : GIRBaseCommandModule(logger)
{
    private static readonly TagCreationModal TagCreationModal = new TagCreationModal();

    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SubSlashCommand("add", "Create new tag")]
    public async Task<InteractionCallbackProperties> AddTag(string name)
    {
        if (tagService.GetTagAsync(name).Result != null)
        {
            var callback = InteractionCallback.Message(ErrorResponse($"Tag with name '{name}' is already exists."));
            return callback;
        }
        
        var modal = TagCreationModal.CreateFrom(name);
        return InteractionCallback.Modal(modal);
    }
}