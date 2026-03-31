using gir.net.Application.Interfaces.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules.Tag;

public class TagAutocompleteProvider(ITagService tagService) : IAutocompleteProvider<AutocompleteInteractionContext>
{
    public async ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(ApplicationCommandInteractionDataOption option, AutocompleteInteractionContext context)
    {
        var userInput = option.Value ?? string.Empty;
        var tags = await tagService.SearchTagsAsync(userInput);

        return tags
            .Select(t => new ApplicationCommandOptionChoiceProperties(t.Name, t.Name))
            .Take(25)
            .ToList();
    }
}