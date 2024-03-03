using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BankProject;

public class PayableDTO
{
    public Receivable Receivable { get; set; }
    public Assignor Assignor { get; set; }
}

public class Receivable {
    [Required()]
    public string Id { get; set; }
    [Range(1, double.MaxValue)]
    public double Value { get; set; }
    [RangeDate(-100)]
    public DateTime Date { get; set; }
}

public class Assignor {
    [StringLength(30, MinimumLength = 11)]
    public string Document { get; set; }
    [StringLength(140, MinimumLength = 0)]
    public string Email { get; set; }
    [StringLength(20)]
    public string Phone { get; set; }

    [StringLength(140, MinimumLength = 0)]
    public string Name { get; set; }
}