using Microsoft.EntityFrameworkCore;

namespace EhrLite.ClinicalDocs.Api.Data;

public class ClinicalDocsDbContext(DbContextOptions<ClinicalDocsDbContext> options)
    : DbContext(options)
{
}
