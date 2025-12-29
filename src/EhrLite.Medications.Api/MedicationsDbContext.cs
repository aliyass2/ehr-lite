using Microsoft.EntityFrameworkCore;

namespace EhrLite.Medications.Api.Data;

public class MedicationsDbContext(DbContextOptions<MedicationsDbContext> options)
    : DbContext(options)
{
}
