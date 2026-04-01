using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace gir.net.Infrastructure;

internal static class DbHelpers
{
    public static bool IsPostgresUniqueViolation(DbUpdateException ex)
    {
        for (var inner = ex.InnerException; inner != null; inner = inner.InnerException)
        {
            if (inner is PostgresException pg && pg.SqlState == PostgresErrorCodes.UniqueViolation)
                return true;
        }

        return false;
    }
}