using CarDomainLib;

namespace AspDotNetCoreRazorPages.Data
{
    public interface ICarData
    {
        public Task<IList<Car>> GetAllCars();
    }
}
