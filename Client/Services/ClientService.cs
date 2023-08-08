using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Client.Services;

public class ClientService
{
    private readonly ILogger<ClientService> _logger;

    public string Ip { get; set; } = "127.0.0.1";

    public ushort Port { get; set; } = 6001;

    public ClientService(ILogger<ClientService> logger)
    {
        _logger = logger;
    }

    public void SendData(string text)
    {
        try
        {
            using var tcpClient = new TcpClient(Ip, Port);

            var data = System.Text.Encoding.ASCII.GetBytes(text);
            NetworkStream stream = tcpClient.GetStream();

            stream.Write(data, 0, data.Length);
            _logger.LogInformation("Sending data: {text}", text);

            data = new Byte[256];  // reponse

            // Read the first batch of the TcpServer response bytes.
            var bytes = stream.Read(data, 0, data.Length);
            var responseText = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            _logger.LogInformation("Received: {text}", responseText);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Unhandled ArgumentNullException!");
        }
        catch (SocketException ex)
        {
            _logger.LogError(ex, "Unhandled SocketException!");
        }
    }
}
