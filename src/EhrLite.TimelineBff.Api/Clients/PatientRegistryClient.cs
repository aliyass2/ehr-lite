using System.Net.Http.Json;
using EhrLite.TimelineBff.Api.Contracts;

namespace EhrLite.TimelineBff.Api.Clients;

public sealed class PatientRegistryClient(HttpClient http)
{
    public Task<PatientDto?> GetPatientAsync(Guid patientId, CancellationToken ct)
        => http.GetFromJsonAsync<PatientDto>($"/api/patients/{patientId}", ct);
}
