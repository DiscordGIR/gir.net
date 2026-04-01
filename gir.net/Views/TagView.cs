using gir.net.Domain.Entities;
using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public sealed class TagView : BaseView, IView<Tag>
{
    public ComponentContainerProperties CreateFrom(Tag tag)
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
        return CreateContainer(components, Color.Salmon);
    }
}