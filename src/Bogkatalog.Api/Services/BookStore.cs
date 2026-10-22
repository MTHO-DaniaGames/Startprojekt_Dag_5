using Bogkatalog.Api.Contracts;
using Bogkatalog.Api.Models;

namespace Bogkatalog.Api.Services;

// Færdig hjælp til øvelserne. Én delt instans i API-processen.
// lock beskytter listen, hvis requests kommer samtidigt.
// Book er immutable, og GetAll returnerer en kopi af listen.
public class BookStore
{
    private readonly object gate = new();
    private readonly List<Book> books =
    [
        new(1, "Hobbitten", "J.R.R. Tolkien", 310),
        new(2, "Frankenstein", "Mary Shelley", 280)
    ];
    private int nextId = 3;

    public Book[] GetAll()
    {
        lock (gate) return books.ToArray();
    }

    public Book? GetById(int id)
    {
        lock (gate) return books.Find(book => book.Id == id);
    }

    // Endpointet skal validere input før Add eller Update.
    public Book Add(BookInput input)
    {
        lock (gate)
        {
            var book = new Book(nextId++, input.Title?.Trim() ?? "",
                input.Author?.Trim() ?? "", input.Pages);
            books.Add(book);
            return book;
        }
    }

    public Book? Update(int id, BookInput input)
    {
        lock (gate)
        {
            int index = books.FindIndex(book => book.Id == id);
            if (index < 0) return null;
            var book = new Book(id, input.Title?.Trim() ?? "",
                input.Author?.Trim() ?? "", input.Pages);
            books[index] = book;
            return book;
        }
    }

    public bool Delete(int id)
    {
        lock (gate)
        {
            int index = books.FindIndex(book => book.Id == id);
            if (index < 0) return false;
            books.RemoveAt(index);
            return true;
        }
    }
}
