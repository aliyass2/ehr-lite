using System.Net.Http.Json;
using EhrLite.TimelineBff.Api.Contracts;

namespace EhrLite.TimelineBff.Api.Clients;

public sealed class AuditClient(HttpClient http)
{
    public async Task<IReadOnlyList<AuditDto>> ListAsync(Guid patientId, CancellationToken ct)
        => await http.GetFromJsonAsync<List<AuditDto>>($"/api/patients/{patientId}/audit", ct)
           ?? [];
}
