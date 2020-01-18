using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;
using TakeMeThereXamarinForms.Repositories;
using TakeMeThereXamarinForms.Services;

namespace TakeMeXamarinForms.Sqlite.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private AppDbContext _dbContext;

        public PlaceRepository(IPlatformInfoService dbpathService)
        {
            this._dbContext = new AppDbContext(dbpathService.GetUserDataFolderPath());
        }

        public async Task<bool> AddAsync(Place item)
        {
            try
            {
                var tracking = await _dbContext.AddAsync(item);
                await _dbContext.SaveChangesAsync();
                var isAdded = tracking.State == EntityState.Added;
                return isAdded;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteAsync(Place item)
        {
            try
            {
                var tracking = _dbContext.Remove(item);
                await _dbContext.SaveChangesAsync();
                var isDeleted = tracking.State == EntityState.Deleted;
                return isDeleted;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<Place>> ReadAll()
        {
            try
            {
                var places = await _dbContext.Places.ToListAsync();
                return places;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateAsync(Place item)
        {
            try
            {
                var tracking = _dbContext.Update(item);
                await _dbContext.SaveChangesAsync();
                var isUpdated = tracking.State == EntityState.Modified;
                return isUpdated;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
