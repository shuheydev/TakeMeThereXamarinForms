using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;

namespace TakeMeThereXamarinForms.Repositories
{
    public interface IPlaceRepository
    {
        Task<IEnumerable<Place>> ReadAll();
        Task<bool> UpdateAsync(Place item);
        Task<bool> AddAsync(Place item);
        Task<bool> DeleteAsync(Place item);
    }
}
