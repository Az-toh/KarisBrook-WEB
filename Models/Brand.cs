using System.Collections.Generic;

namespace KarisBrook.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; } // Путь к логотипу бренда

        public virtual ICollection<Product> Products { get; set; }
    }
}
namespace Karisbrook.Models
{
    public class Brand
    {
    }
}
