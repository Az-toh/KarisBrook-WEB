using System.Collections.Generic;

namespace KarisBrook.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Product> BestSellers { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}

