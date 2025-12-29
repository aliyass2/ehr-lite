using System.Net.Http.Json;
using EhrLite.TimelineBff.Api.Contracts;

namespace EhrLite.TimelineBff.Api.Clients;

public sealed class MedicationsClient(HttpClient http)
{
    public async Task<IReadOnlyList<MedicationDto>> ListAsync(Guid patientId, CancellationToken ct)
        => await http.GetFromJsonAsync<List<MedicationDto>>($"/api/patients/{patientId}/medications", ct)
           ?? [];
}
