using gir.net.Application.Interfaces.Services;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

[SlashCommand("tags", "Manage tags", Contexts = [InteractionContextType.Guild])]
public class TagAdminCommandModule(ITagService tagService) : ApplicationCommandModule<ApplicationCommandContext>
{
    [SubSlashCommand("add", "Create new tag")]
    public async Task<InteractionMessageProperties> AddTag(string name, string content, Attachment? image = null)
    {
        var tag = new Domain.Entities.Tag 
        { 
            Name = name, 
            Content = content,
            AddedByTag = Context.User.Username,
            AddedById = (long)Context.User.Id
        };

        if (image != null)
        {
            using var httpClient = new HttpClient();
            await using var stream = await httpClient.GetStreamAsync(image.Url); 
            await using var inMemoryStream = new MemoryStream();
            await stream.CopyToAsync(inMemoryStream);
            inMemoryStream.Seek(0, SeekOrigin.Begin);
            
            await tagService.AddTagWithImageAsync(tag, inMemoryStream, image.FileName, image.ContentType ?? "application/octet-stream");
        }
        else
        {
            await tagService.AddTagAsync(tag);
        }

        var message = new InteractionMessageProperties()
            .WithContent($"Tag '{name}' added!");

        return message;
    }
}