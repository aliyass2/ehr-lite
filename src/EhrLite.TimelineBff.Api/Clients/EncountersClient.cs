using System.Net.Http.Json;
using EhrLite.TimelineBff.Api.Contracts;

namespace EhrLite.TimelineBff.Api.Clients;

public sealed class EncountersClient(HttpClient http)
{
    public async Task<IReadOnlyList<EncounterDto>> ListAsync(Guid patientId, CancellationToken ct)
        => await http.GetFromJsonAsync<List<EncounterDto>>($"/api/patients/{patientId}/encounters", ct)
           ?? [];
}
