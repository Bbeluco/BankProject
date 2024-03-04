using System.ComponentModel.DataAnnotations;

namespace BankProject;

public class ReceivableEditDTO
{
    [Range(1, double.MaxValue)]
    public double? Value { get; set; }
    [RangeDate(-100)]
    public DateTime? Date { get; set; }
}
