using gir.net.Application.Interfaces.Services;
using gir.net.Domain.Exceptions;
using gir.net.Views;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ComponentInteractions;

namespace gir.net.Modules.Tag;

public class TagCreation_Modal(ITagService tagService) : ComponentInteractionModule<ModalInteractionContext>
{
    private static readonly ErrorView ErrorView = new();
    private static readonly SuccessView SuccessView = new();
    private static readonly TagView TagView = new();

    [ComponentInteraction("create-tag")]
    public async Task Modal(string tagName)
    {
        var content = Context.Components
            .OfType<Label>()
            .Select(l => l.Component)
            .OfType<TextInput>()
            .FirstOrDefault(t => t.CustomId == "create-tag-content")
            ?.Value;

        if (string.IsNullOrWhiteSpace(content))
        {
            var callback = InteractionCallback.Message(ErrorMessage("Tag content is required."));
            await Context.Interaction.SendResponseAsync(callback);
            return;
        }

        var attachments = Context.Components
            .OfType<Label>()
            .Select(l => l.Component)
            .OfType<FileUpload>()
            .SelectMany(f => f.Attachments)
            .ToList();

        if (attachments.Exists(a => !IsImageAttachment(a)))
        {
            var callback = InteractionCallback.Message(ErrorMessage("All uploaded files must use an image content type (MIME type starting with image."));
            await Context.Interaction.SendResponseAsync(callback);
            return;
        }

        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());

        var tag = new Domain.Entities.Tag
        {
            Name = tagName,
            AddedByTag = Context.User.Username,
            AddedById = (long)Context.User.Id,
            Content = content,
        };

        try
        {
            if (attachments.Count > 0)
            {
                var urls = await Task.WhenAll(attachments.Select(UploadImage));
                tag.ImageUrls = [..urls];
            }

            await tagService.AddTagAsync(tag);
        }
        catch (DuplicateTagNameException)
        {
            await FollowupAsync(ErrorMessage($"Tag '{tagName}' already exists."));
            return;
        }

        var successContainer = SuccessView.CreateFrom("Tag created successfully!");
        var tagContainer = TagView.CreateFrom(tag);
        var response = new InteractionMessageProperties()
            .WithComponents([successContainer, tagContainer])
            .WithFlags(MessageFlags.IsComponentsV2);

        await FollowupAsync(response);
    }

    private static bool IsImageAttachment(Attachment attachment)
    {
        var contentType = attachment.ContentType;
        return !string.IsNullOrEmpty(contentType)
            && contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }

    private static InteractionMessageProperties ErrorMessage(string message)
    {
        var container = ErrorView.CreateFrom(message);
        return new InteractionMessageProperties()
            .WithComponents([container])
            .WithFlags(MessageFlags.Ephemeral | MessageFlags.IsComponentsV2);
    }

    private async Task<string> UploadImage(Attachment image)
    {
        using var httpClient = new HttpClient();
        await using var stream = await httpClient.GetStreamAsync(image.Url);
        await using var inMemoryStream = new MemoryStream();
        await stream.CopyToAsync(inMemoryStream);
        inMemoryStream.Seek(0, SeekOrigin.Begin);

        return await tagService.UploadTagImage(inMemoryStream, image.FileName,
            image.ContentType ?? "application/octet-stream");
    }
}
