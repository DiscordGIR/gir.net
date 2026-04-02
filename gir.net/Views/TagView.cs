using System.Text.RegularExpressions;
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
        };

        tag.Content.Split('\n').ToList().ForEach(line =>
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }
            
            // ~{text}[link]{text}[link]
            var pattern = @"^\~(?:\{([^{}]*)\}\[([^\]]*)\]){1,5}$";
            var regex = new Regex(pattern, RegexOptions.CultureInvariant);
            var match = regex.Match(line);
            if (!match.Success)
            {
                components.Add(new TextDisplayProperties(line));
                return;
            }

            var texts = match.Groups[1].Captures;
            var links = match.Groups[2].Captures;
            var buttons = new List<LinkButtonProperties>();
            for (var i = 0; i < texts.Count; i++)
                buttons.Add(new LinkButtonProperties(links[i].Value, texts[i].Value));

            components.Add(new ActionRowProperties(buttons));
        });

        if (!string.IsNullOrEmpty(tag.ImageUrl))
        {
            components.Add(new MediaGalleryProperties([new MediaGalleryItemProperties(tag.ImageUrl)]));
        }

        components.Add(new TextDisplayProperties($"-# Created by {tag.AddedByTag} | Used {tag.UseCount} times"));
        return CreateContainer(components);
    }

    public ComponentContainerProperties CreateFrom(string title, Tag tag)
    {
        throw new NotImplementedException();
    }
}