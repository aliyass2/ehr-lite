using Microsoft.EntityFrameworkCore;

namespace EhrLite.Encounters.Api.Data;

public class EncounterDbContext(DbContextOptions<EncounterDbContext> options)
    : DbContext(options)
{
}
