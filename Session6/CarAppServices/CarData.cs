using CarDomainLib;
using Microsoft.EntityFrameworkCore;

namespace AspDotNetCoreRazorPages.Data
{
    public class CarData : ICarData
    {
        private readonly CarDbContext _dbContext;

        public CarData(CarDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Car>> GetAllCars()
        {
            return await _dbContext.Car.ToListAsync();
        }
    }
}
