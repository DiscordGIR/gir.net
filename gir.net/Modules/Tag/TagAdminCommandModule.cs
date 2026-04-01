using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using gir.net.Views;
using NetCord;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules;

[SlashCommand("tags", "Manage tags")]
public class TagAdminCommandModule(ITagService tagService) : GIRBaseCommandModule
{
    private static readonly TagView _tagView = new();

    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SubSlashCommand("add", "Create new tag")]
    public async Task<InteractionMessageProperties> AddTag(string name, string content, Attachment? image = null)
    {
        var existingTag = await tagService.GetTagAsync(name);
        if (existingTag != null)
        {
            return ErrorResponse($"Tag '{name}' already exists.");
        }
        
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
        
        var tagContainer = _tagView.CreateFrom(tag);

        return SuccessResponse("Tag created successfully!", tagContainer);
    }
}