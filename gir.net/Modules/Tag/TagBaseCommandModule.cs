using gir.net.Application.Interfaces.Services;
using gir.net.Infra;
using gir.net.Modules.Tag;
using gir.net.Views;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

public class TagBaseCommandModule(ITagService tagService) : GIRBaseCommandModule
{
    [SlashCommand("tag", "Gets a tag by name")]
    public async Task<InteractionMessageProperties> GetTag(
        [SlashCommandParameter(Name = "name", Description = "Name of the tag",
            AutocompleteProviderType = typeof(TagAutocompleteProvider))]
        string name
    )
    {
        var tag = await tagService.GetTagAsync(name);

        if (tag == null)
        {
            return ErrorResponse($"No tag found with the name '{name}'");
        }

        var tagContainer = TagView.CreateFrom(tag);
        await tagService.MarkTagUsage(tag);

        return ContainerResponse(tagContainer, ephemralIfNoob: false);
    }
}