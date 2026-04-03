using NetCord.Rest;

namespace gir.net.Views;

public class TagCreationModal : BaseView, IModalView<string>
{
    public ModalProperties CreateFrom(string tagName)
    {
        var modal = new ModalProperties($"create-tag:{tagName}", $"Create new tag: {tagName}")
            .WithComponents([
                    new LabelProperties("Content",
                        new TextInputProperties("create-tag-content", TextInputStyle.Paragraph)
                            .WithPlaceholder("You can add up to 5 link buttons per line using the following format on a new line: ~{text}[link]{text}[link]")
                    ),
                    new LabelProperties("Images",
                        new FileUploadProperties("create-tag-images")
                            .WithRequired(false)
                            .WithMaxValues(5)
                    )
                ]
            );

        return modal;
    }
}