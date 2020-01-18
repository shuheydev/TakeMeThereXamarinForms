using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;

namespace TakeMeThereXamarinForms.Repositories
{
    public class PlaceMockRepository : IPlaceRepository
    {
        private IList<Place> _placeList = new List<Place>();

        public PlaceMockRepository()
        {
            _placeList.Add(new Place
            {
                Id = 1,
                Name = "目的地1",
                PlusCode = "PlusCode1",
                Longitude = 123.4567,
                Latitude = 12.3456,
            });
            _placeList.Add(new Place
            {
                Id = 2,
                Name = "目的地2",
                PlusCode = "PlusCode2",
                Longitude = 234.5678,
                Latitude = 23.4567,
            });
            _placeList.Add(new Place
            {
                Id = 3,
                Name = "目的地3",
                PlusCode = "PlusCode3",
                Longitude = 234.5678,
                Latitude = 23.4567,
            });
        }
        public async Task<bool> AddAsync(Place item)
        {
            try
            {
                _placeList.Add(item);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Place item)
        {
            try
            {
                _placeList.Remove(item);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Place>> ReadAll()
        {
            return _placeList;
        }

        public async Task<bool> UpdateAsync(Place item)
        {
            try
            {
                var target = _placeList.FirstOrDefault(x => x.Id == item.Id); ;

                if (target != null)
                {
                    target = item;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
