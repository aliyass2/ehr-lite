using Microsoft.EntityFrameworkCore;

namespace EhrLite.Audit.Api.Data;

public class AuditDbContext(DbContextOptions<AuditDbContext> options)
    : DbContext(options)
{
}
