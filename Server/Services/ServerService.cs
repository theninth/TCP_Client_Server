using System.Net.Sockets;
using System.Net;
using Microsoft.Extensions.Logging;

public class ServerService
{
    TcpListener? _server;
    private readonly ILogger<ServerService> _logger;

    public string Ip { get; set; } = "127.0.0.1";

    public ushort Port { get; set; } = 6001;

    public ServerService(ILogger<ServerService> logger)
    {
        _logger = logger;
    }

    public async Task StartListening()
    {
        try
        {
            IPAddress addr = IPAddress.Parse(Ip);
            _server = new TcpListener(addr, Port);
            _server.Start();

            var bytes = new Byte[256];
            string? data;

            while (true)
            {
                _logger.LogInformation("Waiting for a connection on {Ip}:{Port}", Ip, Port);

                using TcpClient client = await _server.AcceptTcpClientAsync();
                _logger.LogInformation("Connected!");

                data = null;

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = await stream.ReadAsync(bytes)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    var respText = data.ToUpper();
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(respText);

                    stream.Write(msg, 0, msg.Length);
                    _logger.LogInformation("Sent: {text}", respText);
                }
            }
        }
        catch (SocketException e)
        {
            _logger.LogError(e, "Unhandled exception");
        }
        finally
        {
            _server?.Stop();
        }
    }
}
