using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    int port = args.Length > 0 ? int.Parse(args[0]) : 6200;
    using var receiver = new UdpClient(new IPEndPoint(IPAddress.Any, port));
    Console.WriteLine($"Discovery lytter på UDP {port}. TCP-tjenesten startes separat på 6000.");
    while (true)
    {
        UdpReceiveResult packet = await receiver.ReceiveAsync();
        if (Encoding.UTF8.GetString(packet.Buffer) != "FIND_BOGSERVER") continue;
        byte[] reply = Encoding.UTF8.GetBytes("BOGSERVER tcp=6000");
        await receiver.SendAsync(reply, packet.RemoteEndPoint);
        Console.WriteLine($"Svaret direkte til {packet.RemoteEndPoint}");
    }
}
catch (SocketException ex) { Console.WriteLine($"Discovery-serverfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Porten skal være et heltal."); }
