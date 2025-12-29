using Microsoft.EntityFrameworkCore;

namespace EhrLite.LabsDocs.Api.Data;

public class LabsDocsDbContext(DbContextOptions<LabsDocsDbContext> options)
    : DbContext(options)
{
}
