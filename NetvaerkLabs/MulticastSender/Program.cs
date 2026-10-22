using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    IPAddress localInterface = args.Length > 0 ? IPAddress.Parse(args[0]) : IPAddress.Any;
    int port = args.Length > 1 ? int.Parse(args[1]) : 6300;
    using var sender = new UdpClient(new IPEndPoint(localInterface, 0));
    sender.MulticastLoopback = true;
    sender.Ttl = 1;
    if (!localInterface.Equals(IPAddress.Any))
        sender.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, localInterface.GetAddressBytes());
    byte[] bytes = Encoding.UTF8.GetBytes("HEJ GRUPPE");
    await sender.SendAsync(bytes, new IPEndPoint(IPAddress.Parse("239.255.42.99"), port));
    Console.WriteLine("Multicast sendt. Kontroller modtagervinduerne.");
}
catch (SocketException ex) { Console.WriteLine($"Multicast-senderfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Brug interface-IPv4 og en port som heltal."); }
