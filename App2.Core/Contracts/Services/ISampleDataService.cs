using System.Collections.Generic;
using System.Threading.Tasks;

using App2.Core.Models;

namespace App2.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetContentGridDataAsync();

        Task<IEnumerable<SampleOrder>> GetGridDataAsync();
    }
}
