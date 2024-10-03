using Example.WorkerService.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((host, services) =>
    {
        services.Configure<ServiceSettings>(host.Configuration.GetSection("ServiceSettings"));
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging(log =>
    {
        log.ClearProviders();
        log.AddConsole();
        log.AddEventLog(new Microsoft.Extensions.Logging.EventLog.EventLogSettings()
        {
            LogName = "ServiceWorkerApp",
            SourceName = "ServiceWorker"
        });
    })
    .Build();

await host.RunAsync();