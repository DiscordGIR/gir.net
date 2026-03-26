using gir.net.Application.Interfaces.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

public class TagCommandModule(ITagService tagService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("tag", "Gets a tag by name", Contexts = [InteractionContextType.Guild])]
    public async Task GetTag(string name)
    {
        var tag = await tagService.GetTagAsync(name);

        if (tag == null)
        {
            await RespondError($"No tag found with the name '{name}'");
            return;
        }
        
        var embed = new EmbedProperties()
            .WithTitle(tag.Name)
            .WithDescription(tag.Content)
            .WithTimestamp(tag.AddedDate)
            .WithFooter(new EmbedFooterProperties().WithText($"Added by {tag.AddedByTag} | Used {tag.UseCount} times"));

        var message = new InteractionMessageProperties()
            .WithEmbeds([embed]);
        
        await RespondAsync(InteractionCallback.Message(message));
    }

    public async Task RespondError(string error)
    {
        var embed = new  EmbedProperties()
            .WithTitle(":(\nAn error occured")
            .WithDescription(error)
            .WithColor(new  Color(255, 0, 0));
        
        var message = new InteractionMessageProperties()
            .WithEmbeds([embed]);
        
        await RespondAsync(InteractionCallback.Message(message));
    }
}