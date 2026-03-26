using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gir.net.Infra.Database;

[Table("tags")]
public class Tag
{
    [Key]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(4096)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(128)]
    public string AddedByTag { get; set; } = string.Empty;
    
    public long AddedById { get; set; } = long.MaxValue;

    public DateTime AddedDate { get; set; } = DateTime.UtcNow;

    public int UseCount { get; set; } = 0;
    
    public string? ImageUrl { get; set; }

    public virtual ICollection<ButtonLink> ButtonLinks { get; set; } = new List<ButtonLink>();
}
