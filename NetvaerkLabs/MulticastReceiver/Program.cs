using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    var group = IPAddress.Parse("239.255.42.99");
    IPAddress localInterface = args.Length > 0 ? IPAddress.Parse(args[0]) : IPAddress.Any;
    int port = args.Length > 1 ? int.Parse(args[1]) : 6300;
    using var receiver = new UdpClient(AddressFamily.InterNetwork);
    receiver.ExclusiveAddressUse = false;
    receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
    receiver.Client.Bind(new IPEndPoint(IPAddress.Any, port));
    receiver.JoinMulticastGroup(group, localInterface);
    Console.WriteLine($"Multicast {group}:{port}, interface {localInterface}. Stop med Ctrl+C.");
    while (true)
    {
        UdpReceiveResult packet = await receiver.ReceiveAsync();
        Console.WriteLine($"Fra {packet.RemoteEndPoint}: {Encoding.UTF8.GetString(packet.Buffer)}");
    }
}
catch (SocketException ex) { Console.WriteLine($"Multicast-modtagerfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Brug interface-IPv4 og en port som heltal."); }
