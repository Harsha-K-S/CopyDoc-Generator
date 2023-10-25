using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTool
{
    [JsonConverter(typeof(ApplicationUserJsonConverter))]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public ApplicationUser()
        {
            Id = string.Empty;
        }
    }
}