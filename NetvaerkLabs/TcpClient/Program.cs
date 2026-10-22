using System.Net.Sockets;
using System.Text;

try
{
    string host = args.Length > 0 ? args[0] : "127.0.0.1";
    int port = args.Length > 1 ? int.Parse(args[1]) : 6000;
    using var client = new TcpClient();
    using var connectTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    await client.ConnectAsync(host, port, connectTimeout.Token);
    using NetworkStream stream = client.GetStream();
    using var reader = new StreamReader(stream, new UTF8Encoding(false));
    using var writer = new StreamWriter(stream, new UTF8Encoding(false))
    {
        AutoFlush = true,
        NewLine = "\n"
    };
    Console.WriteLine("Forbundet. Skriv PING eller tekst. Tom linje afslutter.");
    while (Console.ReadLine() is { Length: > 0 } line)
    {
        using var replyTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        string? reply = await ExchangeAsync(writer, reader, line, replyTimeout.Token);
        if (reply is null) { Console.WriteLine("Serveren lukkede forbindelsen."); break; }
        Console.WriteLine($"Svar: {reply}");
    }
}
catch (OperationCanceledException) { Console.WriteLine("TCP: intet svar inden 5 sekunder. Forbindelsen lukkes."); }
catch (IOException ex) { Console.WriteLine($"TCP-forbindelsesfejl: {ex.Message}"); }
catch (SocketException ex) { Console.WriteLine($"TCP-netværksfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Porten skal være et heltal."); }

static async Task<string?> ExchangeAsync(StreamWriter writer, StreamReader reader, string line, CancellationToken token)
{
    Console.WriteLine("TODO Grøn A: send en linje og læs svaret.");
    await Task.CompletedTask;
    return null;
}
