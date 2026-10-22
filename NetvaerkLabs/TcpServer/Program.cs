using System.Net;
using System.Net.Sockets;
using System.Text;

await RunAsync();
async Task RunAsync()
{
    int port = args.Length > 0 ? int.Parse(args[0]) : 6000;
    IPAddress address = args.Contains("--lan") ? IPAddress.Any : IPAddress.Loopback;
    var listener = new TcpListener(address, port);
    try
    {
        listener.Start();
        Console.WriteLine($"TCP lytter på {address}:{port}. Stop med Ctrl+C.");
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            // Grøn B: én client ad gangen. Gul: erstat linjen med _ = HandleClientAsync(client);
            await HandleClientAsync(client);
        }
    }
    catch (SocketException ex) { Console.WriteLine($"TCP-serverfejl: {ex.Message}"); }
    finally { listener.Stop(); }
}

async Task HandleClientAsync(TcpClient client)
{
    using (client)
    {
        Console.WriteLine($"Client tilsluttet: {client.Client.RemoteEndPoint}");
        try
        {
            using NetworkStream stream = client.GetStream();
            using var reader = new StreamReader(stream, new UTF8Encoding(false));
            using var writer = new StreamWriter(stream, new UTF8Encoding(false))
            {
                AutoFlush = true,
                NewLine = "\n"
            };
            Console.WriteLine("TODO Grøn B: læs linjer og send svar.");
            await Task.CompletedTask;
        }
        catch (IOException ex) { Console.WriteLine($"Clientforbindelsen sluttede: {ex.Message}"); }
        catch (SocketException ex) { Console.WriteLine($"Clientfejl: {ex.Message}"); }
        finally { Console.WriteLine("Client afsluttet."); }
    }
}
