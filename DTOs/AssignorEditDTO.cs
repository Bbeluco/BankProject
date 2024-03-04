using System.ComponentModel.DataAnnotations;

namespace BankProject;

public class AssignorEditDTO
{
    [StringLength(30, MinimumLength = 11)]
    public string? Document { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(140, MinimumLength = 0)]
    public string? Name { get; set; }
}
