using Client.Services;
using Microsoft.Extensions.Logging;

namespace Client;

public class App
{
    private readonly ClientService _clientService;
    private readonly ILogger<ClientService> _logger;

    public App(ClientService clientService, ILogger<ClientService> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    public async Task Run(string[] args)
    {
        try
        {
            await Task.Delay(2000);
            _clientService.SendData("Hello world!");
            await Task.Delay(2000);
            _clientService.SendData("Hello Every one!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception");
        }
        finally
        {
        }

        Console.ReadLine();
    }
}
