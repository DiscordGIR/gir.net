using gir.net.Application.Interfaces.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

public class TagCommandModule(ITagService tagService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("tag", "Gets a tag by name", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> GetTag(string name)
    {
        var tag = await tagService.GetTagAsync(name);

        if (tag == null)
        {
            return RespondError($"No tag found with the name '{name}'");
        }

        var components = new List<IComponentContainerComponentProperties>
        {
            new TextDisplayProperties($"# {tag.Name}"),
            new TextDisplayProperties($"{tag.Content}"),
        };
        
        if (!string.IsNullOrEmpty(tag.ImageUrl))
        {
            components.Add(new MediaGalleryProperties([new MediaGalleryItemProperties(tag.ImageUrl)]));
        }

        components.Add(new TextDisplayProperties($"-# Created by {tag.AddedByTag} | Used {tag.UseCount} times"));

        var container = new ComponentContainerProperties()
            .WithComponents(
                components
            )
            .WithAccentColor(new(System.Drawing.Color.Salmon.ToArgb()));


        var message = new InteractionMessageProperties()
            .AddComponents(container)
            .WithFlags(MessageFlags.IsComponentsV2);


        return message;
    }


    public InteractionMessageProperties RespondError(string error)
    {
        var embed = new EmbedProperties()
            .WithTitle(":(\nAn error occured")
            .WithDescription(error)
            .WithColor(new Color(255, 0, 0));

        var message = new InteractionMessageProperties()
            .WithEmbeds([embed]);

        return message;
    }
}