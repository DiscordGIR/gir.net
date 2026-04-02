using gir.net.Modules.Filter;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace gir.net.Modules.Filter.ButtonInteractions;

public class FilterListPaginationButton(FilterListPageBuilder filterListPageBuilder)
    : ComponentInteractionModule<ButtonInteractionContext>
{
    [ComponentInteraction("filter")]
    public async Task<InteractionCallbackProperties> Button(int page)
    {
        var container = await filterListPageBuilder.BuildAsync(page);
        return InteractionCallback.ModifyMessage(m => m
            .WithComponents([container])
            .WithFlags(MessageFlags.IsComponentsV2));
    }
}