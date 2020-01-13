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
        Task Update(Place item);
        Task Add(Place item);
        Task Delete(Place item);
    }
}
