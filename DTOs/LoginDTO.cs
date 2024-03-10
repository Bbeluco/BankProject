using System.ComponentModel.DataAnnotations;

namespace BankProject;

public class LoginDTO
{
    [StringLength(16, MinimumLength = 8)]
    public string Login { get; set; }
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; }
}
