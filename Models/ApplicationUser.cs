using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace KarisBrook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        // Навигационные свойства
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
namespace Karisbrook.Models
{
    public class ApplicationUser
    {
    }
}
