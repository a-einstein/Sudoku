using System.Threading.Tasks;

namespace RCS.Sudoku.Gui.Contracts.Services
{
    public interface IApplicationHostService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
