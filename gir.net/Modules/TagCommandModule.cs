using gir.net.Application.Interfaces.Services;
using gir.net.Infra;
using gir.net.Views;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

public class TagCommandModule(ITagService tagService) : GIRCommandModule
{
    [RequireContext<ApplicationCommandContext>(RequiredContext.Guild),
     SlashCommand("tag", "Gets a tag by name", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> GetTag(string name)
    {
        var tag = await tagService.GetTagAsync(name);

        if (tag == null)
        {
            return ErrorResponse($"No tag found with the name '{name}'");
        }

        var tagContainer = TagContainer.CreateFrom(tag);
        return ContainerResponse(tagContainer);
    }
}