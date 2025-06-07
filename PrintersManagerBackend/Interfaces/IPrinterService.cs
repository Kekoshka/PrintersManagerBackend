using PrintersManagerBackend.Models;
using System.Timers;

namespace PrintersManagerBackend.Interfaces
{
    public interface IPrinterService
    {
        Task CheckPrintersStatisticsAsync();
    }
}
