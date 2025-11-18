using Microsoft.AspNetCore.Identity;

namespace Ếch_ăn_chay.Models
{

    public enum UserRole
    {
        Admin,
        Staff,
        User
    }

    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
    }

}
