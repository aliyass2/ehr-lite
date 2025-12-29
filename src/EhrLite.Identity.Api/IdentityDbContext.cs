using Microsoft.EntityFrameworkCore;

namespace EhrLite.Identity.Api.Data;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : DbContext(options)
{
}
