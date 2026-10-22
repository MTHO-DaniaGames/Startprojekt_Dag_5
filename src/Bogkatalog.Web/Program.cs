using Bogkatalog.Web.Components;
using Bogkatalog.Web.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
// API-clienten opretter en named HttpClient til hvert kald.
builder.Services.AddHttpClient("Books", http =>
{
    var address = builder.Configuration["BooksApi:BaseUrl"]
        ?? throw new InvalidOperationException("BooksApi:BaseUrl mangler.");
    http.BaseAddress = new Uri(address);
    http.Timeout = TimeSpan.FromSeconds(5);
});
builder.Services.AddScoped<BookApiClient>();
var app = builder.Build();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();

