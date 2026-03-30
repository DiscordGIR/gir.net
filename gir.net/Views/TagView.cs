using gir.net.Domain.Entities;
using NetCord.Rest;

namespace gir.net.Views;

public static class TagView
{
    public static ComponentContainerProperties CreateFrom(Tag tag)
    {
        var components = new List<IComponentContainerComponentProperties>
        {
            new TextDisplayProperties($"### {tag.Name}"),
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
        
        return container;
    }
}