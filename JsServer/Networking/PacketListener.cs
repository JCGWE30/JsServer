
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JsServer;

public class PacketListener
{
    private TcpListener listener;
    private Dictionary<TcpClient, Connection> clients = new Dictionary<TcpClient, Connection>();
    public PacketListener()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Any, 25525);
        listener = new TcpListener(ipEndPoint);
        listener.Start();
        Console.WriteLine($"Listener started on {ipEndPoint.Address}:{ipEndPoint.Port}");
    }

    public void listenerTick()
    {
        acceptNewClients();
        dropStaleClients();
        processClients();
    }

    private void acceptNewClients()
    {
        
        while (listener.Pending())
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected");
            clients[client] = new Connection(client);
        }
    }

    private void dropStaleClients()
    {
        List<TcpClient> staleClients = new List<TcpClient>();
        foreach(KeyValuePair<TcpClient, Connection> entry in clients)
        {
            if (entry.Value.shouldDrop||!entry.Key.Connected)
            {
                staleClients.Add(entry.Key);
            }
        }

        foreach (var client in staleClients)
        {
            Console.Write("Client disconnected");
            client.Close();
            clients.Remove(client);
        }
    }

    private void processClients()
    {
        foreach(var entry in clients)
        {
            var client = entry.Key;
            var connection = entry.Value;

            NetworkStream stream = client.GetStream();
            
            byte[] buffer = new byte[1024];

            if (stream.DataAvailable)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    // Console.WriteLine($"Read {bytesRead} bytes");
                    // Console.WriteLine(String.Join(",", buffer));
                    connection.processBytes(buffer);
                }
            }
        }
    }
}