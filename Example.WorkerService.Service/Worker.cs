using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Example.WorkerService.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        IOptions<ServiceSettings> _settings;

        public Worker(ILogger<Worker> logger, IOptions<ServiceSettings> settings)
        {
            _logger = logger;
            _settings = settings;

            if (!EventLog.SourceExists("ServiceWorker"))
                EventLog.CreateEventSource("ServiceWorker", "ServiceWorkerApp");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{time} - Serviço Iniciado", DateTime.Now);

            await Task.CompletedTask;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    await DoWorkAsync(stoppingToken);
                    _logger.LogInformation("{time} - Serviço sendo executado", DateTime.Now);

                    await Task.Delay(_settings.Value.DelayTimeSpan, stoppingToken);
                }
            }

            _logger.LogInformation("{time} - Serviço Finalizado", DateTime.Now);
            await StopAsync(stoppingToken);
        }

        private Task DoWorkAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (!Directory.Exists(_settings.Value.PathLog))
                    Directory.CreateDirectory(_settings.Value.PathLog);

                var newFile = Path.Combine(_settings.Value.PathLog, $"ServiceWorkerFile_{DateTime.Now:yyyyMMddmmss}.txt");
                File.WriteAllTextAsync(newFile, $"Arquivo gerado pelo WorkService as {DateTime.Now}");

                _logger.LogInformation($"Novo arquivo ({newFile}) gerado com sucesso!");
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(ex, "Erro ao criar novo arquivo.");
                };
            }

            return Task.CompletedTask;
        }
    }
}
