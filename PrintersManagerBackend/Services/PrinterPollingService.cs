
using PrintersManagerBackend.Interfaces;

namespace PrintersManagerBackend.Services
{
    public class PrinterPollingService : BackgroundService
    {
        private TimeSpan _polingIntervar = TimeSpan.FromSeconds(30);
        private readonly IServiceScopeFactory _scopeFactory;

        public PrinterPollingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var printerService = scope.ServiceProvider.GetRequiredService<IPrinterService>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    await printerService.CheckPrintersStatisticsAsync();
                    await Task.Delay(_polingIntervar, stoppingToken);
                }
            }  
            

        }
    }
}
