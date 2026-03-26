using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gir.net.Domain.Entities;

[Table("users")]
public class User
{
    [Key]
    public long Id { get; set; }

    public int Xp { get; set; } = 0;
}
