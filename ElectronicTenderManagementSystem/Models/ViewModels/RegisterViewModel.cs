using System.ComponentModel.DataAnnotations;

namespace ETTS.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required] public string Username { get; set; } = default!;
        [Required][EmailAddress] public string Email { get; set; } = default!;
        [Required][DataType(DataType.Password)] public string Password { get; set; } = default!;
        [Required] public string CompanyName { get; set; } = default!;
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}
