using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    int port = args.Length > 0 ? int.Parse(args[0]) : 6100;
    IPAddress address = args.Contains("--lan") ? IPAddress.Any : IPAddress.Loopback;
    using var receiver = new UdpClient(new IPEndPoint(address, port));
    int received = 0;
    Console.WriteLine($"UDP lytter på {address}:{port}. Stop med Ctrl+C.");
    while (true)
    {
        UdpReceiveResult packet = await receiver.ReceiveAsync();
        received++;
        if (args.Contains("--drop-every-3") && received % 3 == 0)
        {
            Console.WriteLine("SIMULERET: programmet ignorerer denne modtagne besked.");
            continue;
        }
        string message = Encoding.UTF8.GetString(packet.Buffer);
        Console.WriteLine($"Fra {packet.RemoteEndPoint}: {message}");
    }
}
catch (SocketException ex) { Console.WriteLine($"UDP-modtagerfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Porten skal være et heltal."); }
