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