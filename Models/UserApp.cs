using Microsoft.AspNetCore.Identity;

namespace Dewi.Models
{
    public class UserApp:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
