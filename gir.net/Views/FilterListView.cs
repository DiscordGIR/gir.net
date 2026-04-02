using gir.net.Configurations;
using gir.net.Domain.Entities;
using NetCord;
using NetCord.Rest;
using Color = System.Drawing.Color;

namespace gir.net.Views;

public class FilterListView : BaseView, IPaginatedView<FilterWord>
{
    public ComponentContainerProperties CreateFrom(string id, int currentPage, int totalPageCount,
        IEnumerable<FilterWord> models)
    {
        var filterWords = models.ToList();
        if (!filterWords.Any())
        {
            return CreateContainer(
                [
                    new TextDisplayProperties("No filtered words found.")
                ],
                Color.DarkGreen
            );
        }

        var components = new List<IComponentContainerComponentProperties>
        {
            new TextDisplayProperties("### Filtered words"),
            new ComponentSeparatorProperties()
        };

        filterWords.ForEach(fw =>
            {
                var flags = "";
                
                if (fw.Notify)
                    flags += "🔔";
                if (fw.PiracyWord)
                    flags += "🏴‍☠️";
                if (fw.SilentFilter)
                    flags += "🤫";
                if (fw.FalsePositive)
                    flags += "🤥";
                
                flags = !string.IsNullOrEmpty(flags) ? $"\n{flags}" : string.Empty;
                
                components.Add(new TextDisplayProperties($"**{fw.Phrase}**\nBypassed by {(PermissionLevel)fw.BypassLevel}{flags}"));
                components.Add(new ComponentSeparatorProperties());
            }
        );
        
        var more = (currentPage + 1) < totalPageCount;

        var actionRow = new ActionRowProperties().WithComponents(
            [
                new ButtonProperties($"{id}:-2", "⏮️", ButtonStyle.Primary)
                    .WithDisabled(currentPage < 1),
                new ButtonProperties($"{id}:{currentPage - 1}", "⬅️", ButtonStyle.Primary)
                    .WithDisabled(currentPage < 1),
                new ButtonProperties($"{id}:{currentPage + 1}", "➡️", ButtonStyle.Primary)
                    .WithDisabled(!more),
                new ButtonProperties($"{id}:{totalPageCount+2}", "⏭️", ButtonStyle.Primary)
                    .WithDisabled(!more)
            ]
        );
        
        components.Add(actionRow);
        components.Add(new TextDisplayProperties($"-# Page {currentPage+1} of {totalPageCount}"));

        return CreateContainer(components, Color.DarkGreen);
    }
}