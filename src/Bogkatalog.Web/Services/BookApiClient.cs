using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Bogkatalog.Web.Models;

namespace Bogkatalog.Web.Services;

public class BookApiClient(IHttpClientFactory factory)
{
    public async Task<List<Book>> GetAllAsync()
    {
        using var http = factory.CreateClient("Books");
        using var response = await http.GetAsync("api/books");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Book>>()
            ?? throw new JsonException("API returnerede null i stedet for en bogliste.");
    }

    public async Task<Book> AddAsync(BookInput input)
    {
        using var http = factory.CreateClient("Books");
        using var response = await http.PostAsJsonAsync("api/books", input);
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<ValidationResponse>();
            var errors = problem?.Errors ?? new();
            throw new BookValidationException(errors);
        }
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Book>()
            ?? throw new JsonException("API returnerede null i stedet for den oprettede bog.");
    }
}

public class ValidationResponse
{
    public Dictionary<string, string[]> Errors { get; set; } = new();
}

public class BookValidationException(Dictionary<string, string[]> errors)
    : Exception("API'et afviste bogens oplysninger.")
{
    public Dictionary<string, string[]> Errors { get; } = errors;
}
