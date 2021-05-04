using System.Threading.Tasks;

namespace RCS.Sudoku.WpfApplication.Contracts.Services
{
    public interface IApplicationHostService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
