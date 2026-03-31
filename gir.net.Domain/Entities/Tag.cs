using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gir.net.Domain.Entities;

[Table("tags")]
public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    [MaxLength(4096)]
    public string Content { get; set; }

    [MaxLength(128)]
    public string AddedByTag { get; set; }
    
    public long AddedById { get; set; }

    public DateTime AddedDate { get; set; }

    public int UseCount { get; set; } = 0;
    
    public string? ImageUrl { get; set; }
}
