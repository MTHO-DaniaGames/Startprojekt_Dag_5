using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    string host = args.Length > 0 ? args[0] : "127.0.0.1";
    int port = args.Length > 1 ? int.Parse(args[1]) : 6100;
    var endpoint = new IPEndPoint(IPAddress.Parse(host), port);
    using var sender = new UdpClient(AddressFamily.InterNetwork);
    for (int number = 1; number <= 5; number++)
    {
        string message = $"{number}: HEJ";
        await SendMessageAsync(sender, endpoint, message);
        Console.WriteLine($"Sendt lokalt til netværksstakken: {message}");
        await Task.Delay(200);
    }
}
catch (SocketException ex) { Console.WriteLine($"UDP-senderfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Brug en IPv4-adresse og en port som heltal."); }

static async Task SendMessageAsync(UdpClient sender, IPEndPoint endpoint, string message)
{
    Console.WriteLine("TODO Grøn C: kod UTF-8 og send datagrammet.");
    await Task.CompletedTask;
}
