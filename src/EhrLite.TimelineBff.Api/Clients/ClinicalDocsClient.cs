using System.Net.Http.Json;
using EhrLite.TimelineBff.Api.Contracts;

namespace EhrLite.TimelineBff.Api.Clients;

public sealed class ClinicalDocsClient(HttpClient http)
{
    public async Task<IReadOnlyList<ClinicalNoteDto>> ListNotesAsync(Guid patientId, CancellationToken ct)
        => await http.GetFromJsonAsync<List<ClinicalNoteDto>>($"/api/patients/{patientId}/notes", ct)
           ?? [];
}
