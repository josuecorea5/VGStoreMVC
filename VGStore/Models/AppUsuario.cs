using Microsoft.AspNetCore.Identity;

namespace VGStore.Models
{
    public class AppUsuario : IdentityUser
    {
        public string FullName { get; set; }
    }
}
