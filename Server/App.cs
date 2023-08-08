public class App
{
    private readonly ServerService _serverService;

    public App(ServerService serverService)
    {
        _serverService = serverService;
    }

    public async Task Run(string[] args)
    {
        await _serverService.StartListening();
    }
}