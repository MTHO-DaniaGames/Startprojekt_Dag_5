using Bogkatalog.Web.Models;
namespace Bogkatalog.Web.Services;
// Scoped i Interactive Server: data hører til den aktuelle circuit.
// Dette er hverken en database eller data, som alle brugere deler.
public class BookRepository
{
    private readonly List<Book> books = [
        new() { Id = 1, Title = "Hobbitten", Author = "J.R.R. Tolkien", Pages = 310 },
        new() { Id = 2, Title = "Frankenstein", Author = "Mary Shelley", Pages = 280 }
    ];
    public IReadOnlyList<Book> GetAll() => books.AsReadOnly();
    public void Add(Book book)
    {
        books.Add(new Book { Id = books.Max(b => b.Id) + 1,
            Title = book.Title.Trim(), Author = book.Author.Trim(), Pages = book.Pages });
    }
}
