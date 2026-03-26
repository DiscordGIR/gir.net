using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gir.net.Infra.Database;

public class ButtonLink
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string? TagName { get; set; }

    [ForeignKey(nameof(TagName))]
    public virtual Tag? Tag { get; set; }
}
