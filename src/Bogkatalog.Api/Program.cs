using Bogkatalog.Api.Contracts;
using Bogkatalog.Api.Services;
using Bogkatalog.Api.Validation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookStore>();
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "Bogkatalog.Api" }));

app.MapGet("/about", () => Results.Ok(new
{
    name = "Bogkatalog.Api",
    version = "1.0"
}));

app.MapGet("/api/books/{id:int}",
    IResult (int id, BookStore store) =>
{
    var book = store.GetById(id);
    if (book is null)
        return Results.NotFound();
    return Results.Ok(book);
});

app.MapPost("/api/books",
    IResult (BookInput input,
             BookStore store) =>
{
    var errors = BookRules.Validate(input);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);
    var book = store.Add(input);
    return Results.Created(
        $"/api/books/{book.Id}", book);
});

app.MapPut("/api/books/{id:int}",
    IResult (int id, BookInput input,
             BookStore store) =>
{
    var errors = BookRules.Validate(input);
    if (errors.Count > 0)
        return Results.ValidationProblem(errors);
    var book = store.Update(id, input);
    if (book is null)
        return Results.NotFound();
    return Results.Ok(book);
});

app.MapDelete("/api/books/{id:int}",
    IResult (int id, BookStore store) =>
{
    if (!store.Delete(id))
        return Results.NotFound();
    return Results.NoContent();
});

app.MapGet("/api/books",
    (string? title, BookStore store) =>
{
    var books = store.GetAll();
    if (!string.IsNullOrWhiteSpace(title))
    {
        var mode =
            StringComparison.OrdinalIgnoreCase;
        books = books.Where(book =>
            book.Title.Contains(title, mode))
            .ToArray();
    }
    return Results.Ok(books);
});

app.Run();

