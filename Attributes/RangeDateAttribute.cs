using System.ComponentModel.DataAnnotations;

namespace BankProject;

public class RangeDateAttribute : RangeAttribute
{
    public RangeDateAttribute(int yearsBack)
        : base(typeof(DateTime), 
            DateTime.Now.AddYears(yearsBack).ToShortDateString(), 
            DateTime.Now.ToShortDateString()
        )
        { }
}
