using AspDotNetCoreRazorPages.Data;
using CarDomainLib;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspDotNetCoreRazorPages.Pages
{
    public class CarsModel : PageModel
    {
        private readonly ICarData _carData;

        public CarsModel(ICarData carData)
        {
            _carData = carData;
        }

        public IList<Car> AllCars { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AllCars = await _carData.GetAllCars();

        }
    }
}
