using System.Net.Http.Json;
using EhrLite.TimelineBff.Api.Contracts;

namespace EhrLite.TimelineBff.Api.Clients;

public sealed class LabsDocsClient(HttpClient http)
{
    public async Task<IReadOnlyList<LabResultDto>> ListAsync(Guid patientId, CancellationToken ct)
        => await http.GetFromJsonAsync<List<LabResultDto>>($"/api/patients/{patientId}/labs", ct)
           ?? [];
}
