using Client;
using Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(services.GetRequiredService<IConfiguration>())
                .Enrich.FromLogContext()
                .CreateLogger();

try
{
    await services.GetRequiredService<App>().Run(args);
}
catch (Exception e)
{
    Log.Error(e, "Unhandled exception!");
}

static IHostBuilder CreateHostBuilder(string[] strings) =>
    Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<App>();
            services.AddTransient<ClientService>();
        })
        .UseSerilog();