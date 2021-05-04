using System;

namespace RCS.Sudoku.WpfApplication.Contracts.Services
{
    public interface IApplicationInfoService
    {
        Version GetVersion();
    }
}
