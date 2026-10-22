using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    // Uden argument bruges broadcast. 127.0.0.1 giver en lokal unicast-kontrol.
    IPAddress target = args.Length > 0 ? IPAddress.Parse(args[0]) : IPAddress.Broadcast;
    int port = args.Length > 1 ? int.Parse(args[1]) : 6200;
    using var client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
    client.EnableBroadcast = true;
    byte[] query = Encoding.UTF8.GetBytes("FIND_BOGSERVER");
    await client.SendAsync(query, new IPEndPoint(target, port));
    Console.WriteLine($"Søger via {target}:{port}. Venter højst 3 sekunder.");
    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(3));
    try
    {
        // Saml svar indtil fristen. Flere servere kan svare på samme søgning.
        while (true)
        {
            UdpReceiveResult reply = await client.ReceiveAsync(timeout.Token);
            string message = Encoding.UTF8.GetString(reply.Buffer);
            if (message == "BOGSERVER tcp=6000")
                Console.WriteLine($"Fundet {reply.RemoteEndPoint.Address}:6000 via UDP-svar: {message}");
        }
    }
    catch (OperationCanceledException) { Console.WriteLine("Søgning afsluttet. Et manglende svar beviser ikke, at serveren er væk."); }
}
catch (SocketException ex) { Console.WriteLine($"Discovery-clientfejl: {ex.Message}"); }
catch (FormatException) { Console.WriteLine("Brug en IPv4-adresse og en port som heltal."); }
