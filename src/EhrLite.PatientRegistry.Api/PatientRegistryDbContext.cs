using Microsoft.EntityFrameworkCore;

namespace EhrLite.PatientRegistry.Api.Data;

public class PatientRegistryDbContext(DbContextOptions<PatientRegistryDbContext> options)
    : DbContext(options)
{
    // Add DbSet<> later. Keep empty for now.
}
