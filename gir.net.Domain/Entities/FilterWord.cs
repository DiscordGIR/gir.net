using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gir.net.Domain.Entities;

[Table("filter_words")]
public class FilterWord
{
    public int Id { get; set; }
    public string Word { get; set; }
    public bool Notify { get; set; }
    public DateTime CreatedAt { get; set; } = new DateTime();
    public DateTime UpdatedAt { get; set; } = new DateTime();
    public int BypassLevel { get; set; }
    public bool FalsePositive { get; set; } = false;
    public bool SilentFilter { get; set; } = false;
    public bool PiracyWord { get; set; } = false;
}