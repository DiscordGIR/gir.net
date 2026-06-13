using System.ComponentModel.DataAnnotations.Schema;

namespace gir.net.Domain.Entities;

[Table(("cases"))]
public class Case
{
    public int Id { get; set; }
    public CaseType Type { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public ulong UserId { get; set; }
    public ulong ModId { get; set; }
    public string ModTag { get; set; }
    public string Reason { get; set; }
    public string Punishment { get; set; }
    public bool Lifted { get; set; } = false;
    public string? LiftedByTag { get; set; }
    public ulong? LiftedById { get; set; }
    public string? LiftedReason { get; set; }
    public DateTime? LiftedDate { get; set; }
}

public enum CaseType
{
    Warn = 1,
    LiftWarn = 2,
    RemovePoints = 3,
    Mute = 4,
    Kick = 5,
    Ban = 6,
    Unban = 7,
    Clem = 8
}
