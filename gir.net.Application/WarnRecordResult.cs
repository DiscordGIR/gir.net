using gir.net.Domain.Entities;

namespace gir.net.Application;

public sealed record WarnRecordResult(Case Case, int CurrentWarnPoints, bool WasWarnKicked);
